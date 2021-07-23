using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsits.DataAccess.Core
{
    public class EntityAttribute: Attribute
    {

        public bool Auto { get; set; } = false;
        
        public string CustomMapper { get; set; }

        public EntityAttribute()
        {
            Auto = false;
            CustomMapper = string.Empty;
        }

        public EntityAttribute(string customMapper)
        {
            CustomMapper = customMapper;
            Auto = true;
        }

        public EntityAttribute(bool auto)
        {
            Auto = auto;
        }

    }
}
