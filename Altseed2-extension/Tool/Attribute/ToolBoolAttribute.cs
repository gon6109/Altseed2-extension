using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool.Attribute
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class ToolBoolAttribute : System.Attribute
    {
        public string Name { get; }

        public ToolBoolAttribute(string name = null)
        {
            Name = name;
        }
    }
}
