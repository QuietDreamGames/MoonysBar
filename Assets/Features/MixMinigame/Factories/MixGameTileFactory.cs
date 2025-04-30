using System;
using Features.MixMinigame.Datas;
using Features.MixMinigame.Models;
using Features.MixMinigame.ViewModels;
using Features.MixMinigame.Views;
using Features.ObjectPools.BasePool;
using Features.ObjectPools.InjectedPool;
using UnityEngine;
using VContainer;

namespace Features.MixMinigame.Factories
{
    public class MixGameTileFactory : MonoBehaviour
    {
        [SerializeField] private GameObject clickablePrefab;
        [SerializeField] private GameObject movablePrefab;
        
        [Inject] private IObjectResolver _objectResolver;
        
        private GameObjectPool<MixGameTileView> _clickablePool;
        private GameObjectPool<MixGameTileView> _movablePool;
        
        private const float ForgivenessWindow = 0.5f;
        
        private void OnValidate()
        {
            if (clickablePrefab != null && clickablePrefab.GetComponent<MixGameTileClickableView>() == null)
            {
                Debug.LogError($"'{clickablePrefab.name}' is missing MixGameClickableView component.", this);
                clickablePrefab = null;
            }

            if (movablePrefab != null && movablePrefab.GetComponent<MixGameTileMovableView>() == null)
            {
                Debug.LogError($"'{movablePrefab.name}' is missing MixGameMovableView component.", this);
                movablePrefab = null;
            }
        }
        
        private void Awake()
        {
            _clickablePool = new InjectedGameObjectPool<MixGameTileView>(_objectResolver, transform);
            _movablePool   = new InjectedGameObjectPool<MixGameTileView>(_objectResolver, transform);
        }

        public (MixGameTileView, MixGameTileViewModel) GetTile(MixGameSequenceElementData data, Transform parent)
        {
            MixGameTileModel tileModel = data switch
            {
                MixGameClickableSequenceElementData clickableData => new MixGameTileClickableModel(clickableData, ForgivenessWindow),
                MixGameMovableSequenceElementData   movableData   => new MixGameTileMovableModel(movableData, ForgivenessWindow),
                _                                                 => throw new ArgumentOutOfRangeException(nameof(data), data, null)
            };
            
            MixGameTileViewModel tileViewModel = data switch
            {
                MixGameClickableSequenceElementData clickableData => new MixGameTileClickableViewModel(tileModel),
                MixGameMovableSequenceElementData   movableData   => new MixGameTileMovableViewModel(tileModel),
                _                                                 => throw new ArgumentOutOfRangeException(nameof(data), data, null)
            };
            

            var (pool, prefab) = data switch
            {
                MixGameClickableSequenceElementData clickableData => (_clickablePool, clickablePrefab),
                MixGameMovableSequenceElementData   movableData   => (_movablePool, movablePrefab),
                _                                                 => throw new ArgumentOutOfRangeException(nameof(data), data, null)
            };

            var tileView = pool.Spawn(prefab, parent);
            
            tileView.Initialize(tileViewModel);
            tileView.OnReturnToPool += () => pool.Despawn(prefab, tileView);

            return (tileView, tileViewModel);
        }
        
    }
}