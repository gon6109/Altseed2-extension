using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool
{
    public class InputTextToolElement : ToolElement
    {
        bool IsMultiLine { get; }

        public InputTextToolElement(string name, object source, string propertyName, bool isMultiLine = false) : base(name, source, propertyName)
        {
            IsMultiLine = isMultiLine;

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
            if (text == null)
                text = "";
            string newStr;
            if (IsMultiLine)
            {
                if ((newStr = Engine.Tool.InputTextMultiline(Name, text, text.Length + 2, new Vector2F(-1, -1), ToolInputTextFlags.None)) != null)
                {
                    PropertyInfo.SetValue(Source, newStr);
                }
            }
            else if ((newStr = Engine.Tool.InputText(Name, text, text.Length + 2, ToolInputTextFlags.None)) != null)
            {
                PropertyInfo.SetValue(Source, newStr);
            }
        }

        public static InputTextToolElement Create(object source, ToolElementManager.ObjectMapping objectMapping)
        {
            var isMultiLine = objectMapping.Options.ContainsKey("isMultiLine") ? (bool)objectMapping.Options["isMultiLine"] : false;
            return new InputTextToolElement(objectMapping.Name, source, objectMapping.PropertyName, isMultiLine);
        }
    }
}
