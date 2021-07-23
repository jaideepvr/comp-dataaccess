using Jsits.DataAccess.Core.Interfaces;
using System;
using System.Reflection;

namespace Jsits.DataAccess.Core.Helpers
{
    public class FieldPropertyMapper : BaseFieldPropertyMapper, IFieldPropertyMapper
    {

        public override FieldPropertyMapping MapField(EntityAttribute entityAttribute, PropertyInfo property, FieldAttribute propertyAttribute)
        {
            bool isPrimaryKey = false;
            if (entityAttribute is TableAttribute) // If Table then get the primary key
            {
                isPrimaryKey = propertyAttribute.FieldName == ((TableAttribute)entityAttribute).PrimaryKey;
            }

            return new FieldPropertyMapping(propertyAttribute.FieldName, property.Name, property.PropertyType, 
                                            propertyAttribute.IsNativeColumn, IsNullableType(property.PropertyType), isPrimaryKey);
        }

    }
}
