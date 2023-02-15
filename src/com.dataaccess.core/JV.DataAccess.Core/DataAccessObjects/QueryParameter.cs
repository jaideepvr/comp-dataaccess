using System;
using System.Data;

namespace JV.DataAccess.Core
{
    public class QueryParameter
    {

        public QueryParameter(string parameterName, ParameterDirection parameterDirection)
        {
            ParameterName = parameterName;
            Direction = parameterDirection;
        }

        public QueryParameter(string parameterName, ParameterDirection parameterDirection, object parameterValue)
        {
            ParameterName = parameterName;
            Direction = parameterDirection;
            Value = parameterValue;
        }

        public ParameterDirection Direction { get; set; }

        public bool IsNullable { get; set; }

        public string ParameterName { get; set; }

        public byte Precision { get; set; }

        public byte Scale { get; set; }

        public int Size { get; set; }

        public object Value { get; set; }

        public object DefaultValue { get; set; }

        public Type ParameterType { get; set; }

    }
}
