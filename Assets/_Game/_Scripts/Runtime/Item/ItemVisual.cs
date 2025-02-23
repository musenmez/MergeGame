using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class ItemVisual : MonoBehaviour
    {
        private ItemBase _item;
        private ItemBase Item => _item == null ? _item = GetComponentInParent<ItemBase>() : _item;

        [SerializeField] private Image visual;

        private void OnEnable()
        {
            Item.OnInitialized.AddListener(SetVisual);
            Item.OnStatusChanged.AddListener(SetVisual);
        }

        private void OnDisable()
        {
            Item.OnInitialized.RemoveListener(SetVisual);
            Item.OnStatusChanged.RemoveListener(SetVisual);
        }

        private void SetVisual() 
        {
            Sprite sprite = Item.Status == ItemStatus.Unlocked ? Item.Data.ColoredVisual : Item.Data.GrayedVisual;
            visual.sprite = sprite;
        }
    }
}
