using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool
{
    public class ColorToolElement : ToolElement
    {
        public ToolColorEditFlags Flags { get; }

        public ColorToolElement(string name, object source, string propertyName, ToolColorEditFlags flags = ToolColorEditFlags.AlphaBar) : base(name, source, propertyName)
        {
            Flags = flags;

            if (!typeof(Color).IsAssignableFrom(PropertyInfo?.PropertyType))
            {
                throw new ArgumentException("参照先がColor型ではありません");
            }
        }

        public override void Update()
        {
            base.Update();

            if (Source == null || PropertyInfo == null) return;

            Color color = (Color)PropertyInfo.GetValue(Source);
            if (Engine.Tool.ColorEdit4(Name, ref color, Flags))
            {
                PropertyInfo.SetValue(Source, color);
            }
        }

        public static ColorToolElement Create(object source, ToolElementManager.ObjectMapping objectMapping)
        {
            var flags = objectMapping.Options.ContainsKey("flags") ? (ToolColorEditFlags)objectMapping.Options["flags"] : ToolColorEditFlags.AlphaBar;
            return new ColorToolElement(objectMapping.Name, source, objectMapping.PropertyName, flags);
        }
    }
}
