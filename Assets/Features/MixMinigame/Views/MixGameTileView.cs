using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Features.MixMinigame.ViewModels;
using Features.TimeSystem.Interfaces.Handlers;
using TMPro;
using UnityEngine;
using VContainer;

namespace Features.MixMinigame.Views
{
    public abstract class MixGameTileView : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] protected TextMeshPro    textMeshVisualNumber;
        [SerializeField] protected ParticleSystem hitStatusParticleSystem;

        [SerializeField] private Color hitPSColor;
        [SerializeField] private Color missPSColor;

        [SerializeField] private bool isDebugMode = false;

        [Inject] protected readonly MixGamePlayingFieldService MixGamePlayingFieldService;

        private Dictionary<CancellationTokenSource, int> _animationCtsWithLayers;

        protected List<Tween> Tweens;

        public virtual void OnUpdate(float deltaTime)
        {
            for (var i = 0; i < Tweens?.Count; i++)
            {
                if (Tweens[i] == null) continue;
                Tweens[i].ManualUpdate(deltaTime, Time.deltaTime);
            }

            if (hitStatusParticleSystem.gameObject.activeInHierarchy)
                hitStatusParticleSystem.Simulate(deltaTime, true, false, false);
        }

        public event Action OnReturnToPool;


        public virtual void Initialize(MixGameTileViewModel tileViewModel)
        {
            tileViewModel.OnHit  += OnHit;
            tileViewModel.OnMiss += OnMiss;
            tileViewModel.OnFail += OnFail;

            textMeshVisualNumber.text = tileViewModel.TileModel.Data.VisualNumber.ToString();

            transform.localPosition = MixGamePlayingFieldService.ConvertRelativeTilePositionToAbsolute(
                tileViewModel.TileModel.Data.InitialPosition);

            Tweens                  = new List<Tween>();
            _animationCtsWithLayers = new Dictionary<CancellationTokenSource, int>();
        }

        public void ReturnToPool()
        {
            OnReturnToPool?.Invoke();
            OnReturnToPool = null;

            for (var i = 0; i < _animationCtsWithLayers?.Count; i++)
            {
                var cts = _animationCtsWithLayers.Keys.ElementAt(i);
                cts.Cancel();
                cts.Dispose();
            }

            _animationCtsWithLayers?.Clear();

            for (var i = Tweens.Count - 1; i >= 0; i--)
                if (Tweens[i] != null && Tweens[i].active)
                    Tweens[i].Kill();

            Tweens.Clear();

            hitStatusParticleSystem.gameObject.SetActive(false);
            hitStatusParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        protected virtual void OnHit()
        {
            // CAREFUL: movable ignores it on hold begin

            hitStatusParticleSystem.gameObject.SetActive(true);
            var main = hitStatusParticleSystem.main;
            main.startColor = hitPSColor;
            hitStatusParticleSystem.Simulate(0, true, true);
        }

        protected virtual void OnMiss()
        {
            hitStatusParticleSystem.gameObject.SetActive(true);
            var main = hitStatusParticleSystem.main;
            main.startColor = missPSColor;
            hitStatusParticleSystem.Simulate(0, true, true);
        }

        protected virtual void OnFail()
        {
            hitStatusParticleSystem.gameObject.SetActive(true);
            var main = hitStatusParticleSystem.main;
            main.startColor = missPSColor;
            hitStatusParticleSystem.Simulate(0, true, true);
        }

        protected abstract UniTask ResolveAnimation(string animationName, CancellationToken ct);

        protected async UniTask PlayAnimationAndWaitAsync(string animationName, int layer)
        {
            CancelCurrentAnimationAwait(layer);
            if (isDebugMode)
                Debug.Log($"PlayAnimationAndWaitAsync {animationName} - Start");
            var cts = new CancellationTokenSource();
            _animationCtsWithLayers.Add(cts, layer);

            await ResolveAnimation(animationName, cts.Token);
            if (isDebugMode)
                Debug.Log($"PlayAnimationAndWaitAsync {animationName} - Finish");
        }

        protected async UniTask PlayAnimationAndReturnToPoolAsync(string animationName, int layer)
        {
            await PlayAnimationAndWaitAsync(animationName, layer);
            if (isDebugMode)
                Debug.Log($"PlayAnimationAndReturnToPoolAsync {animationName} - ReturnToPool");
            ReturnToPool();
        }

        private void CancelCurrentAnimationAwait(int layer)
        {
            if (isDebugMode)
                Debug.Log($"CancelCurrentAnimationAwait {layer}");
            for (var i = 0; i < _animationCtsWithLayers?.Count; i++)
            {
                var cts = _animationCtsWithLayers.Keys.ElementAt(i);
                if (_animationCtsWithLayers[cts] != layer) continue;

                cts.Cancel();
                cts.Dispose();
                _animationCtsWithLayers.Remove(cts);
                break;
            }
        }
    }
}
