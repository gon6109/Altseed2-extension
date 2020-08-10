using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Altseed2Extension.Tool
{
    public abstract class ToolElement
    {
        private readonly WeakReference<object> source;

        public ToolElement(string name, object source, string propertyName)
        {
            Name = name;
            this.source = new WeakReference<object>(source);
            PropertyName = propertyName;
        }

        public virtual void Update() { }

        public object Source
        {
            get
            {
                if (source.TryGetTarget(out var res))
                    return res;
                return null;
            }
        }

        public string PropertyName { get; }

        public string Name { get; }

        public PropertyInfo PropertyInfo
        {
            get
            {
                try
                {
                    return Source?.GetType().GetProperty(PropertyName);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
