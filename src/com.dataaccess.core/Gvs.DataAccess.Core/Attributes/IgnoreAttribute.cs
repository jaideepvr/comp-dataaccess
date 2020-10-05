using System;

namespace Gvs.DataAccess.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IgnoreAttribute: Attribute
    {
    }
}
