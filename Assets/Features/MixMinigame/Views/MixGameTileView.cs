using System;
using Features.MixMinigame.ViewModels;
using Features.TimeSystem.Interfaces.Handlers;
using UnityEngine;

namespace Features.MixMinigame.Views
{
    public abstract class MixGameTileView : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] protected Animator animator;
        
        public abstract void AnimateSuccessfulHit();
        public abstract void AnimateMissedHit();
        public abstract void AnimateTimeRunOut();

        public event Action OnReturnToPool;

        public virtual void Initialize(MixGameTileViewModel tileViewModel)
        {
            tileViewModel.OnHit += AnimateSuccessfulHit;
            tileViewModel.OnMiss += AnimateMissedHit;
            tileViewModel.OnFail += AnimateTimeRunOut;

            transform.localPosition = tileViewModel.TileModel.Data.InitialPosition;
        }

        public virtual void OnUpdate(float deltaTime)
        {
            animator.Update(deltaTime);
        }

        public virtual void ReturnToPool()
        {
            animator.Rebind();
            animator.Update(0);
            
            OnReturnToPool?.Invoke();
            OnReturnToPool = null;
        }
    }
}