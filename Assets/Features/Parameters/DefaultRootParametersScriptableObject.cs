using UnityEngine;

namespace Features.Parameters
{
    [CreateAssetMenu(fileName = "DefaultRootParameters", menuName = "DefaultRootParameters", order = 0)]
    public class DefaultRootParametersScriptableObject : ScriptableObject
    {
        [Header("Resolution Properties")] [SerializeField]
        private int screenReferenceWidth = 1920;

        [SerializeField] private int   screenReferenceHeight         = 1080;
        [SerializeField] private float mixGamePlayfieldWidth         = 0.8f;
        [SerializeField] private float mixGamePlayfieldHeight        = 0.8f;
        [SerializeField] private float mixGameCameraOrthographicSize = 5f;

        public ResolutionParameters ResolutionParameters => new(
            screenReferenceWidth,
            screenReferenceHeight,
            mixGamePlayfieldWidth,
            mixGamePlayfieldHeight,
            mixGameCameraOrthographicSize
        );
    }

    public struct ResolutionParameters
    {
        public int   ScreenReferenceWidth;
        public int   ScreenReferenceHeight;
        public float MixGamePlayfieldWidth;
        public float MixGamePlayfieldHeight;
        public float MixGameCameraOrthographicSize;

        public ResolutionParameters(int   screenReferenceWidth, int screenReferenceHeight, float mixGamePlayfieldWidth,
                                    float mixGamePlayfieldHeight, float mixGameCameraOrthographicSize)
        {
            ScreenReferenceWidth          = screenReferenceWidth;
            ScreenReferenceHeight         = screenReferenceHeight;
            MixGamePlayfieldWidth         = mixGamePlayfieldWidth;
            MixGamePlayfieldHeight        = mixGamePlayfieldHeight;
            MixGameCameraOrthographicSize = mixGameCameraOrthographicSize;
        }
    }
}
