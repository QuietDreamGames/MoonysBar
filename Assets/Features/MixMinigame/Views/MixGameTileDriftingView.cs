using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Features.MixMinigame.Datas;
using Features.MixMinigame.ViewModels;
using UnityEngine;

namespace Features.MixMinigame.Views
{
    public class MixGameTileDriftingView : MixGameTileClickableView
    {
        private float _driftFinalPositionY;

        public override void Initialize(MixGameTileViewModel tileViewModel)
        {
            base.Initialize(tileViewModel);

            var driftFinalRelativePositionY =
                ((MixGameDriftingSequenceElementData)tileViewModel.TileModel.Data)
                .DriftFinalPositionY;

            var convertedPos =
                MixGamePlayingFieldService.ConvertRelativeToWorldPosition(new Vector2(0, driftFinalRelativePositionY));

            _driftFinalPositionY = convertedPos.y;

            _ = PlayAnimationAndWaitAsync("Drifting", 3);
        }

        protected override UniTask ResolveAnimation(string animationName, CancellationToken ct)
        {
            try
            {
                return base.ResolveAnimation(animationName, ct);
            }
            catch (ArgumentOutOfRangeException e)
            {
                if (animationName == "Drifting")
                    return MorphAnimationTweenToUniTask(DriftingTween(), ct);
                throw new ArgumentOutOfRangeException(nameof(animationName), animationName, null);
            }
        }

        private Tween DriftingTween()
        {
            return transform.DOLocalMoveY(_driftFinalPositionY, HitTiming)
                .SetEase(Ease.InQuad);
        }
    }
}
