using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
