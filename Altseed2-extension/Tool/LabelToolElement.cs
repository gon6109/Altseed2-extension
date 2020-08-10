using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool
{
    public class LabelToolElement : ToolElement
    {
        public LabelToolElement(string name, object source, string propertyName) : base(name, source, propertyName)
        {
            if (!typeof(string).IsAssignableFrom(PropertyInfo?.PropertyType))
            {
                throw new ArgumentException("参照先がstring型ではありません");
            }
        }

        public override void Update()
        {
            base.Update();

            if (Source == null || PropertyInfo == null) return;

            string text = (string)PropertyInfo.GetValue(Source);
            Engine.Tool.LabelText(Name, text);
        }

        public static LabelToolElement Create(object source, ToolElementManager.ObjectMapping objectMapping)
        {
            return new LabelToolElement(objectMapping.Name, source, objectMapping.PropertyName);
        }
    }
}
