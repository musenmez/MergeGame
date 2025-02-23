using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

namespace Game.Runtime
{
    public class ProductManager : Singleton<ProductManager>
    {
        public Dictionary<ItemType, List<ProductDataSO>> ProductDatasByType { get; private set; } = new();
        public Dictionary<string, List<Product>> ActiveProductsById { get; private set; } = new();
        public UnityEvent OnActiveProductsChanged { get; } = new();
        
        [SerializeField] private List<ProductDataSO> products = new();

        public void Initialize() 
        {
            SetProductCollection();
            SetActiveProductsCollection();
        }

        public void AddProduct(Product product) 
        {
            string id = product.Data.ItemId;
            if (!ActiveProductsById.ContainsKey(id))
            {
                ActiveProductsById.Add(id, new());
            }

            if (ActiveProductsById[id].Contains(product))
                return;

            ActiveProductsById[id].Add(product);
            OnActiveProductsChanged.Invoke();
        }

        public void RemoveProduct(Product product) 
        {
            string id = product.Data.ItemId;
            if (!ActiveProductsById.ContainsKey(id))
            {
                ActiveProductsById.Add(id, new());
            }

            if (!ActiveProductsById[id].Contains(product))
                return;

            ActiveProductsById[id].Remove(product);
            OnActiveProductsChanged.Invoke();
        }

        public List<ProductDataSO> GetDataCollection(ItemType productType)
        {
            if (!ProductDatasByType.ContainsKey(productType))
            {
                Debug.LogError($"Type: {productType} does not exist!");
                return null;
            }
            return ProductDatasByType[productType];
        }

        public Product GetFirstActiveProduct(string productId) 
        {
            if (!ActiveProductsById.ContainsKey(productId))
            {
                Debug.LogError($"Id: {productId} does not exist!");
                return null;
            }
            return ActiveProductsById[productId][0];
        }

        private void SetActiveProductsCollection() 
        {
            ActiveProductsById.Clear();
            foreach (ProductDataSO productData in products)
            {
                if (!ActiveProductsById.ContainsKey(productData.ItemId))
                {
                    ActiveProductsById.Add(productData.ItemId, new());
                }
            }
        }

        private void SetProductCollection()
        {
            ProductDatasByType.Clear();
            foreach (ProductDataSO productData in products)
            {
                if (!ProductDatasByType.ContainsKey(productData.Type))
                {
                    ProductDatasByType.Add(productData.Type, new());
                }
                ProductDatasByType[productData.Type].Add(productData);
                ProductDatasByType[productData.Type] = ProductDatasByType[productData.Type].OrderBy(x => x.Level).ToList();
            }
        }
    }
}
