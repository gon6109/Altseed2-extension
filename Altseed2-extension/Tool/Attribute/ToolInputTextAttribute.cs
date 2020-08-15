using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool.Attribute
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class ToolInputTextAttribute : System.Attribute
    {
        public int MaxLength { get; }
        public bool IsMultiLine { get; }
        public string Name { get; }

        public ToolInputTextAttribute(string name = null, bool isMultiLine = false, int maxLength = 1024)
        {
            Name = name;
            IsMultiLine = isMultiLine;
            MaxLength = maxLength;
        }
    }
}
