using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class CustomerOrderElement : MonoBehaviour
    {
        public bool IsAvailable { get; private set; }
        public ProductDataSO ProductData { get; private set; }

        [field: SerializeField] public Transform ServeTargetPoint { get; private set; }
        [SerializeField] private Image icon;
        [SerializeField] private Transform checkMark;

        public void Initialize(ProductDataSO productData) 
        {
            ProductData = productData;
            icon.sprite = ProductData.ColoredVisual;
            transform.localScale = Vector3.one;
            UpdateStatus();
        }

        public void UpdateStatus() 
        {
            IsAvailable = ProductManager.Instance.ActiveProductsById[ProductData.ItemId].Count > 0;
            checkMark.gameObject.SetActive(IsAvailable);
        }
    }
}
