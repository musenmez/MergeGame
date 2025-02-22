using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [System.Serializable]
    public class Pool
    {
        public PoolableItem Prefab;
        public int InitialSize = 10;
    }
}
