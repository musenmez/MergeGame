using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class InitialState : GameStateBase
    {
        public override void Enter()
        {
            PoolingManager.Instance.Initialize();
            GameManager.Instance.SetState(GameStateId.InGame);
        }
    }
}
