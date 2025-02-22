using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "Generator Data", menuName = "Game/Data/Generator Data")]
    public class GeneratorDataSO : ItemDataSO
    {
        [Header("Generator")]
        public List<GeneratorProbabilityData> ItemProbabilities = new();
    }
}
