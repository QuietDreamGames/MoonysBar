using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Features.MixMinigame
{
    public class MixGamePlayingFieldService
    {
        private readonly Camera _camera;

        private int _screenWidth;
        private int _screenHeight;

        private int _fieldWidth;
        private int _fieldHeight;

        [Inject]
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public MixGamePlayingFieldService(Camera camera)
        {
            _camera = camera;

            (_screenWidth, _screenHeight) = (Screen.currentResolution.width, Screen.currentResolution.height);

            _fieldWidth = _screenHeight >= _screenWidth
                ? Mathf.FloorToInt(_screenWidth * 0.8f)
                : Mathf.FloorToInt(_screenHeight * 0.8f);

            _fieldHeight = _fieldWidth;
        }

        public Vector2 GetFieldSize()
        {
            return new Vector2(_fieldWidth, _fieldHeight);
        }

        public Vector2 ConvertRelativeTilePositionToAbsolute(Vector2 relativePosition)
        {
            var fieldSize = GetFieldSize();

            // Adjust relative position to screen coordinates
            var x = (relativePosition.x * fieldSize.x) + (_screenWidth / 2f);
            var y = (relativePosition.y * fieldSize.y) + (_screenHeight / 2f);

            return _camera.ScreenToWorldPoint(new Vector3(x, y, _camera.nearClipPlane));
        }
    }
}