namespace Features.MixMinigame.Views
{
    public class MixGameTileMovableView : MixGameTileView
    {
        private bool _isMoving;

        protected override void OnHit()
        {
            if (!_isMoving)
            {
                _isMoving = true;
                _         = PlayAnimationAndWaitAsync("Hit");
            }
            else
            {
                _ = PlayAnimationAndReturnToPoolAsync("HitReleased");
            }
        }

        protected override void OnMiss()
        {
            _ = PlayAnimationAndReturnToPoolAsync("Miss");
        }

        protected override void OnFail()
        {
            _ = PlayAnimationAndReturnToPoolAsync("Fail");
        }
    }
}
