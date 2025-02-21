using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game.Runtime
{
    public class ItemDataManager : Singleton<ItemDataManager>
    {
        public Dictionary<string, List<ItemDataSO>> ItemDataCollectionByVersion { get; private set; } = new();

        [SerializeField] private List<ItemDataSO> itemDataCollection = new();
          
        public void Initialize() 
        {
            SetItemDataCollection();
        }

        public ItemDataSO GetMergeData(ItemDataSO itemData) 
        {
            string key = GetItemDataCollectionKey(itemData);
            int targetIndex = itemData.Level;

            if (!ItemDataCollectionByVersion.ContainsKey(key) || ItemDataCollectionByVersion[key].Count == 0 || ItemDataCollectionByVersion[key].Count <= targetIndex)
                return null;

            return ItemDataCollectionByVersion[key][targetIndex];
        }

        private void SetItemDataCollection() 
        {
            foreach (ItemDataSO itemData in itemDataCollection)
            {
                string key = GetItemDataCollectionKey(itemData);
                if (!ItemDataCollectionByVersion.ContainsKey(key))
                {
                    ItemDataCollectionByVersion.Add(key, new());
                }

                ItemDataCollectionByVersion[key].Add(itemData);
                ItemDataCollectionByVersion[key] = ItemDataCollectionByVersion[key].OrderBy(x => x.Level).ToList();
            }
        }

        private string GetItemDataCollectionKey(ItemDataSO itemData) => $"{itemData.Type}_{itemData.Version}";
    }
}
