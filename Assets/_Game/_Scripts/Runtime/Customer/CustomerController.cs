using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class CustomerController : MonoBehaviour
    {
        public static CustomerController Instance = null;
        public UnityEvent OnCustomersStatusUpdated { get; } = new();
        public List<Customer> Customers { get; private set; } = new();

        [SerializeField] private CustomerSaveDataContainerSO customerSaveDataContainer;
        [SerializeField] private Transform customerContainer;
        [SerializeField] private HorizontalLayoutGroup layoutGroup;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            GameManager.Instance.OnLevelStarted.AddListener(Initialize);
        }

        private void OnDisable()
        {
            GameManager.Instance.OnLevelStarted.RemoveListener(Initialize);
        }

        private void Initialize() 
        {
            Customers.Clear();
            for (int i = 0; i < customerSaveDataContainer.CustomerData.Customers.Count; i++)
            {
                OrderSaveData saveData = customerSaveDataContainer.CustomerData.Customers[i];
                OrderData orderData = new(GetProductsData(saveData));
                
                if (orderData.Products.Count == 0) CreateCustomer();
                else CreateCustomer(orderData);
            }
        }

        public void RemoveCustomer(Customer customer)
        {
            if (!Customers.Contains(customer))
                return;

            Customers.Remove(customer);
            CreateCustomer();
        }       

        private void CreateCustomer(OrderData orderData = null) 
        {
            if (orderData == null) orderData = OrderManager.Instance.GetOrder();
            Customer customer = PoolingManager.Instance.GetInstance(PoolId.Customer, customerContainer.position, Quaternion.identity).GetPoolComponent<Customer>();
            customer.transform.SetParent(customerContainer);
            customer.transform.SetAsLastSibling();
            customer.Initialize(orderData, layoutGroup);

            Customers.Add(customer);
            OnCustomersStatusUpdated.Invoke();
        }

        private List<ProductDataSO> GetProductsData(OrderSaveData saveData) 
        {
            List<ProductDataSO> products = new();
            foreach (var productIds in saveData.ProductIds)
            {
                ItemDataSO itemData = ItemDataManager.Instance.GetItemData(productIds);
                if(itemData != null) products.Add(itemData as ProductDataSO);
            }
            return products;
        }

        private OrderSaveData GetOrderSaveData(Customer customer) 
        {
            OrderSaveData orderSaveData = new();
            foreach (var productData in customer.Data.Products)
            {
                orderSaveData.ProductIds.Add(productData.ItemId);
            }
            return orderSaveData;
        }

        private void SaveCustomers()
        {
            CustomerSaveData customerSaveData = new();
            foreach (var customer in Customers)
            {
                if (customer.IsServingStarted)
                    continue;

                customerSaveData.Customers.Add(GetOrderSaveData(customer));
            }
            customerSaveDataContainer.SaveData(customerSaveData);
        }

        void OnApplicationQuit()
        {
            SaveCustomers();
        }
    }
}
