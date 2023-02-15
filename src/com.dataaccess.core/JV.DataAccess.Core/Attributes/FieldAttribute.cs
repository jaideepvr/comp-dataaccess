using System;

namespace JV.DataAccess.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FieldAttribute : Attribute
    {

        public string FieldName { get; private set; }

        public bool IsNativeColumn { get; private set; }

        public FieldAttribute(string fieldName, bool isNative = true)
        {
            FieldName = fieldName;
            IsNativeColumn = isNative;
        }

    }
}
