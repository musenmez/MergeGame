using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class TileFreeState : TileStateBase
    {
        public TileFreeState(Tile tile) : base(tile) { }

        public override void Enter()
        {
            SetVisual();
        }

        private void SetVisual()
        {
            Tile.LockedVisual.enabled = false;
            Tile.RevealedVisual.enabled = false;
            Tile.ProductIcon.enabled = true;
            Tile.ProductIcon.sprite = Tile.InitialProductData.ColoredVisual;
        }
    }
}
