using Altseed2;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Altseed2Extension.Extension;

namespace Test
{
    [TestFixture]
    public class Extension
    {
        [Test, Apartment(ApartmentState.STA)]
        public void Texture2DExtension()
        {
            Engine.Initialize("Texture2DExtension", 800, 600, new Configuration());
            Altseed2Extension.Extension.Texture2DExtension.ErrorImage = "../TestData/IO/AltseedPink.jpeg";

            var sprite = new SpriteNode();
            sprite.Position = new Vector2F(400, 400);
            sprite.Texture = Altseed2Extension.Extension.Texture2DExtension.Load("../TestData/IO/hoge.png");
            sprite.Texture = Altseed2Extension.Extension.Texture2DExtension.Load("../TestData/IO/AltseedPink.png");
            sprite.CenterPosition = (sprite.Texture?.Size.To2F() ?? default) / 2.0f;
            Engine.AddNode(sprite);

            int count = 0;
            while (Engine.DoEvents())
            {
                if (count++ > 200) break;
                Engine.Update();
            }

            Engine.Terminate();
        }
    }
}
