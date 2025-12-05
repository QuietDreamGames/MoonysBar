using Features.Enchantment.Interfaces;
using UnityEngine;
using VContainer;

namespace Features.Enchantment
{
    public class EnchantmentEntryStarter : MonoBehaviour
    {
        [SerializeField] private EnchantmentNodesLayoutScriptableObject layoutScriptableObject;

        [Inject] private EnchantmentPathController                _enchantmentPathController;
        [Inject] private IEnchantmentForeshadowLineBuilderService _foreshadowLineBuilderService;
        [Inject] private IEnchantmentPlayingFieldService          _playingFieldService;

        private void Awake()
        {
            var layout = layoutScriptableObject.GetLayout();
            if (layout == null)
            {
                Debug.LogError("Layout is null.");
                return;
            }

            _foreshadowLineBuilderService.BuildForeshadowLine(layout);
            _enchantmentPathController.SetLayout(layout);
        }
    }
}
