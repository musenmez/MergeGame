using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class OrderManager : Singleton<OrderManager>
    {
        [SerializeField] private List<OrderProbabilityData> orderProbabilities = new();

        public OrderData GetOrder()
        {
            OrderProbabilityData probabilityData = GetProductType();
            List<ProductDataSO> products = new(ProductManager.Instance.GetDataCollection(probabilityData.ItemType));
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
    }
}
