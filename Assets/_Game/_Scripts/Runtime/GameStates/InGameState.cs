using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class InGameState : GameStateBase
    {
        public override void Enter()
        {
            UIManager.Instance.ShowPanel(PanelId.Currency);
            GameManager.Instance.OnLevelStarted.Invoke();
        }
    }
}
