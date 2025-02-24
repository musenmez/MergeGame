using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game.Runtime
{
    public class ItemDataManager : Singleton<ItemDataManager>
    {
        public Dictionary<ItemType, List<ItemDataSO>> ItemDataCollectionByType { get; private set; } = new();
        public Dictionary<string, ItemDataSO> ItemDataCollectionById { get; private set; } = new();

        [SerializeField] private List<ItemDataSO> itemDataCollection = new();
          
        public void Initialize() 
        {
            SetItemDataById();
            SetItemDataCollectionByType();
        }      

        public ItemDataSO GetItemData(string itemId) 
        {
            if (!ItemDataCollectionById.ContainsKey(itemId))
                return null;

            return ItemDataCollectionById[itemId];
        }

        public ItemDataSO GetMergeData(ItemDataSO itemData) 
        {
            int targetIndex = itemData.Level;

            if (!ItemDataCollectionByType.ContainsKey(itemData.Type) || ItemDataCollectionByType[itemData.Type].Count == 0 || ItemDataCollectionByType[itemData.Type].Count <= targetIndex)
                return null;

            return ItemDataCollectionByType[itemData.Type][targetIndex];
        }

        private void SetItemDataById()
        {
            ItemDataCollectionById.Clear();
            foreach (ItemDataSO itemData in itemDataCollection)
            {
                if (ItemDataCollectionById.ContainsKey(itemData.ItemId))
                    continue;

                ItemDataCollectionById.Add(itemData.ItemId, itemData);
            }
        }

        private void SetItemDataCollectionByType() 
        {
            ItemDataCollectionByType.Clear();
            foreach (ItemDataSO itemData in itemDataCollection)
            {
                if (!ItemDataCollectionByType.ContainsKey(itemData.Type))
                {
                    ItemDataCollectionByType.Add(itemData.Type, new());
                }

                ItemDataCollectionByType[itemData.Type].Add(itemData);
                ItemDataCollectionByType[itemData.Type] = ItemDataCollectionByType[itemData.Type].OrderBy(x => x.Level).ToList();
            }
        }
    }
}
