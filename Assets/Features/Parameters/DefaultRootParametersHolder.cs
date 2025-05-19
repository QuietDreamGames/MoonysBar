using UnityEngine;

namespace Features.Parameters
{
    public class DefaultRootParametersHolder : MonoBehaviour
    {
        [SerializeField] private DefaultRootParametersScriptableObject defaultRootParametersScriptableObject;

        public ResolutionParameters ResolutionParameters => defaultRootParametersScriptableObject.ResolutionParameters;

        public MiniGameScreenParameters MixGameScreenParameters =>
            defaultRootParametersScriptableObject.MixGameScreenParameters;

        public MiniGameScreenParameters EnchantmentParameters =>
            defaultRootParametersScriptableObject.EnchantmentScreenParameters;
    }
}
