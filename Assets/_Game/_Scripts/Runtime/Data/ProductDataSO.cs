using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "ProductData", menuName = "Game/Data/Product Data")]
    public class ProductDataSO : ScriptableObject
    {
        public string ProductId;
        public Sprite ColoredVisual;
        public Sprite GrayedVisual;
    }
}
