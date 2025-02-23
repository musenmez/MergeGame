using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Runtime
{
    public class Product : ItemBase
    {
        public ProductDataSO ProductData { get; private set; }
        public bool IsServeAvailable { get; private set; }
        public Customer CurrentCustomer { get; private set; }

        [SerializeField] private GameObject checkMark;

        private void OnEnable()
        {
            CustomerController.Instance.OnCustomersStatusUpdated.AddListener(CheckOrders);
        }

        private void OnDisable()
        {
            CustomerController.Instance.OnCustomersStatusUpdated.RemoveListener(CheckOrders);
        }

        public override void Initialize(Tile tile, ItemDataSO data)
        {
            ProductData = data as ProductDataSO;
            base.Initialize(tile, data);
        }

        public void Serve(Customer customer, CustomerOrderElement orderElement) 
        {
            ProductManager.Instance.RemoveProduct(this);
        }

        public override void PlaceItem(Tile tile, float duration = 0.1F, bool isJumpAnimEnabled = false)
        {
            if (CurrentTile != tile)
                CurrentTile.Highlight.SetHighlight(false);

            base.PlaceItem(tile, duration, isJumpAnimEnabled);
            CurrentTile.Highlight.SetHighlight(IsServeAvailable);
        }

        public override void Dispose()
        {
            ProductManager.Instance.RemoveProduct(this);
            base.Dispose();
        }

        public void CheckOrders() 
        {
            if (Status != ItemStatus.Unlocked)
                return;

            bool isInOrder = false;
            bool isServeAvailable = false;

            foreach (Customer customer in CustomerController.Instance.Customers)
            {
                foreach (CustomerOrderElement orderElement in customer.OrderElements)
                {
                    if (orderElement.ProductData.ItemId != Data.ItemId)
                        continue;

                    isInOrder = true;
                    CurrentCustomer = customer;

                    if (customer.IsServeAvailable)
                    {
                        isServeAvailable = true;
                        break;
                    }
                }

                if (isServeAvailable)
                    break;
            }

            IsServeAvailable = isServeAvailable;
            SetCheckMark(isInOrder);
            CurrentTile.Highlight.SetHighlight(IsServeAvailable);
        }

        protected override void SetStatus()
        {
            base.SetStatus();

            if (Status != ItemStatus.Unlocked)
                return;

            ProductManager.Instance.AddProduct(this);
        }

        private void SetCheckMark(bool isEnabled) 
        {
            checkMark.SetActive(isEnabled);
        }
    }
}
