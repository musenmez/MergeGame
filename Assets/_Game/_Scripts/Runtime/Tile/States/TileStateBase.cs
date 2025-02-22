using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Runtime
{
    public abstract class TileStateBase
    {
        public Tile Tile { get; private set; }

        public TileStateBase(Tile tile) 
        {
            Tile = tile;
        }
        public abstract void Enter();
        public virtual void Exit() { }
        public virtual void Select() { }
        public virtual void Deselect() { }
        public virtual void OnItemBeginDrag() { }
        public virtual void OnItemEndDrag() { }
        public virtual void OnPointerDown(PointerEventData eventData) { }
        public virtual void OnPointerUp(PointerEventData eventData) { }
    }
}
