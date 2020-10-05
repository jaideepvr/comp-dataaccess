using System;

namespace Gvs.DataAccess.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ViewAttribute : EntityAttribute
    {

        public string ViewName { get; set; }

        public ViewAttribute(string viewName, string primaryKey)
        {
            ViewName = viewName;
        }

        public ViewAttribute(string viewName, string primaryKey, string customMapper, bool auto = true)
            : base(customMapper)
        {
            ViewName = viewName;
        }

        public ViewAttribute(string viewName, string primaryKey, bool auto)
            : base(auto)
        {
            ViewName = viewName;
        }

    }
}
