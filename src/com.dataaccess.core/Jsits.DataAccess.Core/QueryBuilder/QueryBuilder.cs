using Jsits.DataAccess.Core.Helpers;
using Jsits.DataAccess.Core.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Jsits.DataAccess.Core.QueryBuilder {

    public class QueryBuilder<T>: IQueryBuilder<T> where T : class, new() {

        public EntityMapping Map { get; set; }

        public QueryBuilder() { }

        public QueryBuilder(DataAccessBase dbConnection) {
            var mapper = new Mapper<T>();
            Map = mapper.Map(dbConnection);
        }

        public virtual string SelectAllStatement {
            get {
                StringBuilder sbCols = new StringBuilder();
                if (null == Map) {
                    throw new DataAccessException("No Map found for the entity");
                }

                Map.ElementMap.ForEach(map => {
                    if (map.IsNative) {
                        sbCols.AppendFormat($"{Map.ElementStartEnclosure}{map.FieldName}{Map.ElementEndEnclosure},");
                    }
                });

                return $"SELECT {sbCols.ToString().Substring(0, sbCols.Length - 1)} FROM {Map.EnclosedEntityName}";
            }
        }

        public virtual string SelectStatement(IList<QueryParameter> queryParameters) {
            var whereConditions = new List<string>();
            string whereClause = string.Empty;

            foreach (QueryParameter queryParameter in queryParameters) {
                whereConditions.Add($"{queryParameter.ParameterName} = @{queryParameter.ParameterName}");
            }

            if (whereConditions.Any()) {
                whereClause = string.Join(" AND ", whereConditions);
            }

            return $"{SelectAllStatement} {WhereClause(whereClause)}";
        }

        public virtual string InsertStatement {
            get {
                StringBuilder sbCols = new StringBuilder();
                StringBuilder sbVals = new StringBuilder();

                if (null == Map) {
                    throw new DataAccessException("No Map found for the entity");
                }

                Map.ElementMap.ForEach(map => {
                    if (map.IsNative && !map.IsPrimaryKey) {
                        sbCols.AppendFormat($"{Map.ElementStartEnclosure}{map.FieldName}{Map.ElementEndEnclosure},");
                        sbVals.AppendFormat($"@{map.FieldName},");
                    }
                });

                return $"INSERT INTO {Map.EnclosedEntityName} ({sbCols.ToString().Substring(0, sbCols.Length - 1)}) VALUES ({sbVals.ToString().Substring(0, sbVals.Length - 1)})";
            }
        }

        public virtual string UpdateStatement {
            get {
                var setParts = new List<string>();
                string whereClause = string.Empty;

                if (null == Map) {
                    throw new DataAccessException("No Map found for the entity");
                }

                Map.ElementMap.ForEach(map => {
                    if (map.IsNative) {
                        if (map.IsPrimaryKey) {
                            whereClause = $"{Map.ElementStartEnclosure}{map.FieldName}{Map.ElementEndEnclosure} = @{map.FieldName}";
                        } else {
                            setParts.Add($"{Map.ElementStartEnclosure}{map.FieldName}{Map.ElementEndEnclosure} = @{map.FieldName}");
                        }
                    }
                });

                return $"UPDATE {Map.EnclosedEntityName} SET {string.Join(",", setParts)} WHERE {whereClause}";
            }
        }

        public virtual string DeleteStatement {
            get {
                var setParts = new List<string>();
                string whereClause = string.Empty;

                if (null == Map) {
                    throw new DataAccessException("No Map found for the entity");
                }

                Map.ElementMap.ForEach(map => {
                    if (map.IsNative) {
                        if (map.IsPrimaryKey) {
                            whereClause = $"{Map.ElementStartEnclosure}{map.FieldName}{Map.ElementEndEnclosure} = @{map.FieldName}";
                        }
                    }
                });

                return $"DELETE FROM {Map.EnclosedEntityName} WHERE {whereClause}";
            }
        }

        private string WhereClause(string whereClause) => (string.IsNullOrEmpty(whereClause)) ? string.Empty : $" WHERE {whereClause}";

    }

}
