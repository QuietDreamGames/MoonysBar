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
        private const float HitTiming         = 1f;
        private const float ForgivenessWindow = 0.5f;


        [SerializeField] private GameObject clickablePrefab;
        [SerializeField] private GameObject movablePrefab;
        [SerializeField] private GameObject driftPrefab;

        private GameObjectPool<MixGameTileView> _clickablePool;
        private GameObjectPool<MixGameTileView> _driftPool;
        private GameObjectPool<MixGameTileView> _movablePool;

        [Inject] private IObjectResolver _objectResolver;

        private void Awake()
        {
            _clickablePool = new InjectedGameObjectPool<MixGameTileView>(_objectResolver, transform);
            _movablePool   = new InjectedGameObjectPool<MixGameTileView>(_objectResolver, transform);
            _driftPool     = new InjectedGameObjectPool<MixGameTileView>(_objectResolver, transform);
        }

        private void OnValidate()
        {
            if (!clickablePrefab && clickablePrefab.GetComponent<MixGameTileClickableView>())
            {
                Debug.LogError($"'{clickablePrefab.name}' is missing MixGameClickableView component.", this);
                clickablePrefab = null;
            }

            if (!movablePrefab && movablePrefab.GetComponent<MixGameTileMovableView>())
            {
                Debug.LogError($"'{movablePrefab.name}' is missing MixGameMovableView component.", this);
                movablePrefab = null;
            }

            if (!driftPrefab && driftPrefab.GetComponent<MixGameTileDriftingView>())
            {
                Debug.LogError($"'{driftPrefab.name}' is missing MixGameDriftingView component.", this);
                driftPrefab = null;
            }
        }

        public (MixGameTileModel, MixGameTileView, MixGameTileViewModel) GetTile(
            MixGameSequenceElementData data, Transform parent)
        {
            MixGameTileModel tileModel = data switch
            {
                MixGameDriftingSequenceElementData driftingData => new MixGameTileClickableModel(driftingData,
                    HitTiming,
                    ForgivenessWindow),
                MixGameClickableSequenceElementData clickableData => new MixGameTileClickableModel(clickableData,
                    HitTiming,
                    ForgivenessWindow),
                MixGameMovableSequenceElementData movableData => new MixGameTileMovableModel(movableData,
                    HitTiming,
                    ForgivenessWindow),
                _ => throw new ArgumentOutOfRangeException(nameof(data), data, null)
            };

            MixGameTileViewModel tileViewModel = data switch
            {
                MixGameDriftingSequenceElementData  => new MixGameTileClickableViewModel(tileModel),
                MixGameClickableSequenceElementData => new MixGameTileClickableViewModel(tileModel),
                MixGameMovableSequenceElementData   => new MixGameTileMovableViewModel(tileModel),
                _                                   => throw new ArgumentOutOfRangeException(nameof(data), data, null)
            };


            var (pool, prefab) = data switch
            {
                MixGameDriftingSequenceElementData  => (_driftPool, driftPrefab),
                MixGameClickableSequenceElementData => (_clickablePool, clickablePrefab),
                MixGameMovableSequenceElementData   => (_movablePool, movablePrefab),
                _                                   => throw new ArgumentOutOfRangeException(nameof(data), data, null)
            };

            var tileView = pool.Spawn(prefab, parent);

            tileView.Initialize(tileViewModel);
            tileView.OnReturnToPool += () => pool.Despawn(prefab, tileView);

            return (tileModel, tileView, tileViewModel);
        }
    }
}
