using Altseed2;
using Altseed2Extension.Editor;
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
        [Test, Apartment(ApartmentState.STA)]
        public void Basic()
        {
            Editor.Initialize("Texture2DExtension", 1500, 800, new Configuration() { ConsoleLoggingEnabled = true, IsResizable = true });

            var sprite = new SpriteNode();
            sprite.Position = new Vector2F(400, 400);
            sprite.Texture = Texture2D.Load("../TestData/IO/AltseedPink.png");
            sprite.CenterPosition = (sprite.Texture?.Size.To2F() ?? default) / 2.0f;
            Engine.AddNode(sprite);

            Editor.Selected = sprite;

            while (Engine.DoEvents())
            {
                Editor.Update();
            }

            Editor.Terminate();
        }
    }
}
