using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Features.MixMinigame.Datas;
using Features.MixMinigame.Views;
using UnityEngine;

namespace Features.MixMinigame.Presenters
{
    public class MixGameTileMovablePresenter : MixGameTilePresenter
    {
        [SerializeField] private Transform viewRotationPivot;

        [SerializeField] private SpriteRenderer baseSpriteRenderer;
        [SerializeField] private SpriteRenderer handleSpriteRenderer;

        [SerializeField] private SpriteRenderer timingDragCircleSpriteRenderer;
        [SerializeField] private Color          timingCircleDefaultColor;
        [SerializeField] private Color          timingCircleDraggingColor;
        [SerializeField] private Color          timingCircleFailedDraggingColor;

        // [SerializeField] private SpriteRenderer dragCircleRenderer;

        [SerializeField] private MixGamePointerCollider pointerCollider;

        private float _hitTiming;
        private bool  _isMoving;
        private float _moveDuration;
        private int   _tileType;

        public override void Initialize(MixGameTileView tileView)
        {
            base.Initialize(tileView);
            _hitTiming = tileView.TileModel.HitTiming;
            var movableData = (MixGameMovableSequenceElementData)tileView.TileModel.Data;

            _moveDuration = movableData.MoveDuration;
            _tileType     = movableData.TileType;
            _isMoving     = false;

            pointerCollider.Collider.enabled = true;

            viewRotationPivot.localRotation              = Quaternion.Euler(x: 0, y: 0, z: movableData.RotationZEuler);
            textMeshVisualNumber.transform.localRotation = Quaternion.Euler(x: 0, y: 0, z: -movableData.RotationZEuler);

            handleSpriteRenderer.transform.localPosition           = Vector3.zero;
            timingDragCircleSpriteRenderer.transform.localPosition = Vector3.zero;
            pointerCollider.transform.localPosition                = Vector3.zero;
            textMeshVisualNumber.transform.localPosition           = Vector3.zero;
            hitStatusParticleSystem.transform.localPosition        = Vector3.zero;

            var textInitColor = textMeshVisualNumber.color;
            textMeshVisualNumber.color = new Color(r: textInitColor.r, g: textInitColor.g, b: textInitColor.b, a: 1);
            handleSpriteRenderer.color = Color.white;
            baseSpriteRenderer.color = Color.white;
            timingDragCircleSpriteRenderer.color = timingCircleDefaultColor;

            ((CircleCollider2D)pointerCollider.Collider).radius = 1f;

            _ = PlayAnimationAndWaitAsync(animationName: "Shrink", layer: 1);
        }

        protected override void OnHit()
        {
            if (!_isMoving)
            {
                _isMoving = true;

                _ = PlayAnimationAndWaitAsync(animationName: "Hit", layer: 0);
                _ = PlayAnimationAndWaitAsync(animationName: "TimingDragCircleTransform", layer: 1);

                ((CircleCollider2D)pointerCollider.Collider).radius *= 2f;
            }
            else
            {
                base.OnHit();
                pointerCollider.Collider.enabled = false;

                _ = PlayAnimationAndReturnToPoolAsync(animationName: "HitReleased", layer: 2);
                _ = PlayAnimationAndWaitAsync(animationName: "TimingDragCircleFade", layer: 1);
            }
        }

        protected override void OnMiss()
        {
            base.OnMiss();
            pointerCollider.Collider.enabled = false;

            timingDragCircleSpriteRenderer.color = new Color(
                r: timingCircleFailedDraggingColor.r,
                g: timingCircleFailedDraggingColor.g,
                b: timingCircleFailedDraggingColor.b,
                a: timingDragCircleSpriteRenderer.color.a);

            _ = PlayAnimationAndReturnToPoolAsync(animationName: "Miss", layer: 0);
            _ = PlayAnimationAndWaitAsync(animationName: "TimingDragCircleFade", layer: 1);
        }

        protected override void OnFail()
        {
            base.OnFail();
            pointerCollider.Collider.enabled = false;

            timingDragCircleSpriteRenderer.color = new Color(
                r: timingCircleFailedDraggingColor.r,
                g: timingCircleFailedDraggingColor.g,
                b: timingCircleFailedDraggingColor.b,
                a: timingDragCircleSpriteRenderer.color.a);

            _ = PlayAnimationAndReturnToPoolAsync(animationName: "Fail", layer: 0);
            _ = PlayAnimationAndWaitAsync(animationName: "TimingDragCircleFade", layer: 1);
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
                _ =>
                    throw new ArgumentOutOfRangeException(paramName: nameof(animationName), actualValue: animationName,
                        message: null)
            };

            return MorphAnimationTweenToUniTask(tween: tween, ct: ct);
        }

