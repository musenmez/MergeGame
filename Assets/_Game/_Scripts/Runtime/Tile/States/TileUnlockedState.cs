using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Runtime
{
    public class TileUnlockedState : TileStateBase
    {
        public TileUnlockedState(Tile tile) : base(tile) { }

        public override void Enter()
        {
            SetVisual();
        }

        public override void Select()
        {
            base.Select();
            Tile.Indicator.Activate();
        }

        public override void Deselect()
        {
            base.Deselect();
            Tile.Indicator.Disable();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            Tile.Indicator.Punch();
        }

        public override void OnItemBeginDrag()
        {
            base.OnItemBeginDrag();
            Tile.Indicator.Disable();
        }

        public override void OnItemEndDrag()
        {
            base.OnItemEndDrag();
            Tile.Indicator.Activate();
            Tile.Indicator.Punch();
        }

        private void SetVisual()
        {
            Tile.LockedVisual.enabled = false;
            Tile.RevealedVisual.enabled = false;
        }
    }
}
