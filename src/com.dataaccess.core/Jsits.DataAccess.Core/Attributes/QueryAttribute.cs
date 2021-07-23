using System;

namespace Jsits.DataAccess.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class QueryAttribute : EntityAttribute
    {

        public string Query { get; set; }

        public QueryAttribute(string query)
        {
            Query = query;
        }

        public QueryAttribute(string query, string customMapper, bool auto = true)
            : base(customMapper)
        {
            Query = query;
        }

        public QueryAttribute(string query, bool auto)
            : base(auto)
        {
            Query = query;
        }

    }
}
