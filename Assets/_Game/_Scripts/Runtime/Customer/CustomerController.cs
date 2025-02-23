using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Runtime
{
    public class CustomerController : MonoBehaviour
    {
        public static CustomerController Instance = null;
        public UnityEvent OnCustomersStatusUpdated { get; } = new();
        public List<Customer> Customers { get; private set; } = new();
 
        [SerializeField] private Transform customerContainer;

        private const int INITIAL_CUSTOMER_AMOUNT = 2;

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
            for (int i = 0; i < INITIAL_CUSTOMER_AMOUNT; i++)
            {
                CreateCustomer();
            }
        }

        public void RemoveCustomer(Customer customer)
        {
            if (!Customers.Contains(customer))
                return;

            Customers.Remove(customer);
            CreateCustomer();
        }

        private void CreateCustomer() 
        {
            OrderData orderData = OrderManager.Instance.GetOrder();
            Customer customer = PoolingManager.Instance.GetInstance(PoolId.Customer, customerContainer.position, Quaternion.identity).GetPoolComponent<Customer>();
            customer.transform.SetParent(customerContainer);
            customer.transform.SetAsLastSibling();
            customer.Initialize(orderData);
            Customers.Add(customer);
            OnCustomersStatusUpdated.Invoke();
        }
    }
}
