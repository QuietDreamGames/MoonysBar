using Features.Enchantment.Datas;
using UnityEngine;

namespace Features.Enchantment
{
    [CreateAssetMenu(fileName = "EnchantmentNodesLayout", menuName = "Enchantment", order = 0)]
    public class EnchantmentNodesLayoutScriptableObject : ScriptableObject
    {
        [SerializeField] private TextAsset layoutFile;

        public EnchantmentGraphData GetLayout()
        {
            if (layoutFile) return new EnchantmentGraphData(layoutFile.text);

            Debug.LogError("Layout file is not set.");
            return null;
        }
    }
}
