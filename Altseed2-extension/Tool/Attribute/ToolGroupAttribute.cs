using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool.Attribute
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class ToolGroupAttribute : System.Attribute
    {
        public string Name { get; }

        public ToolGroupAttribute(string name = null)
        {
            Name = name;
        }
    }
}
