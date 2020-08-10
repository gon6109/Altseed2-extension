using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool
{
    public class BoolToolElement : ToolElement
    {
        public BoolToolElement(string name, object source, string propertyName) : base(name, source, propertyName)
        {
            if (!typeof(bool).IsAssignableFrom(PropertyInfo?.PropertyType))
            {
                throw new ArgumentException("参照先がbool型ではありません");
            }
        }

        public override void Update()
        {
            base.Update();

            if (Source == null || PropertyInfo == null) return;

            bool flag = (bool)PropertyInfo.GetValue(Source);
            if (Engine.Tool.CheckBox(Name, ref flag))
            {
                PropertyInfo.SetValue(Source, flag);
            }
        }

        public static BoolToolElement Create(object source, ToolElementManager.ObjectMapping objectMapping)
        {
            return new BoolToolElement(objectMapping.Name, source, objectMapping.PropertyName);
        }
    }
}
