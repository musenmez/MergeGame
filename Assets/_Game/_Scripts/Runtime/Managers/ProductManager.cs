using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game.Runtime
{
    public class ProductManager : Singleton<ProductManager>
    {
        public Dictionary<ItemType, List<ProductDataSO>> ProductsByType { get; private set; } = new();

        [SerializeField] private List<ProductDataSO> products = new();

        public void Initialize() 
        {
            SetProductCollection();
        }

        public List<ProductDataSO> GetDataCollection(ItemType productType) 
        {
            if (!ProductsByType.ContainsKey(productType))
            {
                Debug.LogError($"Type: {productType} does not exist!");
                return null;
            }
            return ProductsByType[productType];
        }

        private void SetProductCollection()
        {
            foreach (ProductDataSO productData in products)
            {
                if (!ProductsByType.ContainsKey(productData.Type))
                {
                    ProductsByType.Add(productData.Type, new());
                }
                ProductsByType[productData.Type].Add(productData);
                ProductsByType[productData.Type] = ProductsByType[productData.Type].OrderBy(x => x.Level).ToList();
            }
        }
    }
}
