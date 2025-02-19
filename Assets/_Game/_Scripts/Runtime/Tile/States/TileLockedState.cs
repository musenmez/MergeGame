using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class TileLockedState : TileStateBase
    {
        public TileLockedState(Tile tile) : base(tile) { }

        public override void Enter()
        {
            SetVisual();
        }

        private void SetVisual() 
        {
            Tile.LockedVisual.enabled = true;
            Tile.RevealedVisual.enabled = false;
        }
    }
}
