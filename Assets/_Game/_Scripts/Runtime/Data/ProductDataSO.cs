using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "Product Data", menuName = "Game/Data/Product Data")]
    public class ProductDataSO : ItemDataSO
    {
        [Header("Product")]
        public int Price;
    }
}
