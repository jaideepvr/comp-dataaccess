using System;

namespace Jsits.DataAccess.Core.Helpers
{
    public class FieldPropertyMapping
    {
        public string FieldName = string.Empty;
        public string PropertyName = string.Empty;
        public Type PropertyType;
        public string TypeName => GetTypeName(PropertyType);
        public bool IsNative = true;
        public bool IsPrimaryKey = false;
        public bool IsNullable = false;

        public FieldPropertyMapping(string fieldName, string propertyName, Type propertyType, bool isNative, bool isNullable = false, bool isPrimaryKey = false)
        {
            FieldName = fieldName;
            PropertyName = propertyName;
            PropertyType = propertyType;
            IsNative = isNative;
            IsPrimaryKey = isPrimaryKey;
            IsNullable = isNullable;
        }

        private string GetTypeName(Type propertyType)
        {
            Type propType = null;
            if (IsNullableType(propertyType))
            {
                propType = Nullable.GetUnderlyingType(propertyType);
            }
            else
            {
                propType = propertyType;
            }

            return propType.Name;
        }

        private bool IsNullableType(Type propType) => (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>));

    }
}
