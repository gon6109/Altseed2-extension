using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool.Attribute
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class ToolPathAttribute : System.Attribute
    {
        public string Name { get; }
        public int MaxLength { get; }
        public string Filter { get; }
        public string DefaultPath { get; }
        public bool IsDirectory { get; }

        public ToolPathAttribute(string name = null, bool isDirectory = false, string filter = "", string defaultPath = "", int maxLength = 1024)
        {
            Name = name;
            MaxLength = maxLength;
            Filter = filter;
            DefaultPath = defaultPath;
            IsDirectory = isDirectory;
        }
    }
}
