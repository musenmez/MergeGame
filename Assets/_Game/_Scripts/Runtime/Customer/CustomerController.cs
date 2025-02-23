using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class CustomerController : MonoBehaviour
    {
        [SerializeField] private Transform customerContainer;

        private const int INITIAL_CUSTOMER_AMOUNT = 2;

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
            for (int i = 0; i < INITIAL_CUSTOMER_AMOUNT; i++)
            {
                CreateCustomer();
            }
        }

        private void CreateCustomer() 
        {
            OrderData orderData = OrderManager.Instance.GetOrder();
            Customer customer = PoolingManager.Instance.GetInstance(PoolId.Customer, customerContainer.position, Quaternion.identity).GetPoolComponent<Customer>();
            customer.transform.SetParent(customerContainer);
            customer.Initialize(orderData);
        }
    }
}
