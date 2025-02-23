using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game.Runtime
{
    public class Customer : MonoBehaviour
    {
        public int Reward { get; private set; }
        public bool IsServeAvailable { get; private set; }
        public bool IsServingCompleted { get; private set; }
        public OrderData Data { get; private set; }
        public List<CustomerOrderElement> OrderElements { get; private set; } = new();

        [SerializeField] private Transform body;
        [SerializeField] private Transform character;
        [SerializeField] private Transform orderContainer;
        [SerializeField] private GameObject serveButton;
        [SerializeField] private TextMeshProUGUI rewardText;

        private void OnEnable()
        {
            ProductManager.Instance.OnActiveProductsChanged.AddListener(CheckOrder);
        }

        private void OnDisable()
        {
            ProductManager.Instance.OnActiveProductsChanged.RemoveListener(CheckOrder);
        }

        public void Initialize(OrderData orderData) 
        {
            Data = orderData;
            CreateOrder();
            CheckOrder();
            SetReward();
        }

        public void ServeOrder(Product targetProduct = null) 
        {
            if (!IsServeAvailable)
                return;

            IsServingCompleted = false; ;
            foreach (var element in OrderElements)
            {
                Product product;
                if (targetProduct != null && element.ProductData.ItemId == targetProduct.Data.ItemId)
                    product = targetProduct;
                else
                    product = ProductManager.Instance.GetFirstActiveProduct(element.ProductData.ItemId);

                product.Serve(this, element);
            }
        }

        public void CompleteServing() 
        {
            if (IsServingCompleted)
                return;

            IsServingCompleted = true;
            //TO DO: Give reward
        }

        private void CheckOrder() 
        {
            bool isCompleted = true;
            OrderElementsStatus();

            foreach (CustomerOrderElement element in OrderElements)
            {
                if (!element.IsAvailable)
                {
                    isCompleted = false;
                    break;
                }
            }

            if (isCompleted) ActivateServe();
            else DisableServe();
        }

        private void ActivateServe() 
        {
            if (IsServeAvailable)
                return;

            IsServeAvailable = true;
            serveButton.SetActive(true);
            transform.SetAsFirstSibling();
        }

        private void DisableServe() 
        {
            IsServeAvailable = false;
            serveButton.SetActive(false);
        }

        private void OrderElementsStatus() 
        {
            foreach (CustomerOrderElement element in OrderElements)
            {
                element.UpdateStatus();
            }
        }

        private void CreateOrder() 
        {
            OrderElements.Clear();
            for (int i = 0; i < Data.Products.Count; i++)
            {
                CustomerOrderElement orderElement = PoolingManager.Instance.GetInstance(PoolId.CustomerOrderElement, orderContainer.position, Quaternion.identity).GetPoolComponent<CustomerOrderElement>();
                orderElement.transform.SetParent(orderContainer);
                orderElement.Initialize(Data.Products[i]);
                OrderElements.Add(orderElement);
            }
        }

        private void SetReward() 
        {
            Reward = 0;
            foreach (var orderElement in OrderElements)
            {
                Reward += orderElement.ProductData.Price;
            }
            rewardText.SetText($"+{Reward}");
        }
    }
}
