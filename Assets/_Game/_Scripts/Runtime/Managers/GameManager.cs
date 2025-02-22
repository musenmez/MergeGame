using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Runtime
{
    public class GameManager : Singleton<GameManager>
    {
        public GameStateBase CurrentState { get; private set; }
        public GameStateId CurrentStateId { get; private set; }
        public UnityEvent OnLevelStarted { get; } = new();

        public Dictionary<GameStateId, GameStateBase> StatesById { get; private set; } = new()
        {
            { GameStateId.Initial, new InitialState() },
            { GameStateId.InGame, new InGameState() }
        };

        private void Awake()
        {
            SetState(GameStateId.Initial);
        }

        public void SetState(GameStateId stateId)
        {
            if (!StatesById.ContainsKey(stateId))
            {
                Debug.LogError($"State Id not exist {stateId}");
                return;
            }

            CurrentState?.Exit();
            CurrentStateId = stateId;
            CurrentState = StatesById[stateId];
            CurrentState.Enter();
        }
    }
}
