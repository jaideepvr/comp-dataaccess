using Gvs.DataAccess.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Gvs.DataAccess.SqLite
{
    public class SqLiteParameter : Parameter
    {

        public SqLiteParameter():base() { }

        public SqLiteParameter(QueryParameter queryParameter) : base(queryParameter) { }

        public SqLiteParameter(System.Data.ParameterDirection direction, string parameterName, DbType parameterType)
            : this(direction, parameterName, parameterType, null)
        {
        }

        public SqLiteParameter(System.Data.ParameterDirection direction, string parameterName, DbType parameterType, object value)
            : base(direction, parameterName, value)
        {

            SqLiteDbType = parameterType;

            switch (SqLiteDbType)
            {
                case DbType.String:
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

        public SqLiteParameter(System.Data.ParameterDirection direction, string parameterName, DbType parameterType, object value, int size)
            : base(direction, parameterName, value)
        {
            Size = size;
            SqLiteDbType = parameterType;
        }

        public DbType SqLiteDbType { get; set; }

        public override object ConvertToSpecificDBType() =>
            new System.Data.SQLite.SQLiteParameter
            {
                Direction = Direction,
                IsNullable = IsNullable,
                ParameterName = ParameterName,
                Precision = Precision,
                Scale = Scale,
                SourceColumn = SourceColumn,
                SourceVersion = SourceVersion,
                DbType = MapToSqLiteParameterType(ParameterType),
                Value = Value,

                Size = Size
            };

        private static DbType MapToSqLiteParameterType(Type parameterType)
        {
            DbType dbType = DbType.Int16;

            if (IsTypeNullable(parameterType))
            {
                return MapToSqLiteParameterType(Nullable.GetUnderlyingType(parameterType));
            }
            else
            {
#if (NETCOREAPP || NETSTANDARD2_1)
                dbType = parameterType.Name.ToLower() switch
                {
                    "string" => DbType.String,
                    "int32" => DbType.Int32,
                    "int64" => DbType.Int64,
                    "datetime" => DbType.DateTime,
                    "date" => DbType.String,
                    _ => DbType.Int16
                };
#elif NET461
                switch (parameterType.Name.ToLower())
                {
                    case "string":
                        dbType = DbType.String;
                        break;

                    case "int32":
                        dbType = DbType.Int32;
                        break;

                    case "int64":
                        dbType = DbType.Int64;
                        break;

                    case "datetime":
                        dbType = DbType.String;
                        break;

                    case "date":
                        dbType = DbType.String;
                        break;

                }
#endif
            }

            return dbType;
        }

        private static bool IsTypeNullable(Type type) => Nullable.GetUnderlyingType(type) != null;

    }
}
