using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool.Attribute
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class ToolIntAttribute : System.Attribute
    {
        public string Name { get; }
        public float Speed { get; }
        public int Min { get; }
        public int Max { get; }

        public ToolIntAttribute(string name = null, float speed = 1, int min = -100, int max = 100)
        {
            Name = name;
            Speed = speed;
            Min = min;
            Max = max;
        }
    }
}
