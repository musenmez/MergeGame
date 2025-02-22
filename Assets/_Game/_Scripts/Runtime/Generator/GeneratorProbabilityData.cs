using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [System.Serializable]
    public class GeneratorProbabilityData
    {
        public ItemDataSO ItemData;
        [Range(0f, 1f)]
        public float Probability;
    }
}
