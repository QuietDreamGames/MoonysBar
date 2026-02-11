using System;
using System.Collections.Generic;
using Features.MixMinigame.Models;
using Features.MixMinigame.Presenters;
using Features.MixMinigame.Views;
using Features.TimeSystem.Interfaces.Handlers;
using JetBrains.Annotations;
using VContainer;

namespace Features.MixMinigame
{
    public class MixGameTilesHolderAndUpdater : IUpdateHandler, IDisposable
    {
        private readonly List<(MixGameTileModel, MixGameTilePresenter, MixGameTileView)> _tiles;
        private readonly MixGameLevelTimerHolder                                         _timerHolder;

        [Inject]
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public MixGameTilesHolderAndUpdater(MixGameLevelTimerHolder timerHolder)
        {
            _timerHolder = timerHolder;

            _tiles = new List<(MixGameTileModel, MixGameTilePresenter, MixGameTileView)>();
        }

        public void Dispose()
        {
            for (var i = 0; i < _tiles.Count; i++)
            {
                if (_tiles[i].Item2 != null)
                    _tiles[i].Item2.ReturnToPool();
                _tiles[i].Item3.Dispose();
            }

            _tiles.Clear();
        }

        public void OnUpdate(float deltaTime)
        {
            for (var i = 0; i < _tiles.Count; i++)
            {
                var tile = _tiles[i];
                if (!tile.Item2.gameObject.activeInHierarchy) continue;
                tile.Item2.OnUpdate(deltaTime);
                if (_tiles.Contains(tile))
                    tile.Item3.CheckForMiss(_timerHolder.Timer);
            }
        }

        public void AddTile(MixGameTileModel model, MixGameTilePresenter presenter, MixGameTileView view)
        {
            _tiles.Add((model, presenter, view));
            presenter.OnReturnToPool += () => RemoveTileByPresenter(presenter);
        }

        public bool TryFindTileByPresenter(MixGameTilePresenter           presenter,
            out (MixGameTileModel, MixGameTilePresenter, MixGameTileView) result)
        {
            if (presenter == null)
            {
                result = default;
                return false;
            }

            for (var i = 0; i < _tiles.Count; i++)
                if (_tiles[i].Item2 == presenter)
                {
                    result = _tiles[i];
                    return true;
                }

            result = default;
            return false;
        }

        public void RemoveTileByModel(MixGameTileModel model)
        {
            for (var i = 0; i < _tiles.Count; i++)
                if (_tiles[i].Item1 == model)
                {
                    _tiles.RemoveAt(i);
                    break;
                }
        }

        public void RemoveTileByPresenter(MixGameTilePresenter presenter)
        {
            for (var i = 0; i < _tiles.Count; i++)
                if (_tiles[i].Item2 == presenter)
                {
                    _tiles.RemoveAt(i);
                    break;
                }
        }

        public void RemoveTileByView(MixGameTileView view)
        {
            for (var i = 0; i < _tiles.Count; i++)
                if (_tiles[i].Item3 == view)
                {
                    _tiles.RemoveAt(i);
                    break;
                }
        }
    }
}
