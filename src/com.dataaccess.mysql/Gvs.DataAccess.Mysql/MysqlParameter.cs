using System;
using Gvs.DataAccess.Core;
using MySql.Data.MySqlClient;

namespace Gvs.DataAccess.Mysql
{
    public class MysqlParameter : Parameter
    {

        public MysqlParameter(QueryParameter queryParameter) : base(queryParameter) { }

        public MysqlParameter(System.Data.ParameterDirection direction, string parameterName, MySqlDbType parameterType)
            : this(direction, parameterName, parameterType, null)
        {
        }

        public MysqlParameter(System.Data.ParameterDirection direction, string parameterName, MySqlDbType parameterType, object value)
            : base(direction, parameterName, value)
        {

            MysqlDbType = parameterType;

            switch (MysqlDbType)
            {
                case MySqlDbType.VarChar:
                    if (value == null)
                    {
                        Size = 0;
                    }
                    else
                    {
                        Size = value.ToString().Length;
                    }
                    break;
            }
        }

        public MysqlParameter(System.Data.ParameterDirection direction, string parameterName, MySqlDbType parameterType, object value, int size)
            : base(direction, parameterName, value)
        {
            Size = size;
            MysqlDbType = parameterType;
        }

        public MySqlDbType MysqlDbType { get; set; }

        public override object ConvertToSpecificDBType() =>
            new MySqlParameter
            {
                Direction = Direction,
                IsNullable = IsNullable,
                ParameterName = ParameterName,
                Precision = Precision,
                Scale = Scale,
                SourceColumn = SourceColumn,
                SourceVersion = SourceVersion,
                MySqlDbType = MapToMysqlParameterType(ParameterType),
                Value = Value,

                Size = Size
            };

        private static MySqlDbType MapToMysqlParameterType(Type parameterType)
        {
            MySqlDbType dbType = MySqlDbType.Int16;

            if (IsTypeNullable(parameterType)) {
                return MapToMysqlParameterType(Nullable.GetUnderlyingType(parameterType));
            } else {
#if (NETCOREAPP || NETSTANDARD2_1)
                dbType = parameterType.Name.ToLower() switch
                {
                    "string" => MySqlDbType.VarChar,
                    "int32" => MySqlDbType.Int32,
                    "int64" => MySqlDbType.Int64,
                    "datetime" => MySqlDbType.DateTime,
                    "date" => MySqlDbType.Date,
                    _ => MySqlDbType.Int16
                };
#elif NET461
                switch (parameterType.Name.ToLower())
                {
                    case "string":
                        dbType = MySqlDbType.VarChar;
                        break;

                    case "int32":
                        dbType = MySqlDbType.Int32;
                        break;

                    case "int64":
                        dbType = MySqlDbType.Int64;
                        break;

                    case "datetime":
                        dbType = MySqlDbType.DateTime;
                        break;

                    case "date":
                        dbType = MySqlDbType.Date;
                        break;

                }
#endif
            }

            return dbType;
        }

        private static bool IsTypeNullable(Type type) => Nullable.GetUnderlyingType(type) != null;

    }
}
