using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool.Attribute
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ToolAutoAttribute : System.Attribute
    {
        public ToolAutoAttribute()
        {
        }
    }
}
