using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [System.Serializable]
    public class CustomerSaveData 
    {
        public List<OrderSaveData> Customers = new();
    }
}