        private Tween HitTween()
        {
            // todo: consider tileType

            var bezierLocal = new[]
            {
                new Vector3(x: 3f, y: 0, z: 0),    // WP0 (local)
                new Vector3(x: 0, y: 1.75f, z: 0), // A (local)
                new Vector3(x: 3f, y: 1.75f, z: 0) // B (local)
            };

            // convert to world-space so gizmo and tween use the same space
            var worldPath = new Vector3[bezierLocal.Length];
            for (var i = 0; i < bezierLocal.Length; i++)
                worldPath[i] = handleSpriteRenderer.transform.TransformPoint(bezierLocal[i]);

            var moveHandleTween = handleSpriteRenderer.transform
                .DOPath(path: worldPath, duration: _moveDuration, pathType: PathType.CubicBezier)
                .SetEase(Ease.Linear);
            var moveDragCircleTween = timingDragCircleSpriteRenderer.transform
                .DOPath(path: worldPath, duration: _moveDuration, pathType: PathType.CubicBezier)
                .SetEase(Ease.Linear);
            var moveColliderTween = pointerCollider.transform
                .DOPath(path: worldPath, duration: _moveDuration, pathType: PathType.CubicBezier)
                .SetEase(Ease.Linear);
            var moveTextTween = textMeshVisualNumber.transform
                .DOPath(path: worldPath, duration: _moveDuration, pathType: PathType.CubicBezier)
                .SetEase(Ease.Linear);
            var moveParticlesTween = hitStatusParticleSystem.transform
                .DOPath(path: worldPath, duration: _moveDuration, pathType: PathType.CubicBezier)
                .SetEase(Ease.Linear);

            var textInitColor = textMeshVisualNumber.color;
            var textColorTween = textMeshVisualNumber
                .DOColor(endValue: new Color(r: textInitColor.r, g: textInitColor.g, b: textInitColor.b, a: 0),
                    duration: 0.25f);

            return DOTween.Sequence()
                .Append(moveHandleTween)
                .Join(moveDragCircleTween)
                .Join(moveColliderTween)
                .Join(moveTextTween)
                .Join(moveParticlesTween)
                .Join(textColorTween);
        }

        private Tween HitReleaseTween()
        {
            var baseColorTween = baseSpriteRenderer
                .DOColor(endValue: new Color(r: 1, g: 1, b: 1, a: 0), duration: 0.5f);
            var handleColorTween = handleSpriteRenderer
                .DOColor(endValue: new Color(r: 1, g: 1, b: 1, a: 0), duration: 0.5f);

            return DOTween.Sequence()
                .Append(baseColorTween)
                .Join(handleColorTween);
        }

        private Tween MissTween()
        {
            var baseColorTween = baseSpriteRenderer
                .DOColor(endValue: new Color(r: 1, g: 1, b: 1, a: 0), duration: 0.5f);
            var handleColorTween = handleSpriteRenderer
                .DOColor(endValue: new Color(r: 1, g: 1, b: 1, a: 0), duration: 0.5f);

            var textInitColor = textMeshVisualNumber.color;
            var textColorTween = textMeshVisualNumber
                .DOColor(endValue: new Color(r: textInitColor.r, g: textInitColor.g, b: textInitColor.b, a: 0),
                    duration: 0.25f);

            return DOTween.Sequence()
                .Append(baseColorTween)
                .Join(handleColorTween)
                .Join(textColorTween);
        }

        private Tween FailTween()
        {
            var baseColorTween = baseSpriteRenderer
                .DOColor(endValue: new Color(r: 1, g: 1, b: 1, a: 0), duration: 0.5f);
            var handleColorTween = handleSpriteRenderer
                .DOColor(endValue: new Color(r: 1, g: 1, b: 1, a: 0), duration: 0.5f);

            var textInitColor = textMeshVisualNumber.color;
            var textColorTween = textMeshVisualNumber
                .DOColor(endValue: new Color(r: textInitColor.r, g: textInitColor.g, b: textInitColor.b, a: 0),
                    duration: 0.25f);

            return DOTween.Sequence()
                .Append(baseColorTween)
                .Join(handleColorTween)
                .Join(textColorTween);
        }

        private Tween ShrinkTween()
        {
            var shrinkingTween = timingDragCircleSpriteRenderer
                .transform.DOScale(endValue: Vector3.one, duration: _hitTiming)
                .From(Vector3.one * 3);
            var initColor = new Color(
                r: timingCircleDefaultColor.r,
                g: timingCircleDefaultColor.g,
                b: timingCircleDefaultColor.b,
                a: 0);
            var coloringTween = timingDragCircleSpriteRenderer
                .DOColor(endValue: timingCircleDefaultColor, duration: _hitTiming)
                .From(initColor);

            return DOTween.Sequence()
                .Append(shrinkingTween)
                .Join(coloringTween);
        }

        private Tween TimingDragCircleTransformTween()
        {
            var scaleTween = timingDragCircleSpriteRenderer
                .transform.DOScale(endValue: Vector3.one * 2f, duration: 0.5f);
            var colorTween = timingDragCircleSpriteRenderer
                .DOColor(endValue: timingCircleDraggingColor, duration: 0.5f);
            return DOTween.Sequence()
                .Append(scaleTween)
                .Join(colorTween);
        }

        private Tween TimingDragCircleFade()
        {
            var initColor = timingDragCircleSpriteRenderer.color;
            var colorTween = timingDragCircleSpriteRenderer.DOColor(
                endValue: new Color(r: initColor.r, g: initColor.g, b: initColor.b, a: 0),
                duration: 0.5f);

            var scaleTween = timingDragCircleSpriteRenderer
                .transform.DOScale(endValue: Vector3.one * 3f, duration: 0.5f);

            return DOTween.Sequence()
                .Append(colorTween)
                .Join(scaleTween);
        }
    }
}
