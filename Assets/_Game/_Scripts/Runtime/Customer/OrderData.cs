using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [System.Serializable]
    public class OrderData
    {
        public List<ProductDataSO> Products = new();

        public OrderData(List<ProductDataSO> products) 
        {
            Products = new(products);
        }
    }
}
