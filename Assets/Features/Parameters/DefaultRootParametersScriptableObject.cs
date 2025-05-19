using UnityEngine;

namespace Features.Parameters
{
    [CreateAssetMenu(fileName = "DefaultRootParameters", menuName = "DefaultRootParameters", order = 0)]
    public class DefaultRootParametersScriptableObject : ScriptableObject
    {
        [Header("Resolution Properties")] [SerializeField]
        private int screenReferenceWidth = 1920;

        [SerializeField] private int screenReferenceHeight = 1080;

        [Header("Mix Game Properties")] [SerializeField]
        private float mixGamePlayfieldWidth = 0.8f;

        [SerializeField] private float mixGamePlayfieldHeight        = 0.8f;
        [SerializeField] private float mixGameCameraOrthographicSize = 5f;

        [Header("Enchantment Properties")] [SerializeField]
        private float enchantmentPlayfieldWidth = 0.8f;

        [SerializeField] private float enchantmentPlayfieldHeight        = 0.8f;
        [SerializeField] private float enchantmentCameraOrthographicSize = 5f;


        public ResolutionParameters ResolutionParameters => new(
            screenReferenceWidth,
            screenReferenceHeight
        );

        public MiniGameScreenParameters MixGameScreenParameters => new(
            mixGamePlayfieldWidth,
            mixGamePlayfieldHeight,
            mixGameCameraOrthographicSize
        );

        public MiniGameScreenParameters EnchantmentScreenParameters => new(
            enchantmentPlayfieldWidth,
            enchantmentPlayfieldHeight,
            enchantmentCameraOrthographicSize
        );
    }

    public struct ResolutionParameters
    {
        public int ScreenReferenceWidth;
        public int ScreenReferenceHeight;

        public ResolutionParameters(int screenReferenceWidth, int screenReferenceHeight)
        {
            ScreenReferenceWidth  = screenReferenceWidth;
            ScreenReferenceHeight = screenReferenceHeight;
        }
    }

    public struct MiniGameScreenParameters
    {
        public float MiniGamePlayfieldWidth;
        public float MiniGamePlayfieldHeight;
        public float MiniGameCameraOrthographicSize;

        public MiniGameScreenParameters(float miniGamePlayfieldWidth, float miniGamePlayfieldHeight,
            float                             miniGameCameraOrthographicSize)
        {
            MiniGamePlayfieldWidth         = miniGamePlayfieldWidth;
            MiniGamePlayfieldHeight        = miniGamePlayfieldHeight;
            MiniGameCameraOrthographicSize = miniGameCameraOrthographicSize;
        }
    }
}
