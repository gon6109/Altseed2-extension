using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool
{
    public class Vector2FToolElement : ToolElement
    {
        private float[] vectorArray;

        public float Speed { get; }
        public float Min { get; }
        public float Max { get; }

        public Vector2FToolElement(string name, object source, string propertyName, float speed = 1, float min = -1000, float max = 1000) : base(name, source, propertyName)
        {
            Speed = speed;
            Min = min;
            Max = max;
            vectorArray = new float[2];

            if (!typeof(Vector2F).IsAssignableFrom(PropertyInfo?.PropertyType))
            {
                throw new ArgumentException("参照先がVector2Fではありません");
            }
        }

        public override void Update()
        {
            base.Update();

            if (Source == null || PropertyInfo == null) return;

            Vector2F vector = (Vector2F)PropertyInfo.GetValue(Source);
            vectorArray[0] = vector.X;
            vectorArray[1] = vector.Y;
            if (Engine.Tool.SliderFloat2(Name, vectorArray, Speed, Min, Max))
            {
                vector.X = vectorArray[0];
                vector.Y = vectorArray[1];
                PropertyInfo.SetValue(Source, vector);
            }
        }

        public static Vector2FToolElement Create(object source, ToolElementManager.ObjectMapping objectMapping)
        {
            var speed = objectMapping.Options.ContainsKey("speed") ? (float)objectMapping.Options["speed"] : 1;
            var min = objectMapping.Options.ContainsKey("min") ? (float)objectMapping.Options["min"] : -1000;
            var max = objectMapping.Options.ContainsKey("max") ? (float)objectMapping.Options["max"] : 1000;
            return new Vector2FToolElement(objectMapping.Name, source, objectMapping.PropertyName, speed, min, max);
        }
    }
}
