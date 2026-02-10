using System;
using Features.MixMinigame.Datas;
using Features.MixMinigame.Models;
using Features.MixMinigame.Presenters;
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

        private GameObjectPool<MixGameTilePresenter> _clickablePool;
        private GameObjectPool<MixGameTilePresenter> _driftPool;
        private GameObjectPool<MixGameTilePresenter> _movablePool;

        [Inject] private IObjectResolver _objectResolver;

        private void Awake()
        {
            _clickablePool =
                new InjectedGameObjectPool<MixGameTilePresenter>(objectResolver: _objectResolver, root: transform);
            _movablePool =
                new InjectedGameObjectPool<MixGameTilePresenter>(objectResolver: _objectResolver, root: transform);
            _driftPool =
                new InjectedGameObjectPool<MixGameTilePresenter>(objectResolver: _objectResolver, root: transform);
        }

        private void OnValidate()
        {
            if (!clickablePrefab && clickablePrefab.GetComponent<MixGameTileClickablePresenter>())
            {
                Debug.LogError(message: $"'{clickablePrefab.name}' is missing MixGameClickableView component.",
                    context: this);
                clickablePrefab = null;
            }

            if (!movablePrefab && movablePrefab.GetComponent<MixGameTileMovablePresenter>())
            {
                Debug.LogError(message: $"'{movablePrefab.name}' is missing MixGameMovableView component.",
                    context: this);
                movablePrefab = null;
            }

            if (!driftPrefab && driftPrefab.GetComponent<MixGameTileDriftingPresenter>())
            {
                Debug.LogError(message: $"'{driftPrefab.name}' is missing MixGameDriftingView component.",
                    context: this);
                driftPrefab = null;
            }
        }

        public (MixGameTileModel, MixGameTilePresenter, MixGameTileView) GetTile(
            MixGameSequenceElementData data, Transform parent)
        {
            MixGameTileModel tileModel = data switch
            {
                MixGameDriftingSequenceElementData driftingData => new MixGameTileClickableModel(data: driftingData,
                    hitTiming: HitTiming,
                    forgivenessWindow: ForgivenessWindow),
                MixGameClickableSequenceElementData clickableData => new MixGameTileClickableModel(data: clickableData,
                    hitTiming: HitTiming,
                    forgivenessWindow: ForgivenessWindow),
                MixGameMovableSequenceElementData movableData => new MixGameTileMovableModel(data: movableData,
                    hitTiming: HitTiming,
                    forgivenessWindow: ForgivenessWindow),
                _ => throw new ArgumentOutOfRangeException(paramName: nameof(data), actualValue: data, message: null)
            };

            MixGameTileView tileView = data switch
            {
                MixGameDriftingSequenceElementData => new MixGameTileClickableView(tileModel),
                MixGameClickableSequenceElementData => new MixGameTileClickableView(tileModel),
                MixGameMovableSequenceElementData => new MixGameTileMovableView(tileModel),
                _ => throw new ArgumentOutOfRangeException(paramName: nameof(data), actualValue: data, message: null)
            };


            var (pool, prefab) = data switch
            {
                MixGameDriftingSequenceElementData => (_driftPool, driftPrefab),
                MixGameClickableSequenceElementData => (_clickablePool, clickablePrefab),
                MixGameMovableSequenceElementData => (_movablePool, movablePrefab),
                _ => throw new ArgumentOutOfRangeException(paramName: nameof(data), actualValue: data, message: null)
            };

            var tilePresenter = pool.Spawn(prefab: prefab, newParent: parent);

            tilePresenter.Initialize(tileView);
            tilePresenter.OnReturnToPool += () => pool.Despawn(prefab: prefab, component: tilePresenter);

            return (tileModel, tilePresenter, tileView);
        }
    }
}
