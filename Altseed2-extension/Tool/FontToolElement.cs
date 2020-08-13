using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Tool
{
    public class FontToolElement : ToolElement
    {
        public FontToolElement(string name, object source, string propertyName) : base(name, source, propertyName)
        {
            if (!typeof(Font).IsAssignableFrom(PropertyInfo?.PropertyType))
            {
                throw new ArgumentException("参照先がFont型ではありません");
            }
        }

        public override void Update()
        {
            base.Update();

            if (Source == null || PropertyInfo == null) return;

            Font font = (Font)PropertyInfo.GetValue(Source);
            if (font != null)
            {
                var glyph = font.GetGlyph((int)'阿');
                if (Engine.Tool.ImageButton(font.GetFontTexture(0),
                    new Vector2I(80, 80),
                    new Vector2F(0, 0),
                    (glyph.Position + glyph.Size).To2F() / glyph.TextureSize,
                    5,
                    new Color(),
                    new Color(255, 255, 255, 255)))
                {
                    Editor.Editor.FontBrowserTarget = this;
                    Engine.Tool.SetWindowFocusByName("Font Browser");
                }
            }
            else
            {
                if (Engine.Tool.Button("null"))
                {
                    Editor.Editor.FontBrowserTarget = this;
                    Engine.Tool.SetWindowFocusByName("Font Browser");
                }
            }
            Engine.Tool.SameLine();
            Engine.Tool.LabelText("Font", Name);
        }

        public static FontToolElement Create(object source, ToolElementManager.ObjectMapping objectMapping)
        {
            return new FontToolElement(objectMapping.Name, source, objectMapping.PropertyName);
        }
    }
}
