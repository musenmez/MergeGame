using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public abstract class TileStateBase
    {
        public abstract void Enter();
        public virtual void Exit() { }
    }
}
