using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool
{
    public class IntToolElement : ToolElement
    {
        public float Speed { get; }
        public int Min { get; }
        public int Max { get; }

        public IntToolElement(string name, object source, string propertyName, float speed = 1, int min = -100, int max = 100) : base(name, source, propertyName)
        {
            Speed = speed;
            Min = min;
            Max = max;

            if (!typeof(int).IsAssignableFrom(PropertyInfo?.PropertyType))
            {
                throw new ArgumentException("参照先がint型ではありません");
            }
        }

        public override void Update()
        {
            base.Update();

            if (Source == null || PropertyInfo == null) return;

            int num = (int)PropertyInfo.GetValue(Source);
            if (Engine.Tool.DragInt(Name, ref num, Speed, Min, Max))
            {
                PropertyInfo.SetValue(Source, num);
            }
        }

        public static IntToolElement Create(object source, ToolElementManager.ObjectMapping objectMapping)
        {
            var speed = objectMapping.Options.ContainsKey("speed") ? (float)objectMapping.Options["speed"] : 1;
            var min = objectMapping.Options.ContainsKey("min") ? (int)objectMapping.Options["min"] : -100;
            var max = objectMapping.Options.ContainsKey("max") ? (int)objectMapping.Options["max"] : 100;
            return new IntToolElement(objectMapping.Name, source, objectMapping.PropertyName, speed, min, max);
        }
    }
}
