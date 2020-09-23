using Altseed2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Extension
{
    public static class Coroutine
    {
        public static IEnumerator<IEnumerator> PlayCoroutine(IEnumerator<IEnumerator> coroutine)
        {
            var currentCoroutine = coroutine;
            Stack<IEnumerator<IEnumerator>> stackCroutine = new Stack<IEnumerator<IEnumerator>>();
            while (true)
            {
                if (currentCoroutine?.MoveNext() ?? false)
                {
                    if (currentCoroutine?.Current is IEnumerator<IEnumerator> sub)
                    {
                        stackCroutine.Push(currentCoroutine);
                        currentCoroutine = sub;
                    }
                }
                else
                {
                    if (stackCroutine.Count == 0) yield break;
                    currentCoroutine = stackCroutine.Pop();
                }
                yield return null;
            }
        }

        public static IEnumerator<IEnumerator> Delay(float duration)
        {
            float time = duration;
            while (time > 0)
            {
                time -= Engine.DeltaSecond;
                yield return null;
            }
        }
    }
}
