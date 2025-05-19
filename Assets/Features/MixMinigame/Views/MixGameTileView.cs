using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.MixMinigame.ViewModels;
using Features.View;
using TMPro;
using UnityEngine;
using VContainer;

namespace Features.MixMinigame.Views
{
    public abstract class MixGameTileView : TweenedView
    {
        [SerializeField] protected TextMeshPro    textMeshVisualNumber;
        [SerializeField] protected ParticleSystem hitStatusParticleSystem;

        [SerializeField] private Color hitPSColor;
        [SerializeField] private Color missPSColor;

        [Inject] protected readonly MixGamePlayingFieldService MixGamePlayingFieldService;

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (hitStatusParticleSystem.gameObject.activeInHierarchy)
                hitStatusParticleSystem.Simulate(deltaTime, true, false, false);
        }

        public event Action OnReturnToPool;

        public virtual void Initialize(MixGameTileViewModel tileViewModel)
        {
            base.Initialize();

            tileViewModel.OnHit  += OnHit;
            tileViewModel.OnMiss += OnMiss;
            tileViewModel.OnFail += OnFail;

            textMeshVisualNumber.text = tileViewModel.TileModel.Data.VisualNumber.ToString();

            transform.localPosition = MixGamePlayingFieldService.ConvertRelativeToWorldPosition(
                tileViewModel.TileModel.Data.InitialPosition);
        }

        public void ReturnToPool()
        {
            OnReturnToPool?.Invoke();
            OnReturnToPool = null;

            ClearAnimations();

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
            AnimationCtsWithLayers.Add(cts, layer);

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
    }
}
