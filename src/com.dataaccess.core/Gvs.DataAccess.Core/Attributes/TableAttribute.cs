using System;

namespace Gvs.DataAccess.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TableAttribute : EntityAttribute
    {

        public string TableName { get; set; }

        public string PrimaryKey { get; set; }

        public TableAttribute(string tableName, string primaryKey)
        {
            TableName = tableName;
            PrimaryKey = primaryKey;
        }

        public TableAttribute(string tableName, string primaryKey, string customMapper, bool auto = true)
            : base(customMapper)
        {
            TableName = tableName;
            PrimaryKey = primaryKey;
        }

        public TableAttribute(string tableName, string primaryKey, bool auto)
            : base(auto)
        {
            TableName = tableName;
            PrimaryKey = primaryKey;
        }

    }
}
