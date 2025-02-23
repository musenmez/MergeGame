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
            return null;
        }

        private ItemType GetOrderItemType()
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
                    return itemProbabilityData.ItemType;
                }
            }
            return orderProbabilities[0].ItemType;
        }
    }
}
