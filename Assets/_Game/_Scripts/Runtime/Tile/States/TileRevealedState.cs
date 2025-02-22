using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class TileRevealedState : TileStateBase
    {
        public TileRevealedState(Tile tile) : base(tile) { }

        public override void Enter()
        {
            SetVisual();
        }

        private void SetVisual()
        {
            Tile.LockedVisual.enabled = false;
            Tile.RevealedVisual.enabled = true;
        }
    }
}
