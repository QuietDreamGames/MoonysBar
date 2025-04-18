using Features.GameStateMachine;
using Features.GameStateMachine.States;
using UnityEngine;
using VContainer;

namespace Features.Gameplay
{
    public class GameplayStarter : MonoBehaviour
    {
        [Inject] private GameplayStateMachine _gameStateMachine;

        private void Start()
        {
            _gameStateMachine.Enter<InitState>();
        }
    }
}