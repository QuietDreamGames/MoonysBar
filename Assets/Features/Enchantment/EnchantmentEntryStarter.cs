using Features.Enchantment.Interfaces;
using Features.TimeSystem.Interfaces;
using Features.TimeSystem.Interfaces.Injected;
using UnityEngine;
using VContainer;

namespace Features.Enchantment
{
    public class EnchantmentEntryStarter : MonoBehaviour
    {
        [SerializeField] private EnchantmentNodesLayoutScriptableObject layoutScriptableObject;

        [Inject] private readonly IEnchantmentElementsFactory              _elementsFactory;
        [Inject] private readonly EnchantmentElementsHolderAndUpdater      _elementsHolder;
        [Inject] private readonly EnchantmentPathController                _enchantmentPathController;
        [Inject] private readonly IEnchantmentForeshadowLineBuilderService _foreshadowLineBuilderService;
        [Inject] private readonly IEnchantmentPlayingFieldService          _playingFieldService;

        [Inject] private readonly ITransientTimeCollector _timeCollector;
        [Inject] private readonly ITimeSystem             _timeSystem;

        private void Start()
        {
            _timeCollector.UpdateHandlers.Add(_elementsHolder);
            _timeCollector.UpdateHandlers.Add(_enchantmentPathController);
            _timeSystem.Subscribe(_timeCollector);

            var layout = layoutScriptableObject.GetLayout();
            if (layout == null)
            {
                Debug.LogError("Layout is null.");
                return;
            }

            _foreshadowLineBuilderService.BuildForeshadowLine(layout);

            for (var i = 0; i < layout.Nodes.Count; i++)
            {
                var nodeData = layout.Nodes[i];
                var node     = _elementsFactory.CreateEnchantmentNode(nodeData);
                _elementsHolder.AddEnchantmentNode(model: node.Item1, presenter: node.Item2, view: node.Item3);
                node.Item2.transform.position =
                    _playingFieldService.ConvertRelativeToWorldPosition(nodeData.Position);
            }

            var enchantmentHandle = _elementsFactory.CreateEnchantmentHandle();
            enchantmentHandle.Item2.Deactivate();
            _elementsHolder.SetEnchantmentHandle(presenter: enchantmentHandle.Item1, view: enchantmentHandle.Item2);

            _enchantmentPathController.SetLayout(layout);
        }
    }
}
