using Features.Parameters;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Features.Enchantment
{
    public class EnchantmentPlayingFieldService
    {
        private readonly Camera _camera;

        private readonly ResolutionParameters     _resolutionDefaultParameters;
        private readonly MiniGameScreenParameters _screenDefaultParameters;

        private int _fieldHeight;
        private int _fieldWidth;

        private int _screenHeight;
        private int _screenWidth;

        [Inject]
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public EnchantmentPlayingFieldService(DefaultRootParametersHolder rootParametersHolder, Camera camera)
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
                                   _screenDefaultParameters.MiniGamePlayfieldWidth)
                : Mathf.FloorToInt(_resolutionDefaultParameters.ScreenReferenceHeight *
                                   _screenDefaultParameters.MiniGamePlayfieldHeight);

            // var defaultFieldHeight = defaultFieldWidth;

            _fieldWidth = _screenHeight >= _screenWidth
                ? Mathf.FloorToInt(_screenWidth * _screenDefaultParameters.MiniGamePlayfieldWidth)
                : Mathf.FloorToInt(_screenHeight * _screenDefaultParameters.MiniGamePlayfieldHeight);

            if (_fieldWidth != defaultFieldWidth)
                _camera.orthographicSize = _screenDefaultParameters.MiniGameCameraOrthographicSize *
                    _fieldWidth / defaultFieldWidth;

            _fieldHeight = _fieldWidth;
        }

        // public Vector2 ConvertRelativeTilePositionToAbsolute(Vector2 relativePosition)
        // {
        //     var fieldSize = GetFieldSize();
        //
        //     // Adjust relative position to screen coordinates
        //     var x = relativePosition.x * fieldSize.x + _screenWidth / 2f;
        //     var y = relativePosition.y * fieldSize.y + _screenHeight / 2f;
        //
        //     return _camera.ScreenToWorldPoint(new Vector3(x, y, _camera.nearClipPlane));
        // }

        public Vector2 ConvertRelativeToWorldPosition(Vector2 screenUnscaledPosition)
        {
            // Calculate the playfield size in world units
            var playfieldSizeInWorldUnits = _camera.orthographicSize * 2f; // Orthographic size covers half the height

            // Scale factor to convert from pixels to world units
            var scaleFactor = playfieldSizeInWorldUnits / _screenHeight;

            // Convert screen default position to world position
            var worldX = screenUnscaledPosition.x * scaleFactor;
            var worldY = screenUnscaledPosition.y * scaleFactor;

            // Return the world position
            return new Vector2(worldX, worldY);
        }
    }
}
