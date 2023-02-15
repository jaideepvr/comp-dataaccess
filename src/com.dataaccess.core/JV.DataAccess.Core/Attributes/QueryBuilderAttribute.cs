using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JV.DataAccess.Core
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class QueryBuilderAttribute : Attribute
    {

        public QueryBuilderAttribute(string elementEnclosure = "")
        {
            if (!string.IsNullOrEmpty(elementEnclosure) && (elementEnclosure.Split(':').Count() == 2))
            {
                ElementEnclosure = elementEnclosure;
            }
            else
            {
                throw new DataAccessException($"Illegal QueryBuilderAttribute expression: {elementEnclosure} is not an acceptable enclosure expression.");
            }
        }

        public string ElementEnclosure { private get; set; } = string.Empty;

        public string ElementStartEnclosure { get => ElementEnclosure.Split(':').Count() == 2 ? ElementEnclosure.Split(':')[0] : string.Empty; }

        public string ElementEndEnclosure { get => ElementEnclosure.Split(':').Count() == 2 ? ElementEnclosure.Split(':')[1] : string.Empty; }

    }
}
