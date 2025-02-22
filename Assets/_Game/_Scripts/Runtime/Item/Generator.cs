using Codice.CM.WorkspaceServer.Lock;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Runtime
{
    public class Generator : ItemBase, IPointerClickHandler
    {
        public bool IsGeneratorAvailable { get; private set; }
        public GeneratorDataSO GeneratorData { get; private set; }

        private const int GENERATOR_LEVEL_THRESHOLD = 5;

        public override void Initialize(Tile tile, ItemDataSO data)
        {
            base.Initialize(tile, data);
            GeneratorData = Data as GeneratorDataSO;
            IsGeneratorAvailable = Data.Level >= GENERATOR_LEVEL_THRESHOLD;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Generate();
        }

        private void Generate() 
        {
            if (!IsGeneratorAvailable)
            {
                Debug.LogError("Level Requirement is not set");
                return;
            }

            ItemDataSO itemData = GetItemData();
            if (itemData == null) return;

            Tile tile = TileController.Instance.GetRandomEmptyTile();
            if (tile == null)
            {
                //TO DO: Spawn Warning Text;
                Debug.LogError("Board is Full");
                return;
            }

            CreateItem(tile, itemData);
        }

        private void CreateItem(Tile tile, ItemDataSO itemData) 
        {
            ItemBase item = PoolingManager.Instance.GetInstance(itemData.PoolId, CurrentTile.ItemSocket.position, Quaternion.identity).GetComponent<ItemBase>();
            item.transform.SetParent(tile.ItemSocket);
            item.Initialize(tile, itemData);
            item.PlaceItem(tile, 0.4f, true);
        }

        private ItemDataSO GetItemData() 
        {
            float totalProbability = 0f;
            foreach (var itemProbabilityData in GeneratorData.ItemProbabilities)
            {
                totalProbability += itemProbabilityData.Probability;
            }

            float random = Random.Range(0, totalProbability);
            float currentSum = 0f;

            foreach (var itemProbabilityData in GeneratorData.ItemProbabilities)
            {
                currentSum += itemProbabilityData.Probability;
                if (random <= currentSum)
                {
                    return itemProbabilityData.ItemData;
                }
            }
            return null;
        }
    }
}
