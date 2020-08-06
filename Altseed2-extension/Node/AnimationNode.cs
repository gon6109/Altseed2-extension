using System;
using System.Collections.Generic;
using System.Text;
using Altseed2;

namespace Altseed2Extension.Node
{
    public partial class AnimationNode : Altseed2.Node
    {
        Dictionary<int, IEnumerator<object>> animations;

        public AnimationNode()
        {
            animations = new Dictionary<int, IEnumerator<object>>();
        }

        /// <summary>
        /// アニメーションしているか
        /// </summary>
        public bool IsAnimating => animations.Count > 0;

        /// <summary>
        /// 更新処理
        /// </summary>
        protected override void OnUpdate()
        {
            List<int> removeAnimationKeys = new List<int>();
            foreach (var item in animations)
            {
                if (!item.Value.MoveNext()) removeAnimationKeys.Add(item.Key);
            }

            foreach (var item in removeAnimationKeys)
            {
                animations.Remove(item);
            }
            base.OnUpdate();
        }

        /// <summary>
        /// アニメーションを追加する
        /// </summary>
        /// <param name="object2D">アニメーションするオブジェクト</param>
        /// <param name="animation">アニメーション</param>
        /// <param name="slot">スロット</param>
        public void AddAnimation(TransformNode transformNode, Animation animation, int slot = 0)
        {
            animations[slot] = animation.GetAnimationCoroutine(transformNode);
        }
    }
}
