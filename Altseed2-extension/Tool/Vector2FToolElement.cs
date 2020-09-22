using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool
{
    public class Vector2FToolElement : ToolElement
    {
        public float Speed { get; }
        public float Min { get; }
        public float Max { get; }

        public Vector2FToolElement(string name, object source, string propertyName, float speed = 1, float min = -1000, float max = 1000) : base(name, source, propertyName)
        {
            Speed = speed;
            Min = min;
            Max = max;

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
            float x = vector.X;
            float y = vector.Y;

            var width = Engine.Tool.GetColumnWidth(0) * 0.65f;
            Engine.Tool.Columns(2, false);
            Engine.Tool.SetColumnWidth(0, width / 2);
            Engine.Tool.SetColumnWidth(1, width / 2);
            Engine.Tool.PushItemWidth(-1);

            if (Engine.Tool.DragFloat($"##{Name}_X", ref x, Speed, Min, Max))
            {
                vector.X = x;
                PropertyInfo.SetValue(Source, vector);
            }

            Engine.Tool.PopItemWidth();
            Engine.Tool.NextColumn();
            Engine.Tool.PushItemWidth(-1);

            if (Engine.Tool.DragFloat($"##{Name}_Y", ref y, Speed, Min, Max))
            {
                vector.Y = y;
                PropertyInfo.SetValue(Source, vector);
            }
            Engine.Tool.PopItemWidth();
            Engine.Tool.Columns(1, false);
            Engine.Tool.SameLine();
            Engine.Tool.Text(Name);
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
