using System;
using System.Threading;
using NUnit.Framework;
using Altseed2Extension.Node;
using Easing = Altseed2Extension.Node.AnimationNode.Animation.Easing;
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

            var renderTexture = RenderTexture.Create(new Vector2I(500, 400), TextureFormat.R8G8B8A8_UNORM);
            var camera = new CameraNode();
            camera.Group = 0;
            Engine.AddNode(camera);

            var sprite = new SpriteNode();
            sprite.Position = new Vector2F(50, 50);
            sprite.CameraGroup = 1 << 0;
            sprite.Texture = renderTexture;
            Engine.AddNode(sprite);

            var scalingCamera = new ScalingCameraNode();
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

        [Test, Apartment(ApartmentState.STA)]
        public void AnimationNode()
        {
            Engine.Initialize("AnimationNode", 800, 600, new Configuration());

            var sprite = new SpriteNode();
            sprite.Position = new Vector2F(400, 400);
            sprite.Texture = Texture2D.Load("../TestData/IO/AltseedPink.png");
            sprite.CenterPosition = (sprite.Texture?.Size.To2F() ?? default) / 2.0f;
            Engine.AddNode(sprite);

            var animationNode = new AnimationNode();
            sprite.AddChildNode(animationNode);

            var animation = new AnimationNode.Animation();
            animation.MoveTo(new Vector2F(200, 200), 50, Easing.InBack);
            animation.RotateTo(360, 15, Easing.InOutSine);
            animation.ScaleTo(new Vector2F(0.4f, 0.4f), 85, Easing.InCubic);
            animationNode.AddAnimation(sprite, animation);

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
