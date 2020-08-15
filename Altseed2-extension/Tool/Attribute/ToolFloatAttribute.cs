using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool.Attribute
{

    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class ToolFloatAttribute : System.Attribute
    {
        public string Name { get; }
        public float Speed { get; }
        public float Min { get; }
        public float Max { get; }

        public ToolFloatAttribute(string name = null, float speed = 1, float min = -100, float max = 100)
        {
            Name = name;
            Speed = speed;
            Min = min;
            Max = max;
        }
    }
}
