using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool.Attribute
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class ToolColorAttribute : System.Attribute
    {
        public ToolColorEditFlags Flags { get; }
        public string Name { get; }

        public ToolColorAttribute(string name = null, ToolColorEditFlags flags = ToolColorEditFlags.AlphaBar)
        {
            Name = name;
            Flags = flags;
        }
    }
}
