using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Runtime
{
    public class Product : ItemBase
    {
        public ProductDataSO ProductData { get; private set; }

        public override void Initialize(Tile tile, ItemDataSO data)
        {
            ProductData = data as ProductDataSO;
            base.Initialize(tile, data);
        }
    }
}
