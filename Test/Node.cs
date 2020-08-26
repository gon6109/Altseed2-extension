using System;
using System.Threading;
using NUnit.Framework;
using Altseed2Extension.Node;
using Altseed2;
using Altseed2Extension.Input;
using System.Linq;

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

            var rect = new RectangleNode();
            rect.RectangleSize = new Vector2F(600, 600);
            rect.Color = new Color(255, 0, 0);
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
            animation
                .MoveTo(new Vector2F(200, 200), 0.8f, EasingType.InBack)
                .RotateTo(360, 0.25f, EasingType.InOutSine)
                .ScaleTo(new Vector2F(0.4f, 0.4f), 1.4f, EasingType.InCubic);
            animationNode.AddAnimation(sprite, animation);

            int count = 0;
            while (Engine.DoEvents())
            {
                if (count++ > 200) break;
                Engine.Update();
            }

            Engine.Terminate();
        }

        class RectangleUINode : UINode
        {
            RectangleNode Rectangle { get; }

            public RectangleUINode()
            {
                Rectangle = new RectangleNode();
                Rectangle.RectangleSize = new Vector2F(100, 50);
                Rectangle.Color = new Color(200, 200, 200);

                OnChangedFocus += OnChangedFocusRectangleUINode;
                Size = new Vector2F(100, 50);

                AddChildNode(Rectangle);
            }

            private void OnChangedFocusRectangleUINode(bool focus)
            {
                if (focus)
                    Rectangle.Color = new Color(200, 0, 0);
                else
                    Rectangle.Color = new Color(200, 200, 200);
            }

            public new Vector2F Position { get => base.Position; set => Rectangle.Position = value; }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void UINode()
        {
            Engine.Initialize("UINode", 800, 600, new Configuration());
            Input.InitInput();

            var parent = new UINode();

            for (int i = 0; i < 3; i++)
            {
                var child = new RectangleUINode();
                child.Position = new Vector2F(150 + 150 * i, 300);
                parent.AddChildNode(child);
            }

            Engine.AddNode(parent);

            int count = 0;
            while (Engine.DoEvents())
            {
                if (count++ > 200) break;
                Engine.Update();
                Input.UpdateInput();
            }

            Engine.Terminate();
        }
    }
}
