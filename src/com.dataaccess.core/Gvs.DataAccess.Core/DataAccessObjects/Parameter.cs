using System;
using System.Data;
using System.Globalization;

namespace Gvs.DataAccess.Core
{
    /// <summary>
    /// Implements IDbDataParameter with stored proc related properties
    /// </summary>
    public abstract class Parameter : IDbDataParameter
    {

        protected QueryParameter SourceParameter { get; set; }

        public Parameter(QueryParameter queryParameter)
        {
            SourceParameter = queryParameter;
            ParameterName = queryParameter.ParameterName;
            Direction = queryParameter.Direction;
            Value = queryParameter.Value;
            DefaultValue = queryParameter.DefaultValue;
            IsNullable = queryParameter.IsNullable;
            ParameterType = queryParameter.ParameterType;
        }

        public Parameter()
        {
        }

        public Parameter(ParameterDirection direction, string parameterName)
            : this(direction, parameterName, null)
        {
        }

        public Parameter(ParameterDirection paramDirection, string paramName, object paramValue)
        {
            Direction = paramDirection;
            ParameterName = paramName;
            Value = paramValue;
        }

        public Parameter(ParameterDirection paramDirection, string paramName, object paramValue, object defValue)
        {
            Direction = paramDirection;
            ParameterName = paramName;
            Value = paramValue;
            DefaultValue = defValue;
        }

        public abstract object ConvertToSpecificDBType();

        #region IDataParameter Members

        public DbType DbType { get; set; }

        public ParameterDirection Direction { get; set; }

        public bool IsNullable { get; set; } = false;

        public string SourceColumn { get; set; }

        public DataRowVersion SourceVersion { get; set; } = DataRowVersion.Original;

        public string ParameterName { get; set; }

        public object Value { get; set; }

        public object DefaultValue { get; set; }

        #endregion

        #region IDbDataParameter Members

        public byte Precision { get; set; }

        public byte Scale { get; set; }

        public int Size { get; set; }

        #endregion

        public Type ParameterType { get; set; }

        public string ValueAsString => Convert.ToString(Value, CultureInfo.InvariantCulture);

        public int ValueAsInt => Convert.ToInt32(Value.ToString(), CultureInfo.InvariantCulture);

        public bool ValueAsBoolean => Convert.ToBoolean(Convert.ToInt32(Value.ToString(), CultureInfo.InvariantCulture));

        public Double ValueAsDouble => Convert.ToDouble(Value.ToString(), CultureInfo.InvariantCulture);

        public Int64 ValueAsLong => Convert.ToInt64(Value.ToString(), CultureInfo.InvariantCulture);

    }
}
