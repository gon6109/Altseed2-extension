using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool
{
    public class EnumToolElement : ToolElement
    {
        public EnumToolElement(string name, object source, string propertyName) : base(name, source, propertyName)
        {
            if (!(PropertyInfo?.PropertyType.IsEnum ?? true))
            {
                throw new ArgumentException("参照先がEnumではありません");
            }
        }

        public override void Update()
        {
            base.Update();

            if (Source == null || PropertyInfo == null) return;

            int current = (int)PropertyInfo.GetValue(Source);
            if (Engine.Tool.Combo(Name, ref current, Enum.GetNames(PropertyInfo.PropertyType), -1))
            {
                PropertyInfo.SetValue(Source, current);
            }
        }

        public static EnumToolElement Create(object source, ToolElementManager.ObjectMapping objectMapping)
        {
            return new EnumToolElement(objectMapping.Name, source, objectMapping.PropertyName);
        }
    }
}
