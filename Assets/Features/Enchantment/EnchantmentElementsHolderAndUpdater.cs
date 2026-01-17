using System;
using System.Collections.Generic;
using Features.Enchantment.Models;
using Features.Enchantment.Presenters;
using Features.Enchantment.Views;
using Features.TimeSystem.Interfaces.Handlers;

namespace Features.Enchantment
{
    public class EnchantmentElementsHolderAndUpdater : IUpdateHandler, IDisposable
    {
        private readonly List<(EnchantmentNodeModel, EnchantmentNodePresenter, EnchantmentNodeView)> _enchantmentNodes =
            new();

        private (EnchantmentHandlePresenter, EnchantmentHandleView) _enchantmentHandle;

        public void Dispose()
        {
            for (var i = 0; i < _enchantmentNodes.Count; i++)
            {
                if (_enchantmentNodes[i].Item2 != null)
                    _enchantmentNodes[i].Item2.ReturnToPool();
                _enchantmentNodes[i].Item3.Dispose();
            }

            _enchantmentNodes.Clear();

            // _enchantmentHandle.Item1.ReturnToPool();
            _enchantmentHandle.Item2.Dispose();
        }

        public void OnUpdate(float deltaTime)
        {
            for (var i = 0; i < _enchantmentNodes.Count; i++)
            {
                var node = _enchantmentNodes[i];
                if (!node.Item2.gameObject.activeInHierarchy) continue;
                node.Item2.OnUpdate(deltaTime);
            }

            if (_enchantmentHandle.Item1.gameObject.activeInHierarchy)
                _enchantmentHandle.Item1.OnUpdate(deltaTime);
        }

        public void AddEnchantmentNode(
            EnchantmentNodeModel     model,
            EnchantmentNodePresenter presenter,
            EnchantmentNodeView      view
        )
        {
            _enchantmentNodes.Add((model, presenter, view));
            presenter.OnReturnToPool += () => RemoveEnchantmentNodeByPresenter(presenter);
        }

        public void SetEnchantmentHandle(
            EnchantmentHandlePresenter presenter,
            EnchantmentHandleView      view
        )
        {
            _enchantmentHandle = (presenter, view);
            // presenter.OnReturnToPool += RemoveEnchantmentHandle;
        }

        public bool TryFindEnchantmentNodeByPresenter(EnchantmentNodePresenter presenter,
                                                      out (EnchantmentNodeModel, EnchantmentNodePresenter,
                                                          EnchantmentNodeView) result)
        {
            if (presenter == null)
            {
                result = default;
                return false;
            }

            for (var i = 0; i < _enchantmentNodes.Count; i++)
                if (_enchantmentNodes[i].Item2 == presenter)
                {
                    result = _enchantmentNodes[i];
                    return true;
                }

            result = default;
            return false;
        }

        public bool TryFindEnchantmentNodeByIndex(int index,
                                                  out (EnchantmentNodeModel, EnchantmentNodePresenter,
                                                      EnchantmentNodeView) result)
        {
            for (var i = 0; i < _enchantmentNodes.Count; i++)
                if (_enchantmentNodes[i].Item1.Data.Index == index)
                {
                    result = _enchantmentNodes[i];
                    return true;
                }

            result = default;
            return false;
        }

        public bool TryGetEnchantmentHandle(out (EnchantmentHandlePresenter, EnchantmentHandleView) result)
        {
            if (_enchantmentHandle == default)
            {
                result = default;
                return false;
            }

            result = _enchantmentHandle;
            return true;
        }

        private void RemoveEnchantmentNodeByPresenter(EnchantmentNodePresenter presenter)
        {
            for (var i = 0; i < _enchantmentNodes.Count; i++)
                if (_enchantmentNodes[i].Item2 == presenter)
                {
                    _enchantmentNodes.RemoveAt(i);
                    break;
                }
        }

        // private void RemoveEnchantmentHandle()
        // {
        //     _enchantmentHandle = default;
        // }
    }
}
