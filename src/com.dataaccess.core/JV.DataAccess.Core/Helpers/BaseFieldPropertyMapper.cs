using JV.DataAccess.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JV.DataAccess.Core.Helpers
{
    public abstract class BaseFieldPropertyMapper : IFieldPropertyMapper
    {

        public IFieldPropertyMapper InnerMapper { get; set; }

        public abstract FieldPropertyMapping MapField(EntityAttribute entityAttribute, PropertyInfo property, FieldAttribute propertyAttribute);

        protected string GetTypeName(Type propertyType)
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

        protected bool IsNullableType(Type propType) => (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>));

    }
}
