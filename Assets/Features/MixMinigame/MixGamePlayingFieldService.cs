using Features.Parameters;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Features.MixMinigame
{
    public class MixGamePlayingFieldService
    {
        private readonly Camera _camera;

        private readonly ResolutionParameters _resolutionDefaultParameters;

        private int _fieldHeight;
        private int _fieldWidth;

        private int _screenHeight;
        private int _screenWidth;

        [Inject]
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public MixGamePlayingFieldService(DefaultRootParametersHolder rootParametersHolder, Camera camera)
        {
            _resolutionDefaultParameters = rootParametersHolder.ResolutionParameters;
            _camera                      = camera;

            SetScreenResolution(Screen.currentResolution.width, Screen.currentResolution.height);

            // todo: here must be an event listener on settings change.
            // IDisposable should be included, on dispose it should remove listener.
        }

        public Vector2 GetFieldSize()
        {
            return new Vector2(_fieldWidth, _fieldHeight);
        }

        public void SetScreenResolution(int width, int height)
        {
            _screenWidth  = width;
            _screenHeight = height;

            var defaultFieldWidth = _resolutionDefaultParameters.ScreenReferenceHeight >=
                                    _resolutionDefaultParameters.ScreenReferenceWidth
                ? Mathf.FloorToInt(_resolutionDefaultParameters.ScreenReferenceWidth *
                                   _resolutionDefaultParameters.MixGamePlayfieldWidth)
                : Mathf.FloorToInt(_resolutionDefaultParameters.ScreenReferenceHeight *
                                   _resolutionDefaultParameters.MixGamePlayfieldHeight);

            // var defaultFieldHeight = defaultFieldWidth;

            _fieldWidth = _screenHeight >= _screenWidth
                ? Mathf.FloorToInt(_screenWidth * _resolutionDefaultParameters.MixGamePlayfieldWidth)
                : Mathf.FloorToInt(_screenHeight * _resolutionDefaultParameters.MixGamePlayfieldHeight);

            _fieldHeight = _fieldWidth;

            if (_fieldWidth != defaultFieldWidth)
                _camera.orthographicSize = _resolutionDefaultParameters.MixGameCameraOrthographicSize *
                    _fieldWidth / defaultFieldWidth;
        }

        public Vector2 ConvertRelativeTilePositionToAbsolute(Vector2 relativePosition)
        {
            var fieldSize = GetFieldSize();

            // Adjust relative position to screen coordinates
            var x = relativePosition.x * fieldSize.x + _screenWidth / 2f;
            var y = relativePosition.y * fieldSize.y + _screenHeight / 2f;

            return _camera.ScreenToWorldPoint(new Vector3(x, y, _camera.nearClipPlane));
        }

        public Vector2 ConvertRelativeToWorldPosition(Vector2 relativePosition)
        {
            // Calculate the playfield size in world units
            float playfieldSizeInWorldUnits = Camera.main.orthographicSize * 2f; // Orthographic size covers half the height
            float playfieldSizeInPixels = Screen.height * 0.8f;

            // Scale factor to convert from pixels to world units
            float scaleFactor = playfieldSizeInWorldUnits / playfieldSizeInPixels;

            // Convert relative position to world position
            float worldX = relativePosition.x * scaleFactor;
            float worldY = relativePosition.y * scaleFactor;

            // Return the world position
            return new Vector2(worldX, worldY);
        }

    }
}
