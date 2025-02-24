using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Runtime
{
    public class TileRevealedState : TileStateBase
    {
        public TileRevealedState(Tile tile) : base(tile) { }

        public override void Enter()
        {
            SetVisual();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            CreateLockedText();
        }

        private void CreateLockedText()
        {
            FloatingText floatingText = PoolingManager.Instance.GetInstance(PoolId.FloatingText, Vector3.up * 10f + Tile.transform.position, Quaternion.identity).GetPoolComponent<FloatingText>();
            floatingText.transform.SetParent(Tile.transform.root);
            floatingText.Initialize("Locked!");
        }

        private void SetVisual()
        {
            Tile.LockedVisual.enabled = false;
            Tile.RevealedVisual.enabled = true;
        }
    }
}
