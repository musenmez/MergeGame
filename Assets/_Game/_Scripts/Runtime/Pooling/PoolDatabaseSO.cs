using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "Pool Database", menuName = "Game/Data/Pool Database")]
    public class PoolDatabaseSO : ScriptableObject
    {
        [ReorderableList]
        public List<Pool> Pools = new();
    }
}
