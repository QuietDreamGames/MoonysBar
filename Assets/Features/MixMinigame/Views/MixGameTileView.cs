using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.MixMinigame.ViewModels;
using Features.TimeSystem.Interfaces.Handlers;
using UnityEngine;
using VContainer;

namespace Features.MixMinigame.Views
{
    public abstract class MixGameTileView : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] protected Animator animator;

        [Inject] protected readonly MixGamePlayingFieldService MixGamePlayingFieldService;

        private CancellationTokenSource _animationCts;

        public virtual void OnUpdate(float deltaTime)
        {
            animator.Update(deltaTime);
        }

        public event Action OnReturnToPool;

        public virtual void Initialize(MixGameTileViewModel tileViewModel)
        {
            tileViewModel.OnHit  += OnHit;
            tileViewModel.OnMiss += OnMiss;
            tileViewModel.OnFail += OnFail;

            transform.localPosition = MixGamePlayingFieldService.ConvertRelativeTilePositionToAbsolute(
                tileViewModel.TileModel.Data.InitialPosition);
        }

        public virtual void ReturnToPool()
        {
            animator.Rebind();
            animator.Update(0);

            OnReturnToPool?.Invoke();
            OnReturnToPool = null;

            _animationCts?.Cancel();
            _animationCts?.Dispose();
        }

        protected abstract void OnHit();
        protected abstract void OnMiss();
        protected abstract void OnFail();

        protected async UniTask PlayAnimationAndWaitAsync(string stateName, int layer = 0)
        {
            CancelCurrentAnimationAwait();
            _animationCts = new CancellationTokenSource();

            animator.Play(stateName, layer);

            await UniTask.WaitUntil(() =>
                    animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName) &&
                    animator.GetCurrentAnimatorStateInfo(layer).normalizedTime >= 0f,
                cancellationToken: _animationCts.Token);

            await UniTask.WaitUntil(() =>
                    !animator.IsInTransition(layer) &&
                    animator.GetCurrentAnimatorStateInfo(layer).normalizedTime >= 1f,
                cancellationToken: _animationCts.Token);
        }

        protected async UniTask PlayAnimationAndReturnToPoolAsync(string stateName, int layer = 0)
        {
            await PlayAnimationAndWaitAsync(stateName, layer);
            ReturnToPool();
        }

        private void CancelCurrentAnimationAwait()
        {
            if (_animationCts == null) return;

            _animationCts.Cancel();
            _animationCts.Dispose();
            _animationCts = null;
        }
    }
}
