using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Features.MixMinigame.ViewModels;
using Features.TimeSystem.Interfaces.Handlers;
using UnityEngine;
using VContainer;

namespace Features.MixMinigame.Views
{
    public abstract class MixGameTileView : MonoBehaviour, IUpdateHandler
    {
        [Inject] protected readonly MixGamePlayingFieldService MixGamePlayingFieldService;

        private Dictionary<CancellationTokenSource, int> _animationCtsWithLayers;

        protected List<Tween> Tweens;

        public virtual void OnUpdate(float deltaTime)
        {
            for (var i = 0; i < Tweens?.Count; i++)
            {
                if (Tweens[i] == null) continue;
                Tweens[i].SetUpdate(UpdateType.Manual);
                Tweens[i].ManualUpdate(deltaTime, Time.deltaTime);
            }
        }

        public event Action OnReturnToPool;


        public virtual void Initialize(MixGameTileViewModel tileViewModel)
        {
            tileViewModel.OnHit  += OnHit;
            tileViewModel.OnMiss += OnMiss;
            tileViewModel.OnFail += OnFail;

            transform.localPosition = MixGamePlayingFieldService.ConvertRelativeTilePositionToAbsolute(
                tileViewModel.TileModel.Data.InitialPosition);

            Tweens                  = new List<Tween>();
            _animationCtsWithLayers = new Dictionary<CancellationTokenSource, int>();
        }

        public virtual void ReturnToPool()
        {
            OnReturnToPool?.Invoke();
            OnReturnToPool = null;

            for (var i = Tweens.Count - 1; i >= 0; i--)
                if (Tweens[i] != null && Tweens[i].active)
                    Tweens[i].Kill();

            Tweens?.Clear();

            for (var i = 0; i < _animationCtsWithLayers?.Count; i++)
            {
                var cts = _animationCtsWithLayers.Keys.ElementAt(i);
                cts.Cancel();
                cts.Dispose();
            }
        }

        protected abstract void OnHit();
        protected abstract void OnMiss();
        protected abstract void OnFail();

        protected abstract UniTask ResolveAnimation(string animationName, CancellationToken ct);

        protected async UniTask PlayAnimationAndWaitAsync(string animationName, int layer)
        {
            CancelCurrentAnimationAwait(layer);
            var cts = new CancellationTokenSource();
            _animationCtsWithLayers.Add(cts, layer);

            await ResolveAnimation(animationName, cts.Token);
        }

        protected async UniTask PlayAnimationAndReturnToPoolAsync(string animationName, int layer)
        {
            CancelCurrentAnimationAwait(layer);
            await PlayAnimationAndWaitAsync(animationName, layer);
            ReturnToPool();
        }

        private void CancelCurrentAnimationAwait(int layer)
        {
            for (var i = 0; i < _animationCtsWithLayers?.Count; i++)
            {
                var cts = _animationCtsWithLayers.Keys.ElementAt(i);
                if (_animationCtsWithLayers[cts] == layer)
                {
                    cts.Cancel();
                    cts.Dispose();
                    _animationCtsWithLayers.Remove(cts);
                    break;
                }
            }
        }
    }
}
