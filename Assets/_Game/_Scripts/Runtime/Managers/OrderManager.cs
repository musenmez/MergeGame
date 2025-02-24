using Codice.Client.BaseCommands.Merge.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class OrderManager : Singleton<OrderManager>
    {
        [SerializeField] private List<ProductDataSO> exceptions = new();
        [SerializeField] private List<OrderProbabilityData> orderProbabilities = new();

        private const int MIN_HARDNESS = 2;
        private const string HARDNESS_SUFFIX = "_HARDNESS";

        public OrderData GetOrder()
        {
            OrderProbabilityData probabilityData = GetProductType();
            List<ProductDataSO> products = new(ProductManager.Instance.GetDataCollection(probabilityData.ItemType));
            products = RemoveExceptions(products);
            products = CheckHardness(probabilityData.ItemType, products);
            products.Shuffle();
            
            int orderSize = Random.Range(1, probabilityData.MaxOrderSize + 1);
            orderSize = Mathf.Min(orderSize, products.Count);

            OrderData orderData = new(new());
            for (int i = 0; i < orderSize; i++)
            {
                orderData.Products.Add(products[0]);
                products.RemoveAt(0);
            }
            return orderData;
        }

        public void UpdateHardness(OrderData orderData) 
        {
            foreach (ProductDataSO productData in orderData.Products)
            {
                int hardness = GetHardness(productData.Type);
                if (productData.Level > hardness)
                {
                    hardness = productData.Level;
                }
                SetHardness(productData.Type, hardness);
            }
        }

        private List<ProductDataSO> CheckHardness(ItemType itemType, List<ProductDataSO> products) 
        {
            int maxLevel = GetHardness(itemType) + 2;
            List<ProductDataSO> filteredCollection = new(products);

            foreach (ProductDataSO product in products)
            {
                if (product.Level > maxLevel)
                {
                    filteredCollection.Remove(product);
                }
            }
            return filteredCollection;
        }

        private List<ProductDataSO> RemoveExceptions(List<ProductDataSO> products) 
        {
            foreach (var exception in exceptions)
            {
                if (products.Contains(exception))
                {
                    products.Remove(exception);
                }
            }
            return products;
        }

        private OrderProbabilityData GetProductType()
        {
            float totalProbability = 0f;
            foreach (var orderProbabilityData in orderProbabilities)
            {
                totalProbability += orderProbabilityData.Probability;
            }

            float random = Random.Range(0, totalProbability);
            float currentSum = 0f;

            foreach (var itemProbabilityData in orderProbabilities)
            {
                currentSum += itemProbabilityData.Probability;
                if (random <= currentSum)
                {
                    return itemProbabilityData;
                }
            }
            return orderProbabilities[0];
        }

        private int GetHardness(ItemType itemType) 
        {
            string key = GetHardnessKey(itemType);
            return PlayerPrefs.GetInt(key, MIN_HARDNESS);
        }

        private void SetHardness(ItemType itemType, int value) 
        {
            string key = GetHardnessKey(itemType);
            PlayerPrefs.SetInt(key, value);
        }

        private string GetHardnessKey(ItemType itemType) 
        {
            return $"{nameof(itemType)}{HARDNESS_SUFFIX}";
        }
    }
}
