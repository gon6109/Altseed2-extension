﻿using Altseed2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altseed2Extension.Tool
{
    public class TextureBaseToolElement : ToolElement
    {

        public TextureBaseToolElement(string name, object source, string propertyName) : base(name, source, propertyName)
        {
            if (!typeof(TextureBase).IsAssignableFrom(PropertyInfo?.PropertyType))
            {
                throw new ArgumentException("参照先がTextureBase型ではありません");
            }
        }

        public override void Update()
        {
            base.Update();

            if (Source == null || PropertyInfo == null) return;

            TextureBase texture = (TextureBase)PropertyInfo.GetValue(Source);
            if (texture != null)
            {
                if (Engine.Tool.ImageButton(texture,
                    new Vector2I(80, 80),
                    new Vector2F(0, 0),
                    new Vector2F(1, 1),
                    5,
                    new Color(),
                    new Color(255, 255, 255, 255)))
                {
                    Editor.Editor.TextureBrowserTarget = this;
                    Engine.Tool.SetWindowFocusByName("Texture Browser");
                }
            }
            else
            {
                if (Engine.Tool.Button("null"))
                {
                    Editor.Editor.TextureBrowserTarget = this;
                    Engine.Tool.SetWindowFocusByName("Texture Browser");
                }
            }
            Engine.Tool.SameLine();
            Engine.Tool.LabelText("Texture", Name);
        }

        public static TextureBaseToolElement Create(object source, ToolElementManager.ObjectMapping objectMapping)
        {
            return new TextureBaseToolElement(objectMapping.Name, source, objectMapping.PropertyName);
        }
    }
}
