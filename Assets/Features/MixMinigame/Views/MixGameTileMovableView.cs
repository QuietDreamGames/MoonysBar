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

        [SerializeField] private SpriteRenderer timingDragCircleSpriteRenderer;
        [SerializeField] private Color          timingCircleDefaultColor;
        [SerializeField] private Color          timingCircleDraggingColor;

        // [SerializeField] private SpriteRenderer dragCircleRenderer;

        [SerializeField] private MixGamePointerCollider pointerCollider;
        private                  float                  _hitTiming;

        private Vector3 _initialBaseScale;
        private Vector3 _initialTimingCircleScale;
        private bool    _isMoving;
        private float   _moveDuration;
        private int     _tileType;

        public override void Initialize(MixGameTileViewModel tileViewModel)
        {
            base.Initialize(tileViewModel);
            _hitTiming = tileViewModel.TileModel.HitTiming;
            var movableData = (MixGameMovableSequenceElementData)tileViewModel.TileModel.Data;

            _moveDuration             = movableData.MoveDuration;
            _tileType                 = movableData.TileType;
            _isMoving                 = false;
            _initialBaseScale         = transform.localScale;
            _initialTimingCircleScale = timingDragCircleSpriteRenderer.transform.localScale;

            viewRotationPivot.localRotation = Quaternion.Euler(0, 0, movableData.RotationZEuler);

            handleSpriteRenderer.transform.localPosition           = Vector3.zero;
            timingDragCircleSpriteRenderer.transform.localPosition = Vector3.zero;
            pointerCollider.transform.localPosition                = Vector3.zero;

            var initHandleColor = handleSpriteRenderer.color;
            handleSpriteRenderer.color = new Color(initHandleColor.r, initHandleColor.g, initHandleColor.b, 1);

            ((CircleCollider2D)pointerCollider.Collider).radius = 1f;

            _ = PlayAnimationAndWaitAsync("Shrink", 1);
        }

        public override void ReturnToPool()
        {
            base.ReturnToPool();

            transform.localScale                                = _initialBaseScale;
            timingDragCircleSpriteRenderer.transform.localScale = _initialTimingCircleScale;
        }

        protected override void OnHit()
        {
            if (!_isMoving)
            {
                _isMoving = true;

                _ = PlayAnimationAndWaitAsync("Hit", 0);
                _ = PlayAnimationAndWaitAsync("TimingDragCircleTransform", 2);

                ((CircleCollider2D)pointerCollider.Collider).radius *= 2f;
            }
            else
            {
                _ = PlayAnimationAndReturnToPoolAsync("HitReleased", 0);
                _ = PlayAnimationAndWaitAsync("TimingDragCircleFade", 2);
            }
        }

        protected override void OnMiss()
        {
            _ = PlayAnimationAndReturnToPoolAsync("Miss", 0);
            _ = PlayAnimationAndWaitAsync("TimingDragCircleFade", 2);
        }

        protected override void OnFail()
        {
            _ = PlayAnimationAndReturnToPoolAsync("Fail", 0);
            _ = PlayAnimationAndWaitAsync("TimingDragCircleFade", 2);
        }

        protected override UniTask ResolveAnimation(string animationName, CancellationToken ct)
        {
            var tween = animationName switch
            {
                "Hit"                       => HitTween(),
                "HitReleased"               => HitReleaseTween(),
                "Miss"                      => MissTween(),
                "Fail"                      => FailTween(),
                "Shrink"                    => ShrinkTween(),
                "TimingDragCircleTransform" => TimingDragCircleTransformTween(),
                "TimingDragCircleFade"      => TimingDragCircleFade(),
                _                           => null
            };

            tween.SetUpdate(UpdateType.Manual);
            Tweens.Add(tween);
            tween.OnKill(() =>
            {
                if (Tweens.Contains(tween)) Tweens.Remove(tween);
            });

            return tween.WithCancellation(ct);
        }

        private Tween HitTween()
        {
            // cubic bezier paths - simple curve from 0, 0 to 0.5, 0.5 to 1, 0

            // todo: consider tileType

            /*var bezierPath = new[]
            {
                new Vector3(0.5f, 0.5f, 0), // WP0
                new Vector3(0, 1, 0), // A
                new Vector3(-1, 0, 0), // B
                new Vector3(1, 0, 0), // WP1
                new Vector3(1, 0, 0), // C
                new Vector3(0, 1, 0) // D
            };*/

            var bezierPath = new[]
            {
                new Vector3(0.48f, 0, 0), // WP0
                new Vector3(0, 0.285f, 0), // A
                new Vector3(0.48f, 0.285f, 0) // B
            };

            var moveHandleTween = handleSpriteRenderer.transform
                .DOLocalPath(bezierPath, _moveDuration, PathType.CubicBezier);
            var moveDragCircleTween = timingDragCircleSpriteRenderer.transform
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

        private Tween FailTween()
        {
            return transform.DOScale(_initialBaseScale * 0.8f, 0.5f)
                .From(_initialBaseScale);
        }

        private Tween ShrinkTween()
        {
            var shrinkingTween = timingDragCircleSpriteRenderer
                .transform.DOScale(_initialTimingCircleScale, _hitTiming)
                .From(_initialTimingCircleScale * 3);
            var initColor = new Color(
                timingCircleDefaultColor.r,
                timingCircleDefaultColor.g,
                timingCircleDefaultColor.b,
                0);
            var coloringTween = timingDragCircleSpriteRenderer
                .DOColor(timingCircleDefaultColor, _hitTiming)
                .From(initColor);

            return DOTween.Sequence()
                .Append(shrinkingTween)
                .Join(coloringTween);
        }

        private Tween TimingDragCircleTransformTween()
        {
            var scaleTween = timingDragCircleSpriteRenderer
                .transform.DOScale(_initialTimingCircleScale * 2f, 0.5f);
            var colorTween = timingDragCircleSpriteRenderer
                .DOColor(timingCircleDraggingColor, 0.5f);
            return DOTween.Sequence()
                .Append(scaleTween)
                .Join(colorTween);
        }

        private Tween TimingDragCircleFade()
        {
            var initColor = timingDragCircleSpriteRenderer.color;
            return timingDragCircleSpriteRenderer.DOColor(
                new Color(initColor.r, initColor.g, initColor.b, 0),
                0.5f);
        }
    }
}
