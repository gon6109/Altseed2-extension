﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool.Attribute
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class ToolUserAttribute : System.Attribute
    {
        public string Name { get; }

        public ToolUserAttribute(string name = null)
        {
            Name = name;
        }
    }
}