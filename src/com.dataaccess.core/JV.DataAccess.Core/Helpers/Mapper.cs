using JV.DataAccess.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JV.DataAccess.Core.Helpers
{
    internal class Mapper<T> where T : class, new()
    {

        private IFieldPropertyMapper CustomFieldPropertyMapper { get; set; }
        private IFieldPropertyMapper DefaultFieldPropertyMapper { get; set; }

        internal EntityMapping Map(DataAccessBase dbAccess)
        {
            var mapping = new EntityMapping()
            {
                ElementMap = new List<FieldPropertyMapping>()
            };
            Type objectType = new T().GetType();
            PropertyInfo[] typeProps = objectType.GetProperties();
            object[] propAttributes;

            extractQueryBuilderBehaviors(mapping, dbAccess);

            var classAttributes = objectType.GetCustomAttributes(typeof(EntityAttribute));
            EntityAttribute entityAttribute = GetEntityAttribute(classAttributes);
            if (null == entityAttribute)
            {
                throw new MappingException("No associated entity attribute defined for type");
            }

            // Extract the entity related information from the attribute
            mapping.EntityType = map2EntityType(entityAttribute);
            mapping.EntityName = getEntityName(entityAttribute);

            mapping.Query = getEntityQuery(entityAttribute); // If Query then get the sql query from the attribute

            SetupPropertyMappers(entityAttribute);

            foreach (PropertyInfo typeProp in typeProps)
            {
                propAttributes = typeProp.GetCustomAttributes(true);

                if (HasIgnoreAttribute(propAttributes)) // If the property has Ignore attribute, then ignore mapping the property
                {
                    continue;
                }
                FieldAttribute fieldAttribute = GetFieldAttribute(propAttributes);

                mapping.ElementMap.Add(GetFieldMapping(entityAttribute, typeProp, fieldAttribute));
            }

            return mapping;
        }

        #region Private Instance Methods

        private FieldPropertyMapping GetFieldMapping(EntityAttribute entityAttribute, PropertyInfo typeProp, FieldAttribute fieldAttribute)
        {
            FieldPropertyMapping map = null;
            if (null != fieldAttribute)
            {
                IFieldPropertyMapper fpMapper = new FieldPropertyMapper();
                return fpMapper.MapField(entityAttribute, typeProp, fieldAttribute);
            }
            if (null != CustomFieldPropertyMapper)
            {
                map = CustomFieldPropertyMapper.MapField(entityAttribute, typeProp, fieldAttribute);
            }
            if (null == map)
            {
                map = DefaultFieldPropertyMapper.MapField(entityAttribute, typeProp, fieldAttribute);
            }

            return map;
        }

        private void SetupPropertyMappers(EntityAttribute entityAttribute)
        {
            if (entityAttribute.Auto) // If property mapping is automatic then set the mappers
            {
                DefaultFieldPropertyMapper = PrepareDefaultFieldPropertyMappers();
                if (!string.IsNullOrEmpty(entityAttribute.CustomMapper))
                {
                    CustomFieldPropertyMapper = null;
                }
            }
        }

        #endregion Private Instance Methods

        #region Private Static Methods

        private static void extractQueryBuilderBehaviors(EntityMapping mapping, DataAccessBase dbAccess)
        {
            //TODO: Type Cast dbAccess to its actual type and extract the custom attributes
            Type objectType = dbAccess.GetType();
            QueryBuilderAttribute qbAttribute = objectType.GetCustomAttributes(typeof(QueryBuilderAttribute)).FirstOrDefault() as QueryBuilderAttribute;
            if (null != qbAttribute)
            {
                mapping.ElementStartEnclosure = qbAttribute.ElementStartEnclosure;
                mapping.ElementEndEnclosure = qbAttribute.ElementEndEnclosure;
            }
        }

        private static string getEntityQuery(EntityAttribute entityAttribute)
        {
            if (entityAttribute is QueryAttribute)
            {
                return ((QueryAttribute)entityAttribute).Query;
            }
            return string.Empty;
        }

        private static string getEntityName(EntityAttribute entityAttribute)
        {
            string entityName = string.Empty;

            if (entityAttribute is TableAttribute)
            {
                return ((TableAttribute)entityAttribute).TableName;
            }
            if (entityAttribute is ViewAttribute)
            {
                return ((ViewAttribute)entityAttribute).ViewName;
            }
            return string.Empty;
        }

        private static EntityMapping.MappingType map2EntityType(EntityAttribute entityAttribute)
        {
            EntityMapping.MappingType entityType = EntityMapping.MappingType.None;

            if (entityAttribute is TableAttribute)
            {
                entityType = EntityMapping.MappingType.Table;
            }
            if (entityAttribute is ViewAttribute)
            {
                entityType = EntityMapping.MappingType.View;
            }
            if (entityAttribute is QueryAttribute)
            {
                entityType = EntityMapping.MappingType.Query;
            }

            return entityType;
        }

        private static IFieldPropertyMapper PrepareDefaultFieldPropertyMappers()
        {
            return null;
        }

        private static bool HasIgnoreAttribute(object[] propAttributes) => propAttributes.Count(pa => pa is IgnoreAttribute) > 0;

        private static FieldAttribute GetFieldAttribute(object[] propAttributes) => propAttributes.FirstOrDefault(pa => pa is FieldAttribute) as FieldAttribute;

        private static EntityAttribute GetEntityAttribute(IEnumerable<Attribute> propAttributes) => propAttributes.FirstOrDefault(pa => pa is EntityAttribute) as EntityAttribute;

        #endregion Private Static Methods

    }
}
