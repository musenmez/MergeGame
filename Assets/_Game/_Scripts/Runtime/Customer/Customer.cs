using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using NaughtyAttributes;

namespace Game.Runtime
{
    public class Customer : MonoBehaviour
    {
        public int Reward { get; private set; }
        public bool IsServeAvailable { get; private set; }
        public bool IsServingStarted{ get; private set; }
        public bool IsServingCompleted { get; private set; }
        public OrderData Data { get; private set; }
        public List<CustomerOrderElement> OrderElements { get; private set; } = new();

        [SerializeField] private Transform body;
        [SerializeField] private Transform character;
        [SerializeField] private Transform orderContainer;
        [SerializeField] private GameObject serveButton;
        [SerializeField] private TextMeshProUGUI rewardText;

        private const float REWARD_OFFSET = 100f;

        private Tween _scaleTween;

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
            transform.localScale = Vector3.one;
            Data = orderData;
            CreateOrder();
            CheckOrder();
            SetReward();
        }

        public void ServeButton() 
        {
            ServeOrder();
        }

        public void ServeOrder(Product targetProduct = null) 
        {
            if (!IsServeAvailable || IsServingStarted)
                return;

            IsServingStarted = true;
            IsServingCompleted = false;

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

            IsServingStarted = false;
            IsServingCompleted = true;
            GiveReward();

            ScaleTween(0f, 0.25f, 0.2f, onComplete:() =>
            {
                Dispose();
            });
        }

        private void CheckOrder() 
        {
            if (IsServingStarted)
                return;

            bool isCompleted = true;
            UpdateOrderElementsStatus();

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

            CustomerController.Instance.OnCustomersStatusUpdated.Invoke();
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

        private void UpdateOrderElementsStatus() 
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

        [Button]
        private void GiveReward() 
        {
            for (int i = 0; i < Reward; i++)
            {
                Vector2 offset = UnityEngine.Random.insideUnitCircle * REWARD_OFFSET;
                Vector3 spawnPosition = new Vector3(offset.x, offset.y, 0f) + character.position;

                FloatingCoin floatingCoin = PoolingManager.Instance.GetInstance(PoolId.FloatingCoin, spawnPosition, Quaternion.identity).GetPoolComponent<FloatingCoin>(); ;
                floatingCoin.Initialize(1);
            }
        }


        private void ScaleTween(float endValue, float duration, float delay = 0, Ease ease = Ease.Linear, Action onComplete = null) 
        {
            _scaleTween.Kill();
            _scaleTween = transform.DOScale(endValue, duration).SetDelay(delay).SetEase(ease).OnComplete(() => onComplete?.Invoke());
        }

        private void Dispose() 
        {
            CustomerController.Instance.RemoveCustomer(this);
            gameObject.SetActive(false);
        }
    }
}
