using System;
using System.Collections.Generic;
using Features.MixMinigame.Models;
using Features.MixMinigame.ViewModels;
using Features.MixMinigame.Views;
using Features.TimeSystem.Interfaces.Handlers;
using JetBrains.Annotations;
using VContainer;

namespace Features.MixMinigame
{
    public class MixGameTilesHolderAndUpdater : IUpdateHandler, IDisposable
    {
        private readonly List<(MixGameTileModel, MixGameTileView, MixGameTileViewModel)> _tiles;
        private readonly MixGameLevelTimerHolder                                         _timerHolder;

        [Inject]
        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
        public MixGameTilesHolderAndUpdater(MixGameLevelTimerHolder timerHolder)
        {
            _timerHolder = timerHolder;

            _tiles = new List<(MixGameTileModel, MixGameTileView, MixGameTileViewModel)>();
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
                var tileView = _tiles[i].Item2;
                if (!tileView.gameObject.activeInHierarchy) continue;
                tileView.OnUpdate(deltaTime);
                _tiles[i].Item3.CheckForMiss(_timerHolder.Timer);
            }
        }

        public void AddTile(MixGameTileModel model, MixGameTileView view, MixGameTileViewModel viewModel)
        {
            _tiles.Add((model, view, viewModel));
        }

        public List<(MixGameTileModel, MixGameTileView, MixGameTileViewModel)> GetTiles()
        {
            return _tiles;
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

        public void RemoveTileByView(MixGameTileView view)
        {
            for (var i = 0; i < _tiles.Count; i++)
                if (_tiles[i].Item2 == view)
                {
                    _tiles.RemoveAt(i);
                    break;
                }
        }

        public void RemoveTileByViewModel(MixGameTileViewModel viewModel)
        {
            for (var i = 0; i < _tiles.Count; i++)
                if (_tiles[i].Item3 == viewModel)
                {
                    _tiles.RemoveAt(i);
                    break;
                }
        }
    }
}