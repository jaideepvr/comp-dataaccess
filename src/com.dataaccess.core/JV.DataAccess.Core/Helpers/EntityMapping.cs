using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JV.DataAccess.Core.Helpers
{
    public class EntityMapping
    {

        public enum MappingType
        {
            None,
            Table,
            View,
            Query
        }

        public MappingType EntityType { get; set; }

        public string EntityName { get; set; }

        public string EnclosedEntityName => $"{ElementStartEnclosure}{EntityName}{ElementEndEnclosure}";

        public FieldPropertyMapping PrimaryKey => ElementMap.FirstOrDefault(em => em.IsPrimaryKey);

        public string Query { get; set; }

        public List<FieldPropertyMapping> ElementMap { get; set; }

        public string ElementStartEnclosure { get; set; } = string.Empty;

        public string ElementEndEnclosure { get; set; } = string.Empty;

    }
}
