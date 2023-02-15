using JV.DataAccess.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JV.DataAccess.Core.Interfaces
{
    public interface IFieldPropertyMapper
    {

        IFieldPropertyMapper InnerMapper { get; set; }

        FieldPropertyMapping MapField(EntityAttribute entityAttribute, PropertyInfo property, FieldAttribute propertyAttribute);

    }
}
