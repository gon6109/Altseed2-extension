using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool.Attribute
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class ToolTextureBaseAttribute : System.Attribute
    {
        public string Name { get; }

        public ToolTextureBaseAttribute(string name = null)
        {
            Name = name;
        }
    }
}
