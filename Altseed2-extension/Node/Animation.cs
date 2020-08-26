using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Node
{
    public partial class AnimationNode : Altseed2.Node
    {
        public class Animation
        {
            abstract class BaseAnimationElement
            {
                public bool isRequireFrom;
                public float duration;
                public EasingType easing;
            }

            List<BaseAnimationElement> animationElements;

            public Animation()
            {
                animationElements = new List<BaseAnimationElement>();
            }

            public IEnumerator<object> GetAnimationCoroutine(Altseed2.Node node)
            {
                foreach (var item in animationElements)
                {
                    switch (item)
                    {
                        case MoveAnimationElement move:
                            {
                                var coRutine = GetMoveCoroutine(node, move);
                                while (coRutine.MoveNext())
                                {
                                    yield return null;
                                }
                            }
                            break;
                        case ScaleAnimationElement scale:
                            {
                                var coRutine = GetScaleCoroutine(node, scale);
                                while (coRutine.MoveNext())
                                {
                                    yield return null;
                                }
                            }
                            break;
                        case RotateAnimationElement rotate:
                            {
                                var coRutine = GetRotateCoroutine(node, rotate);
                                while (coRutine.MoveNext())
                                {
                                    yield return null;
                                }
                            }
                            break;
                        case SleepAnimationElement sleep:
                            for (int i = 0; i < sleep.duration; i++)
                            {
                                yield return null;
                            }
                            break;
                        case UserAnimationElement user:
                            {
                                var coroutine = GetUserAnimationCoroutine(node, user);
                                while (coroutine.MoveNext())
                                {
                                    yield return null;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            IEnumerator<object> GetMoveCoroutine(Altseed2.Node node, MoveAnimationElement move)
            {
                if (node is TransformNode transformNode)
                {
                    Vector2F start = move.isRequireFrom ? move.from : transformNode.Position;
                    float current = 0;
                    while (current < move.duration)
                    {
                        transformNode.Position = new Vector2F(GetEasing(move.easing, move.duration, start.X, move.to.X, current), GetEasing(move.easing, move.duration, start.Y, move.to.Y, current));
                        yield return null;
                        current += Engine.DeltaSecond;
                    }
                }
                else if (node is AnchorTransformerNode anchorTransformerNode)
                {
                    Vector2F start = move.isRequireFrom ? move.from : anchorTransformerNode.Position;
                    float current = 0;
                    while (current < move.duration)
                    {
                        anchorTransformerNode.Position = new Vector2F(GetEasing(move.easing, move.duration, start.X, move.to.X, current), GetEasing(move.easing, move.duration, start.Y, move.to.Y, current));
                        yield return null;
                        current += Engine.DeltaSecond;
                    }
                }
            }


            IEnumerator<object> GetScaleCoroutine(Altseed2.Node node, ScaleAnimationElement scale)
            {
                if (node is TransformNode transformNode)
                {
                    Vector2F start = scale.isRequireFrom ? scale.from : transformNode.Scale;
                    float current = 0;
                    while (current < scale.duration)
                    {
                        transformNode.Scale = new Vector2F(GetEasing(scale.easing, scale.duration, start.X, scale.to.X, current), GetEasing(scale.easing, scale.duration, start.Y, scale.to.Y, current));
                        yield return null;
                        current += Engine.DeltaSecond;
                    }
                }
                else if (node is AnchorTransformerNode anchorTransformerNode)
                {
                    Vector2F start = scale.isRequireFrom ? scale.from : anchorTransformerNode.Scale;
                    float current = 0;
                    while (current < scale.duration)
                    {
                        anchorTransformerNode.Scale = new Vector2F(GetEasing(scale.easing, scale.duration, start.X, scale.to.X, current), GetEasing(scale.easing, scale.duration, start.Y, scale.to.Y, current));
                        yield return null;
                        current += Engine.DeltaSecond;
                    }
                }
            }

            IEnumerator<object> GetRotateCoroutine(Altseed2.Node node, RotateAnimationElement rotate)
            {
                if (node is TransformNode transformNode)
                {
                    float start = rotate.isRequireFrom ? rotate.from : transformNode.Angle;
                    float current = 0;
                    while (current < rotate.duration)
                    {
                        transformNode.Angle = GetEasing(rotate.easing, rotate.duration, start, rotate.to, current);
                        yield return null;
                        current += Engine.DeltaSecond;
                    }
                }
                else if (node is AnchorTransformerNode anchorTransformerNode)
                {
                    float start = rotate.isRequireFrom ? rotate.from : anchorTransformerNode.Angle;
                    float current = 0;
                    while (current < rotate.duration)
                    {
                        anchorTransformerNode.Angle = GetEasing(rotate.easing, rotate.duration, start, rotate.to, current);
                        yield return null;
                        current += Engine.DeltaSecond;
                    }
                }
            }

            IEnumerator<object> GetUserAnimationCoroutine(Altseed2.Node node, UserAnimationElement userAnimation)
            {
                float current = 0;
                while (current < userAnimation.duration)
                {
                    userAnimation.easingFunc(userAnimation.easing, current, userAnimation.duration, node);
                    yield return null;
                    current += Engine.DeltaSecond;
                }
            }

            public Animation Move(Vector2F from, Vector2F to, float duration, EasingType easing = EasingType.Linear)
            {
                var element = new MoveAnimationElement
                {
                    from = from,
                    to = to,
                    duration = duration > 0 ? duration : 1,
                    easing = easing,
                    isRequireFrom = true
                };
                animationElements.Add(element);

                return this;
            }

            public Animation MoveTo(Vector2F to, float duration, EasingType easing = EasingType.Linear)
            {
                var element = new MoveAnimationElement
                {
                    to = to,
                    duration = duration > 0 ? duration : 1,
                    easing = easing,
                    isRequireFrom = false
                };
                animationElements.Add(element);

                return this;
            }

            class MoveAnimationElement : BaseAnimationElement
            {
                public Vector2F to;
                public Vector2F from;
            }

            public Animation Scale(Vector2F from, Vector2F to, float duration, EasingType easing = EasingType.Linear)
            {
                var element = new ScaleAnimationElement
                {
                    from = from,
                    to = to,
                    duration = duration > 0 ? duration : 1,
                    easing = easing,
                    isRequireFrom = true
                };
                animationElements.Add(element);

                return this;
            }

            public Animation ScaleTo(Vector2F to, float duration, EasingType easing = EasingType.Linear)
            {
                var element = new ScaleAnimationElement
                {
                    to = to,
                    duration = duration > 0 ? duration : 1,
                    easing = easing,
                    isRequireFrom = false
                };
                animationElements.Add(element);

                return this;
            }

            class ScaleAnimationElement : BaseAnimationElement
            {
                public Vector2F to;
                public Vector2F from;
            }

            public Animation Rotate(float from, float to, float duration, EasingType easing = EasingType.Linear)
            {
                var element = new RotateAnimationElement
                {
                    from = from,
                    to = to,
                    duration = duration > 0 ? duration : 1,
                    easing = easing,
                    isRequireFrom = true
                };
                animationElements.Add(element);

                return this;
            }

            public Animation RotateTo(float to, float duration, EasingType easing = EasingType.Linear)
            {
                var element = new RotateAnimationElement
                {
                    to = to,
                    duration = duration > 0 ? duration : 1,
                    easing = easing,
                    isRequireFrom = false
                };
                animationElements.Add(element);

                return this;
            }

            class RotateAnimationElement : BaseAnimationElement
            {
                public float to;
                public float from;
            }

            public Animation AnimateUserFunc(float duration, Action<EasingType, float, float, Altseed2.Node> easingFunc, EasingType easing = EasingType.Linear)
            {
                var element = new UserAnimationElement
                {
                    duration = duration > 0 ? duration : 1,
                    easing = easing,
                    isRequireFrom = false,
                    easingFunc = easingFunc
                };
                animationElements.Add(element);

                return this;
            }

            class UserAnimationElement : BaseAnimationElement
            {
                public Action<EasingType, float, float, Altseed2.Node> easingFunc;
            }

            public Animation Sleep(float duration)
            {
                var element = new SleepAnimationElement
                {
                    duration = duration > 0 ? duration : 1,
                    isRequireFrom = false
                };
                animationElements.Add(element);

                return this;
            }

            class SleepAnimationElement : BaseAnimationElement
            {
            }

            public static float GetEasing(EasingType easing, float duration, float start, float end, float current) => Easing.GetEasing(easing, current / duration) * (end - start) + start;
        }
    }
}
