using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Features.TimeSystem.Interfaces.Handlers;
using UnityEngine;

namespace Features.View
{
    public abstract class TweenedView : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] protected bool isDebugMode = false;

        protected Dictionary<CancellationTokenSource, int> AnimationCtsWithLayers;

        protected List<Tween> Tweens;

        public virtual void OnUpdate(float deltaTime)
        {
            for (var i = 0; i < Tweens?.Count; i++)
            {
                if (Tweens[i] == null) continue;
                Tweens[i].ManualUpdate(deltaTime, Time.deltaTime);
            }
        }

        protected void Initialize()
        {
            Tweens                 = new List<Tween>();
            AnimationCtsWithLayers = new Dictionary<CancellationTokenSource, int>();
        }

        protected UniTask MorphAnimationTweenToUniTask(Tween tween, CancellationToken ct)
        {
            tween.SetUpdate(UpdateType.Manual);
            Tweens.Add(tween);
            tween.OnKill(() =>
            {
                if (Tweens.Contains(tween)) Tweens.Remove(tween);
            });

            return tween.WithCancellation(ct);
        }

        protected void ClearAnimations()
        {
            for (var i = 0; i < AnimationCtsWithLayers?.Count; i++)
            {
                var cts = AnimationCtsWithLayers.Keys.ElementAt(i);
                cts.Cancel();
                cts.Dispose();
            }

            AnimationCtsWithLayers?.Clear();

            for (var i = Tweens.Count - 1; i >= 0; i--)
                if (Tweens[i] != null && Tweens[i].active)
                    Tweens[i].Kill();

            Tweens.Clear();
        }

        protected void CancelCurrentAnimationAwait(int layer)
        {
            if (isDebugMode)
                Debug.Log($"CancelCurrentAnimationAwait {layer}");
            for (var i = 0; i < AnimationCtsWithLayers?.Count; i++)
            {
                var cts = AnimationCtsWithLayers.Keys.ElementAt(i);
                if (AnimationCtsWithLayers[cts] != layer) continue;

                cts.Cancel();
                cts.Dispose();
                AnimationCtsWithLayers.Remove(cts);
                break;
            }
        }
    }
}