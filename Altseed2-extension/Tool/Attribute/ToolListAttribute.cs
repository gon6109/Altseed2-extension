using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool.Attribute
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class ToolListAttribute : System.Attribute
    {
        public string Name { get; }
        public string ListElementPropertyName { get; }
        public string SelectedItemPropertyName { get; }
        public string AddMethodName { get; }
        public string RemoveMethodName { get; }

        public ToolListAttribute(string name = null, string listElementPropertyName = null, string selectedItemPropertyName = null, string addMethodName = null, string removeMethodName = null)
        {
            Name = name;
            ListElementPropertyName = listElementPropertyName;
            SelectedItemPropertyName = selectedItemPropertyName;
            AddMethodName = addMethodName;
            RemoveMethodName = removeMethodName;
        }
    }
}
