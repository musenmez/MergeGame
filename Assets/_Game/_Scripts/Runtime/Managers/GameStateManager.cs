using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace Game.Runtime
{
    public class GameStateManager : Singleton<GameStateManager>
    {
        public Dictionary<GameStateId, GameStateBase> StatesById { get; private set; } = new()
        {
            { GameStateId.Initial, new InitialState() },
            { GameStateId.InGame, new InGameState() }
        };
    }
}
