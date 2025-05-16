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

        [SerializeField] private SpriteRenderer baseSpriteRenderer;
        [SerializeField] private SpriteRenderer handleSpriteRenderer;

        [SerializeField] private SpriteRenderer timingDragCircleSpriteRenderer;
        [SerializeField] private Color          timingCircleDefaultColor;
        [SerializeField] private Color          timingCircleDraggingColor;

        // [SerializeField] private SpriteRenderer dragCircleRenderer;

        [SerializeField] private MixGamePointerCollider pointerCollider;

        private float _hitTiming;
        private bool  _isMoving;
        private float _moveDuration;
        private int   _tileType;

        public override void Initialize(MixGameTileViewModel tileViewModel)
        {
            base.Initialize(tileViewModel);
            _hitTiming = tileViewModel.TileModel.HitTiming;
            var movableData = (MixGameMovableSequenceElementData)tileViewModel.TileModel.Data;

            _moveDuration = movableData.MoveDuration;
            _tileType     = movableData.TileType;
            _isMoving     = false;

            pointerCollider.Collider.enabled = true;

            viewRotationPivot.localRotation              = Quaternion.Euler(0, 0, movableData.RotationZEuler);
            textMeshVisualNumber.transform.localRotation = Quaternion.Euler(0, 0, -movableData.RotationZEuler);

            handleSpriteRenderer.transform.localPosition           = Vector3.zero;
            timingDragCircleSpriteRenderer.transform.localPosition = Vector3.zero;
            pointerCollider.transform.localPosition                = Vector3.zero;
            textMeshVisualNumber.transform.localPosition           = Vector3.zero;

            var textInitColor = textMeshVisualNumber.color;
            textMeshVisualNumber.color           = new Color(textInitColor.r, textInitColor.g, textInitColor.b, 1);
            handleSpriteRenderer.color           = Color.white;
            baseSpriteRenderer.color             = Color.white;
            timingDragCircleSpriteRenderer.color = timingCircleDefaultColor;

            ((CircleCollider2D)pointerCollider.Collider).radius = 1f;

            _ = PlayAnimationAndWaitAsync("Shrink", 1);
        }

        protected override void OnHit()
        {
            base.OnHit();
            if (!_isMoving)
            {
                _isMoving = true;

                _ = PlayAnimationAndWaitAsync("Hit", 0);
                _ = PlayAnimationAndWaitAsync("TimingDragCircleTransform", 1);

                ((CircleCollider2D)pointerCollider.Collider).radius *= 2f;
            }
            else
            {
                pointerCollider.Collider.enabled = false;

                _ = PlayAnimationAndReturnToPoolAsync("HitReleased", 2);
                _ = PlayAnimationAndWaitAsync("TimingDragCircleFade", 1);
            }
        }

        protected override void OnMiss()
        {
            base.OnMiss();
            pointerCollider.Collider.enabled = false;

            _ = PlayAnimationAndReturnToPoolAsync("Miss", 0);
            _ = PlayAnimationAndWaitAsync("TimingDragCircleFade", 1);
        }

        protected override void OnFail()
        {
            base.OnFail();
            pointerCollider.Collider.enabled = false;

            _ = PlayAnimationAndReturnToPoolAsync("Fail", 0);
            _ = PlayAnimationAndWaitAsync("TimingDragCircleFade", 1);
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
            // todo: consider tileType

            var bezierPath = new[]
            {
                new Vector3(3f, 0, 0), // WP0
                new Vector3(0, 1.75f, 0), // A
                new Vector3(3f, 1.75f, 0) // B
            };

            var moveHandleTween = handleSpriteRenderer.transform
                .DOLocalPath(bezierPath, _moveDuration, PathType.CubicBezier).SetEase(Ease.Linear);
            var moveDragCircleTween = timingDragCircleSpriteRenderer.transform
                .DOLocalPath(bezierPath, _moveDuration, PathType.CubicBezier).SetEase(Ease.Linear);
            var moveColliderTween = pointerCollider.transform
                .DOLocalPath(bezierPath, _moveDuration, PathType.CubicBezier).SetEase(Ease.Linear);
            var moveTextTween = textMeshVisualNumber.transform
                .DOLocalPath(bezierPath, _moveDuration, PathType.CubicBezier).SetEase(Ease.Linear);

            var textInitColor = textMeshVisualNumber.color;
            var textColorTween = textMeshVisualNumber
                .DOColor(new Color(textInitColor.r, textInitColor.g, textInitColor.b, 0), 0.25f);

            return DOTween.Sequence()
                .Append(moveHandleTween)
                .Join(moveDragCircleTween)
                .Join(moveColliderTween)
                .Join(moveTextTween)
                .Join(textColorTween);
        }

        private Tween HitReleaseTween()
        {
            var baseColorTween = baseSpriteRenderer
                .DOColor(new Color(1, 1, 1, 0), 0.5f);
            var handleColorTween = handleSpriteRenderer
                .DOColor(new Color(1, 1, 1, 0), 0.5f);

            return DOTween.Sequence()
                .Append(baseColorTween)
                .Join(handleColorTween);
        }

        private Tween MissTween()
        {
            var baseColorTween = baseSpriteRenderer
                .DOColor(new Color(1, 1, 1, 0), 0.5f);
            var handleColorTween = handleSpriteRenderer
                .DOColor(new Color(1, 1, 1, 0), 0.5f);

            var textInitColor = textMeshVisualNumber.color;
            var textColorTween = textMeshVisualNumber
                .DOColor(new Color(textInitColor.r, textInitColor.g, textInitColor.b, 0), 0.25f);

            return DOTween.Sequence()
                .Append(baseColorTween)
                .Join(handleColorTween)
                .Join(textColorTween);
        }

        private Tween FailTween()
        {
            var baseColorTween = baseSpriteRenderer
                .DOColor(new Color(1, 1, 1, 0), 0.5f);
            var handleColorTween = handleSpriteRenderer
                .DOColor(new Color(1, 1, 1, 0), 0.5f);

            var textInitColor = textMeshVisualNumber.color;
            var textColorTween = textMeshVisualNumber
                .DOColor(new Color(textInitColor.r, textInitColor.g, textInitColor.b, 0), 0.25f);

            return DOTween.Sequence()
                .Append(baseColorTween)
                .Join(handleColorTween)
                .Join(textColorTween);
        }

        private Tween ShrinkTween()
        {
            var shrinkingTween = timingDragCircleSpriteRenderer
                .transform.DOScale(Vector3.one, _hitTiming)
                .From(Vector3.one * 3);
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
                .transform.DOScale(Vector3.one * 2f, 0.5f);
            var colorTween = timingDragCircleSpriteRenderer
                .DOColor(timingCircleDraggingColor, 0.5f);
            return DOTween.Sequence()
                .Append(scaleTween)
                .Join(colorTween);
        }

        private Tween TimingDragCircleFade()
        {
            var initColor = timingDragCircleSpriteRenderer.color;
            var colorTween = timingDragCircleSpriteRenderer.DOColor(
                new Color(initColor.r, initColor.g, initColor.b, 0),
                0.5f);

            var scaleTween = timingDragCircleSpriteRenderer
                .transform.DOScale(Vector3.one * 3f, 0.5f);

            return DOTween.Sequence()
                .Append(colorTween)
                .Join(scaleTween);
        }
    }
}
