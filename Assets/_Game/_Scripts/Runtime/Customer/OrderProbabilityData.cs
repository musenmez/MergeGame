using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [System.Serializable]
    public class OrderProbabilityData
    {
        public ItemType ItemType;
        [Range(0f,1f)]
        public float Probability;
        [Range(1, 3)]
        public int MaxOrderSize;
    }
}
