using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.MixMinigame.Views;
using Features.Presenter;
using TMPro;
using UnityEngine;
using VContainer;

namespace Features.MixMinigame.Presenters
{
    public abstract class MixGameTilePresenter : TweenedPresenter
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
                hitStatusParticleSystem.Simulate(t: deltaTime, withChildren: true, restart: false,
                    fixedTimeStep: false);
        }

        public event Action OnReturnToPool;

        public virtual void Initialize(MixGameTileView tileView)
        {
            base.Initialize();

            tileView.OnHit  += OnHit;
            tileView.OnMiss += OnMiss;
            tileView.OnFail += OnFail;

            textMeshVisualNumber.text = tileView.TileModel.Data.VisualNumber.ToString();

            transform.localPosition = MixGamePlayingFieldService.ConvertRelativeToWorldPosition(
                tileView.TileModel.Data.InitialPosition);
        }

        public void ReturnToPool()
        {
            OnReturnToPool?.Invoke();
            OnReturnToPool = null;

            ClearAnimations();

            hitStatusParticleSystem.gameObject.SetActive(false);
            hitStatusParticleSystem.Stop(withChildren: true,
                stopBehavior: ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        protected virtual void OnHit()
        {
            // CAREFUL: movable ignores it on hold begin

            hitStatusParticleSystem.gameObject.SetActive(true);
            var main = hitStatusParticleSystem.main;
            main.startColor = hitPSColor;
            hitStatusParticleSystem.Simulate(t: 0, withChildren: true, restart: true);
        }

        protected virtual void OnMiss()
        {
            hitStatusParticleSystem.gameObject.SetActive(true);
            var main = hitStatusParticleSystem.main;
            main.startColor = missPSColor;
            hitStatusParticleSystem.Simulate(t: 0, withChildren: true, restart: true);
        }

        protected virtual void OnFail()
        {
            hitStatusParticleSystem.gameObject.SetActive(true);
            var main = hitStatusParticleSystem.main;
            main.startColor = missPSColor;
            hitStatusParticleSystem.Simulate(t: 0, withChildren: true, restart: true);
        }

        protected abstract UniTask ResolveAnimation(string animationName, CancellationToken ct);

        protected async UniTask PlayAnimationAndWaitAsync(string animationName, int layer)
        {
            CancelCurrentAnimationAwait(layer);
            if (isDebugMode)
                Debug.Log($"PlayAnimationAndWaitAsync {animationName} - Start");
            var cts = new CancellationTokenSource();
            AnimationCtsWithLayers.Add(key: cts, value: layer);

            await ResolveAnimation(animationName: animationName, ct: cts.Token);
            if (isDebugMode)
                Debug.Log($"PlayAnimationAndWaitAsync {animationName} - Finish");
        }

        protected async UniTask PlayAnimationAndReturnToPoolAsync(string animationName, int layer)
        {
            await PlayAnimationAndWaitAsync(animationName: animationName, layer: layer);
            if (isDebugMode)
                Debug.Log($"PlayAnimationAndReturnToPoolAsync {animationName} - ReturnToPool");
            ReturnToPool();
        }
    }
}
