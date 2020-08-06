using System;
using System.Threading;
using NUnit.Framework;
using Altseed2Extension;
using Altseed2;

namespace Test
{
    [TestFixture]
    public class Node
    {
        [Test, Apartment(ApartmentState.STA)]
        public void ScalingCameraNode()
        {
            Engine.Initialize("ScalingCameraNode", 800, 600, new Configuration() { IsResizable = true });
            //Engine.ClearColor = new Color(255, 255, 255);

            var renderTexture = RenderTexture.Create(new Vector2I(500, 400), TextureFormat.R8G8B8A8_UNORM);
            var camera = new CameraNode();
            camera.Group = 0;
            Engine.AddNode(camera);

            var sprite = new SpriteNode();
            sprite.Position = new Vector2F(50, 50);
            sprite.CameraGroup = 1 << 0;
            sprite.Texture = renderTexture;
            Engine.AddNode(sprite);

            var scalingCamera = new Altseed2Extension.Node.ScalingCameraNode();
            scalingCamera.IsUpdateScalingAuto = true;
            scalingCamera.IsFixAspectRatio = true;
            scalingCamera.OriginDisplaySize = new Vector2F(600, 600);
            scalingCamera.ClearColor = new Color(255, 255, 255);
            scalingCamera.IsColorCleared = true;
            scalingCamera.TargetTexture = renderTexture;
            scalingCamera.Group = 1;
            Engine.AddNode(scalingCamera);

            var rect = new PolygonNode();
            rect.SetVertexes(new[] {
                new Vector2F(0 , 0),
                new Vector2F(600, 0),
                new Vector2F(600, 600),
                new Vector2F(0, 600),
            }, new Color(255, 0, 0));
            rect.CameraGroup = 1 << 1;
            Engine.AddNode(rect);

            int count = 0;
            while (Engine.DoEvents())
            {
                if (count++ > 200) break;
                Engine.Update();
            }
            renderTexture.Save("ScalingCameraNode.png");

            Engine.Terminate();
        }
    }
}
