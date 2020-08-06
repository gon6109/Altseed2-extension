using System;
using System.Collections.Generic;
using System.Text;
using Altseed2;

namespace Altseed2Extension.Node
{
    /// <summary>
    /// スクリーンサイズに自動調整する<see cref="CameraNode" />
    /// </summary>
    public class ScalingCameraNode : CameraNode
    {
        private bool isFixAspectRatio;
        private Vector2F originDisplaySize = new Vector2F(1920, 1080);

        /// <summary>
        /// カメラが撮影する範囲
        /// </summary>
        public Vector2F OriginDisplaySize
        {
            get => originDisplaySize;
            set
            {
                originDisplaySize = value;
                UpdateScaling();
            }
        }

        /// <summary>
        /// 縦横比を固定するか
        /// </summary>
        public bool IsFixAspectRatio
        {
            get => isFixAspectRatio;
            set
            {
                isFixAspectRatio = value;
                UpdateScaling();
            }
        }

        /// <summary>
        /// Updating時自動的にスケーリングするか
        /// </summary>
        public bool IsUpdateScalingAuto { get; set; }

        protected override void OnAdded()
        {
            UpdateScaling();
            base.OnAdded();
        }

        protected override void OnUpdate()
        {
            if (IsUpdateScalingAuto)
                UpdateScaling();
            base.OnUpdate();
        }

        /// <summary>
        /// スケーリングを更新
        /// </summary>
        public void UpdateScaling()
        {
            var windowSize = (TargetTexture?.Size ?? Engine.WindowSize).To2F();
            if (IsFixAspectRatio)
            {
                if (windowSize.X / windowSize.Y > OriginDisplaySize.X / OriginDisplaySize.Y)
                {
                    Scale = new Vector2F(OriginDisplaySize.Y * OriginDisplaySize.Y / (windowSize.Y * OriginDisplaySize.X), OriginDisplaySize.Y / windowSize.Y);
                    Position = -new Vector2F((windowSize.X - OriginDisplaySize.X * windowSize.Y * OriginDisplaySize.X / (OriginDisplaySize.Y * OriginDisplaySize.Y)) * 0.5f, 0) * Scale;
                }
                else
                {
                    Scale = new Vector2F(OriginDisplaySize.X / windowSize.X, OriginDisplaySize.X * OriginDisplaySize.X / (windowSize.X * OriginDisplaySize.Y));
                    Position = -new Vector2F(0, (windowSize.Y - OriginDisplaySize.Y * windowSize.X * OriginDisplaySize.Y / (OriginDisplaySize.X * OriginDisplaySize.X)) * 0.5f) * Scale;
                }
            }
            else
            {
                Scale = new Vector2F(OriginDisplaySize.X / windowSize.X, OriginDisplaySize.Y / windowSize.Y);
            }
        }
    }
}
