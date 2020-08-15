﻿using Altseed2;
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
                public int frame;
                public Easing easing;
            }

            List<BaseAnimationElement> animationElements;

            public Animation()
            {
                animationElements = new List<BaseAnimationElement>();
            }

            public IEnumerator<object> GetAnimationCoroutine(TransformNode transformNode)
            {
                foreach (var item in animationElements)
                {
                    switch (item)
                    {
                        case MoveAnimationElement move:
                            {
                                var coRutine = GetMoveCoroutine(transformNode, move);
                                while (coRutine.MoveNext())
                                {
                                    yield return null;
                                }
                            }
                            break;
                        case ScaleAnimationElement scale:
                            {
                                var coRutine = GetScaleCoroutine(transformNode, scale);
                                while (coRutine.MoveNext())
                                {
                                    yield return null;
                                }
                            }
                            break;
                        case RotateAnimationElement rotate:
                            {
                                var coRutine = GetRotateCoroutine(transformNode, rotate);
                                while (coRutine.MoveNext())
                                {
                                    yield return null;
                                }
                            }
                            break;
                        case SleepAnimationElement sleep:
                            for (int i = 0; i < sleep.frame; i++)
                            {
                                yield return null;
                            }
                            break;
                        case UserAnimationElement user:
                            {
                                var coroutine = GetUserAnimationCoroutine(transformNode, user);
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

            IEnumerator<object> GetMoveCoroutine(TransformNode transformNode, MoveAnimationElement move)
            {
                Vector2F start = move.isRequireFrom ? move.from : transformNode.Position;
                for (int i = 1; i <= move.frame; i++)
                {
                    transformNode.Position = new Vector2F(GetEasing(move.easing, i, start.X, move.to.X, move.frame), GetEasing(move.easing, i, start.Y, move.to.Y, move.frame));
                    yield return null;
                }
            }

            IEnumerator<object> GetScaleCoroutine(TransformNode transformNode, ScaleAnimationElement scale)
            {
                Vector2F start = scale.isRequireFrom ? scale.from : transformNode.Scale;
                for (int i = 1; i <= scale.frame; i++)
                {
                    transformNode.Scale = new Vector2F(GetEasing(scale.easing, i, start.X, scale.to.X, scale.frame), GetEasing(scale.easing, i, start.Y, scale.to.Y, scale.frame));
                    yield return null;
                }
            }

            IEnumerator<object> GetRotateCoroutine(TransformNode transformNode, RotateAnimationElement rotate)
            {
                float start = rotate.isRequireFrom ? rotate.from : transformNode.Angle;
                for (int i = 1; i <= rotate.frame; i++)
                {
                    transformNode.Angle = GetEasing(rotate.easing, i, start, rotate.to, rotate.frame);
                    yield return null;
                }
            }

            IEnumerator<object> GetUserAnimationCoroutine(TransformNode transformNode, UserAnimationElement userAnimation)
            {
                for (int i = 1; i <= userAnimation.frame; i++)
                {
                    userAnimation.easingFunc(userAnimation.easing, i, userAnimation.frame, transformNode);
                    yield return null;
                }
            }

            public float GetEasing(Easing easing, int current, float start, float end, int frame)
            {
                if (current == 0) return start;
                if (current == frame) return end;

                float t = (float)current / frame;
                if (current > frame) t = 1;
                else if (current < 0) t = 0;

                float d = end - start;

                switch (easing)
                {
                    case Easing.Linear:
                        return start + d * t;
                    case Easing.InSine:
                        return -d * (float)Math.Cos(t * MathHelper.DegreeToRadian(90)) + d + start;
                    case Easing.OutSine:
                        return d * (float)Math.Sin(t * MathHelper.DegreeToRadian(90)) + start;
                    case Easing.InOutSine:
                        return -d / 2 * ((float)Math.Cos(t * Math.PI) - 1) + start;
                    case Easing.InQuad:
                        return d * t * t + start;
                    case Easing.OutQuad:
                        return d * t * (2 - t) + start;
                    case Easing.InOutQuad:
                        return d * (t < 0.5 ? 2 * t * t : t * (4 - 2 * t) - 1) + start;
                    case Easing.InCubic:
                        return d * t * t * t + start;
                    case Easing.OutCubic:
                        return d * (--t * t * t + 1) + start;
                    case Easing.InOutCubic:
                        return d * (t < 0.5f ? 4 * t * t * t : 1 + (--t) * (2 * t) * (2 * t)) + start;
                    case Easing.InQuart:
                        return d * t * t * t * t + start;
                    case Easing.OutQuart:
                        return -d * (--t * t * t * t - 1) + start;
                    case Easing.InOutQuart:
                        if (t < 0.5)
                        {
                            t *= t;
                            return d * 8 * t * t + start;
                        }
                        else
                        {
                            t = (--t) * t;
                            return d * (1 - 8 * t * t) + start;
                        }
                    case Easing.InQuint:
                        return d * t * t * t * t * t + start;
                    case Easing.OutQuint:
                        return d * (--t * t * t * t * t + 1) + start;
                    case Easing.InOutQuint:
                        {
                            float t2;
                            if (t < 0.5)
                            {
                                t2 = t * t;
                                return d * 16 * t * t2 * t2 + start;
                            }
                            else
                            {
                                t2 = (--t) * t;
                                return d * (1 + 16 * t * t2 * t2) + start;
                            }
                        }
                    case Easing.InExpo:
                        return t == 0.0f ? start : d * (float)Math.Pow(2, 10 * (t - 1)) + start;
                    case Easing.OutExpo:
                        return t == 1 ? d + start : d * (-(float)Math.Pow(2, -10 * t) + 1) + start;
                    case Easing.InOutExpo:
                        if (t == 0.0)
                            return start;
                        if (t == end)
                            return end;
                        if (t < 0.5)
                        {
                            return d * ((float)Math.Pow(2, 16 * t) - 1) / 510 + start;
                        }
                        else
                        {
                            return d * (1 - 0.5f * (float)Math.Pow(2, -16 * (t - 0.5))) + start;
                        }
                    case Easing.InCirc:
                        return d * (1 - (float)Math.Sqrt(1 - t)) + start;
                    case Easing.OutCirc:
                        return d * (float)Math.Sqrt(t) + start;
                    case Easing.InOutCirc:
                        if (t < 0.5)
                        {
                            return d * (1 - (float)Math.Sqrt(1 - 2 * t)) * 0.5f + start;
                        }
                        else
                        {
                            return d * (1 + (float)Math.Sqrt(2 * t - 1)) * 0.5f + start;
                        }
                    case Easing.InBack:
                        return d * t * t * (2.70158f * t - 1.70158f) + start;
                    case Easing.OutBack:
                        return d * (1 + (--t) * t * (2.70158f * t + 1.70158f)) + start;
                    case Easing.InOutBack:
                        if (t < 0.5)
                        {
                            return d * t * t * (7 * t - 2.5f) * 2 + start;
                        }
                        else
                        {
                            return d * (1 + (--t) * t * 2 * (7 * t + 2.5f)) + start;
                        }
                    case Easing.InElastic:
                        return d * t * t * t * t * (float)Math.Sin(t * Math.PI * 4.5) + start;
                    case Easing.OutElastic:
                        {
                            float t2 = (t - 1) * (t - 1);
                            return d * (1 - t2 * t2 * (float)Math.Cos(t * Math.PI * 4.5)) + start;
                        }
                    case Easing.InOutElastic:
                        {
                            float t2;
                            if (t < 0.45)
                            {
                                t2 = t * t;
                                return d * 8 * t2 * t2 * (float)Math.Sin(t * Math.PI * 9) + start;
                            }
                            else if (t < 0.55)
                            {
                                return d * (0.5f + 0.75f * (float)Math.Sin(t * Math.PI * 4)) + start;
                            }
                            else
                            {
                                t2 = (t - 1) * (t - 1);
                                return d * (1 - 8 * t2 * t2 * (float)Math.Sin(t * Math.PI * 9)) + start;
                            }
                        }
                    case Easing.InBounce:
                        return d * (float)Math.Pow(2, 6 * (t - 1)) * Math.Abs((float)Math.Sin(t * Math.PI * 3.5)) + start;
                    case Easing.OutBounce:
                        return d * (1 - (float)Math.Pow(2, -6 * t) * Math.Abs((float)Math.Cos(t * (float)Math.PI * 3.5))) + start;
                    case Easing.InOutBounce:
                        if (t < 0.5)
                        {
                            return d * 8 * (float)Math.Pow(2, 8 * (t - 1)) * Math.Abs((float)Math.Sin(t * (float)Math.PI * 7)) + start;
                        }
                        else
                        {
                            return d * (1 - 8 * (float)Math.Pow(2, -8 * t) * Math.Abs((float)Math.Sin(t * (float)Math.PI * 7))) + start;
                        }
                    default:
                        return 0;
                }
            }

            public void Move(Vector2F from, Vector2F to, int frame, Easing easing = Easing.Linear)
            {
                var element = new MoveAnimationElement
                {
                    from = from,
                    to = to,
                    frame = frame > 0 ? frame : 1,
                    easing = easing,
                    isRequireFrom = true
                };
                animationElements.Add(element);
            }

            public void MoveTo(Vector2F to, int frame, Easing easing = Easing.Linear)
            {
                var element = new MoveAnimationElement
                {
                    to = to,
                    frame = frame > 0 ? frame : 1,
                    easing = easing,
                    isRequireFrom = false
                };
                animationElements.Add(element);
            }

            class MoveAnimationElement : BaseAnimationElement
            {
                public Vector2F to;
                public Vector2F from;
            }

            public void Scale(Vector2F from, Vector2F to, int frame, Easing easing = Easing.Linear)
            {
                var element = new ScaleAnimationElement
                {
                    from = from,
                    to = to,
                    frame = frame > 0 ? frame : 1,
                    easing = easing,
                    isRequireFrom = true
                };
                animationElements.Add(element);
            }

            public void ScaleTo(Vector2F to, int frame, Easing easing = Easing.Linear)
            {
                var element = new ScaleAnimationElement
                {
                    to = to,
                    frame = frame > 0 ? frame : 1,
                    easing = easing,
                    isRequireFrom = false
                };
                animationElements.Add(element);
            }

            class ScaleAnimationElement : BaseAnimationElement
            {
                public Vector2F to;
                public Vector2F from;
            }

            public void Rotate(float from, float to, int frame, Easing easing = Easing.Linear)
            {
                var element = new RotateAnimationElement
                {
                    from = from,
                    to = to,
                    frame = frame > 0 ? frame : 1,
                    easing = easing,
                    isRequireFrom = true
                };
                animationElements.Add(element);
            }

            public void RotateTo(float to, int frame, Easing easing = Easing.Linear)
            {
                var element = new RotateAnimationElement
                {
                    to = to,
                    frame = frame > 0 ? frame : 1,
                    easing = easing,
                    isRequireFrom = false
                };
                animationElements.Add(element);
            }

            class RotateAnimationElement : BaseAnimationElement
            {
                public float to;
                public float from;
            }

            public void AnimateUserFunc(int frame, Action<Easing, int, int, TransformNode> easingFunc, Easing easing = Easing.Linear)
            {
                var element = new UserAnimationElement
                {
                    frame = frame > 0 ? frame : 1,
                    easing = easing,
                    isRequireFrom = false,
                    easingFunc = easingFunc
                };
                animationElements.Add(element);
            }

            class UserAnimationElement : BaseAnimationElement
            {
                public Action<Easing, int, int, TransformNode> easingFunc;
            }

            public void Sleep(int frame)
            {
                var element = new SleepAnimationElement
                {
                    frame = frame > 0 ? frame : 1,
                    isRequireFrom = false
                };
                animationElements.Add(element);
            }

            class SleepAnimationElement : BaseAnimationElement
            {
            }

            public enum Easing
            {
                Linear,
                InSine,
                OutSine,
                InOutSine,
                InQuad,
                OutQuad,
                InOutQuad,
                InCubic,
                OutCubic,
                InOutCubic,
                InQuart,
                OutQuart,
                InOutQuart,
                InQuint,
                OutQuint,
                InOutQuint,
                InExpo,
                OutExpo,
                InOutExpo,
                InCirc,
                OutCirc,
                InOutCirc,
                InBack,
                OutBack,
                InOutBack,
                InElastic,
                OutElastic,
                InOutElastic,
                InBounce,
                OutBounce,
                InOutBounce,
            }
        }
    }
}
