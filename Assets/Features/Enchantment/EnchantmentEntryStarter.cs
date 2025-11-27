using System;
using Features.CameraSystem;
using Features.Interaction;
using Features.Interaction.Enums;
using Features.Interaction.Interfaces;
using UnityEngine;
using VContainer;

namespace Features.Enchantment
{
    public class EnchantmentEntryStarter : MonoBehaviour
    {
        [SerializeField] private EnchantmentNodesLayoutScriptableObject layoutScriptableObject;

        // [Inject] private EnchantmentPathController _enchantmentPathController;
        // [Inject] private IEnchantmentForeshadowLineBuilderService _foreshadowLineBuilderService;
        // [Inject] private IEnchantmentPlayingFieldService _playingFieldService;


        // test

        [Inject] private CameraHolderService _cameraHolderService;
        private          IDisposable         _disposable;

        [Inject] private IInteractionEventBusFeed _feed;
        private          Action<InteractionEvent> _handler;

        private void Awake()
        {
            var layout = layoutScriptableObject.GetLayout();
            if (layout == null)
            {
                Debug.LogError("Layout is null.");
                return;
            }

            // _foreshadowLineBuilderService.BuildForeshadowLine(layout);
            // _enchantmentPathController.SetLayout(layout);

            _handler = ReactToClick;


            _disposable = _feed.Subscribe(
                kinds: InteractionKind.Click,
                phases: InteractionPhase.Action,
                supportsMultipleHits: false,
                handler: _handler
            );
        }

        private void OnDestroy()
        {
            _handler = null;
            _disposable?.Dispose();
        }

        private void ReactToClick(InteractionEvent interactionEvent)
        {
            Debug.Log("EnchantmentEntryStarter ReactToClick");
        }
    }
}
