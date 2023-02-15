using System;

namespace JV.DataAccess.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IgnoreAttribute: Attribute
    {
    }
}
