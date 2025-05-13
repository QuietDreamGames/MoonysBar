using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Features.MixMinigame.Datas;
using Features.MixMinigame.ViewModels;
using UnityEngine;

namespace Features.MixMinigame.Views
{
    public class MixGameTileMovableView : MixGameTileView
    {
        [SerializeField] private Transform viewRotationPivot;

        [SerializeField] private SpriteRenderer handleSpriteRenderer;
        [SerializeField] private SpriteRenderer timingCircleSpriteRenderer;
        [SerializeField] private SpriteRenderer dragCircleRenderer;

        [SerializeField] private MixGamePointerCollider pointerCollider;


        private int   _tileType;
        private float _hitTiming;
        private float _moveDuration;
        private bool  _isMoving;

        private Vector3 _initialBaseScale;
        private Vector3 _initialTimingCircleScale;

        public override void Initialize(MixGameTileViewModel tileViewModel)
        {
            base.Initialize(tileViewModel);
            _hitTiming = tileViewModel.TileModel.HitTiming;
            var movableData = (MixGameMovableSequenceElementData)tileViewModel.TileModel.Data;

            _moveDuration             = movableData.MoveDuration;
            _tileType                 = movableData.TileType;
            _isMoving                 = false;
            _initialBaseScale         = transform.localScale;
            _initialTimingCircleScale = timingCircleSpriteRenderer.transform.localScale;

            viewRotationPivot.localRotation = Quaternion.Euler(0, 0, movableData.RotationZEuler);

            var initHandleColor = handleSpriteRenderer.color;
            handleSpriteRenderer.color = new Color(
                initHandleColor.r, initHandleColor.g, initHandleColor.b,
                1);

            dragCircleRenderer.gameObject.SetActive(false);

            ((CircleCollider2D)pointerCollider.Collider).radius = 1f;

            _ = PlayAnimationAndWaitAsync("Shrink", 1);
        }

        public override void ReturnToPool()
        {
            base.ReturnToPool();

            transform.localScale                            = _initialBaseScale;
            timingCircleSpriteRenderer.transform.localScale = _initialTimingCircleScale;
        }

        protected override void OnHit()
        {
            if (!_isMoving)
            {
                _isMoving = true;

                _ = PlayAnimationAndWaitAsync("Hit", 0);
                _ = PlayAnimationAndWaitAsync("ShrinkCircleFade", 1);
                _ = PlayAnimationAndWaitAsync("DragCircleFadeIn", 2);

                ((CircleCollider2D)pointerCollider.Collider).radius *= 2f;
            }
            else
            {
                _ = PlayAnimationAndReturnToPoolAsync("HitReleased", 0);
                _ = PlayAnimationAndWaitAsync("DragCircleFade", 2);
            }
        }

        protected override void OnMiss()
        {
            _ = PlayAnimationAndReturnToPoolAsync("Miss", 0);
            _ = PlayAnimationAndWaitAsync("DragCircleFadeOut", 2);
        }

        protected override void OnFail()
        {
            _ = PlayAnimationAndReturnToPoolAsync("Fail", 0);
            _ = PlayAnimationAndWaitAsync("DragCircleFadeOut", 2);
        }

        protected override UniTask ResolveAnimation(string animationName, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        private Tween HitTween()
        {
            // cubic bezier paths - simple curve from 0, 0 to 0.5, 0.5 to 1, 0
            // todo: consider tileType

            var bezierPath = new []
            {
                new Vector3(0.5f, 0.5f, 0),  // WP0
                new Vector3(0, 1, 0),        // A
                new Vector3(-1, 0, 0),       // B
                new Vector3(1, 0, 0),        // WP1
                new Vector3(1, 0, 0),        // C
                new Vector3(0, 1, 0),        // D
            };

            var moveHandleTween = handleSpriteRenderer.transform
                .DOLocalPath(bezierPath, _moveDuration, PathType.CubicBezier);
            var moveDragCircleTween = dragCircleRenderer.transform
                .DOLocalPath(bezierPath, _moveDuration, PathType.CubicBezier);
            var moveColliderTween = pointerCollider.transform
                .DOLocalPath(bezierPath, _moveDuration, PathType.CubicBezier);

            // all at once:
            return DOTween.Sequence()
                .Append(moveHandleTween)
                .Join(moveDragCircleTween)
                .Join(moveColliderTween);

        }

        private Tween HitReleaseTween()
        {
            return transform.DOScale(_initialBaseScale * 1.2f, 0.5f)
                .From(_initialBaseScale);
        }

        private Tween MissTween()
        {
            return transform.DOScale(_initialBaseScale * 0.8f, 0.5f)
                .From(_initialBaseScale);
        }

        // todo : make shrink circle and drag circle the one.
    }
}
