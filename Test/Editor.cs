﻿using Altseed2;
using Altseed2Extension.Editor;
using Altseed2Extension.Tool.Attribute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Test
{
    [TestFixture]
    class EditorTest
    {
        [ToolAuto]
        class TestNode : SpriteNode
        {
            public string Label => "Test OutPut";
            public string Text { get; set; }

            [ToolButton]
            public void Test()
            {
                Text = "hoge";
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Basic()
        {
            Editor.Initialize("Texture2DExtension", 1500, 800, new Configuration() { ConsoleLoggingEnabled = true, IsResizable = true });

            for (int i = 0; i < 5; i++)
            {
                var sprite = new TestNode();
                sprite.Position = new Vector2F(50 + 75 * i, 400);
                sprite.Texture = Texture2D.Load("../TestData/IO/AltseedPink.png");
                sprite.CenterPosition = (sprite.Texture?.Size.To2F() ?? default) / 2.0f;
                for (int l = 0; l < 3; l++)
                {
                    var text = new TextNode();
                    text.Position = new Vector2F(0, 50 * l);
                    text.Font = Font.LoadDynamicFont("../TestData/Font/mplus-1m-regular.ttf", 80);
                    text.Color = new Color(255, 255, 255);
                    text.Text = "テキスト";
                    sprite.AddChildNode(text);
                }
                Engine.AddNode(sprite);
                Editor.Selected = sprite;
            }


            while (Engine.DoEvents())
            {
                Editor.Update();
            }

            Editor.Terminate();
        }
    }
}
