using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Node
{
    /// <summary>
    /// スプライトをアニメーションするノード
    /// </summary>
    public class AnimationSpriteNode : SpriteNode
    {
        private Vector2F size;
        private int frame = 1;
        private int column = 4;
        private IEnumerator<object> coroutine;

        /// <summary>
        /// アニメーションするテクスチャの1コマでのサイズ
        /// </summary>
        public Vector2F Size
        {
            get => size;
            set
            {
                size = value;

                Src = new RectF(new Vector2F(), size);
            }
        }

        /// <summary>
        /// コマ数
        /// </summary>
        public int Frame
        {
            get => frame;
            set
            {
                if (value <= 0)
                    return;

                frame = value;
            }
        }

        /// <summary>
        /// 列数
        /// </summary>
        public int Column
        {
            get => column;
            set
            {
                if (value <= 0)
                    return;

                column = value;
            }
        }

        /// <summary>
        /// 再生時間
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// ループするか
        /// </summary>
        public bool IsLoop { get; set; }

        /// <summary>
        /// 再生中か
        /// </summary>
        public bool IsPlaying { get; private set; }

        /// <summary>
        /// アニメーションが終了したとき、自動でノードを削除するか
        /// </summary>
        public bool IsRemoveNodeOnEndAnimation { get; set; }

        protected override void OnAdded()
        {
            base.OnAdded();
            Play();
        }

        protected override void OnUpdate()
        {
            IsPlaying = coroutine?.MoveNext() ?? false;
            base.OnUpdate();
        }

        /// <summary>
        /// 再生する
        /// </summary>
        public void Play()
        {
            coroutine = Animate();
        }

        private IEnumerator<object> Animate()
        {
            do
            {
                Src = new RectF(new Vector2F(), Size);

                var time = 0f;
                while (time < Duration)
                {
                    int current = (int)(time / Duration * Frame);
                    Src = new RectF(new Vector2F(current % Column, current / Column) * Size, Size);
                    yield return null;
                    time += Engine.DeltaSecond;
                }
            } while (IsLoop);

            if (IsRemoveNodeOnEndAnimation)
                Parent?.RemoveChildNode(this);
        }
    }
}
