using Features.Enchantment.Datas;
using Features.Enchantment.Interfaces;
using Features.Enchantment.Models;
using Features.Enchantment.Presenters;
using Features.Enchantment.Views;
using UnityEngine;

namespace Features.Enchantment.Factories
{
    public class EnchantmentElementsFactory : MonoBehaviour, IEnchantmentElementsFactory
    {
        [SerializeField] private EnchantmentNodePresenter   nodePresenterPrefab;
        [SerializeField] private EnchantmentHandlePresenter handlePresenterPrefab;

        public (EnchantmentNodeModel, EnchantmentNodePresenter, EnchantmentNodeView) CreateEnchantmentNode(
            EnchantmentNodeData data)
        {
            var model     = new EnchantmentNodeModel(data);
            var presenter = Instantiate(nodePresenterPrefab);
            var view      = new EnchantmentNodeView(model: model, presenter: presenter);

            presenter.Initialize(model);

            return (model, presenter, view);
        }

        public (EnchantmentHandlePresenter, EnchantmentHandleView) CreateEnchantmentHandle()
        {
            var presenter = Instantiate(handlePresenterPrefab);
            var view      = new EnchantmentHandleView(presenter);

            return (presenter, view);
        }
    }
}