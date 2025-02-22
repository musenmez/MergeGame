using Game.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.EventSystems;

namespace Game.Runtime
{
    public class Tile : MonoBehaviour, IPointerDownHandler, IDropHandler
    {
        public ItemBase PlacedItem { get; private set; } = null;
        public TileStateBase CurrentState { get; private set; }
        public TileStateId CurrentStateId { get; private set; }
        public Dictionary<TileStateId, TileStateBase> StatesById { get; private set; } = new();

        [field : SerializeField, OnValueChanged(nameof(Initialize))] public ItemDataSO InitialItemData { get; private set; }
        [field: SerializeField, OnValueChanged(nameof(Initialize))] public TileStateId InitialState { get; private set; } = TileStateId.Locked;
        [field: Header("Components"), SerializeField] public RectTransform ItemSocket { get; private set; }
        [field : SerializeField] public Image LockedVisual { get; private set; }
        [field : SerializeField] public Image RevealedVisual { get; private set; }

        public void Initialize()
        {
            if (InitialItemData != null)
                CreateItem(InitialItemData);

            SetStateCollection();
            SetState(InitialState);
        }

        public void SetState(TileStateId stateId)
        {
            if (!StatesById.ContainsKey(stateId))
            {
                Debug.LogError($"State Id not exist {stateId}");
                return;
            }

            CurrentState?.Exit();
            CurrentStateId = stateId;
            CurrentState = StatesById[stateId];
            CurrentState.Enter();
        }

        public void PlaceItem(ItemBase item) 
        {
            PlacedItem = item;
        }

        public void RemoveItem(ItemBase item) 
        {
            if (PlacedItem != item) 
                return;

            PlacedItem = null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Tile Pointer Down");
        }

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("Tile On Drop");
        }

        public void CreateItem(ItemDataSO itemData) 
        {
            if (!Application.isPlaying) return;

            ItemBase item = PoolingManager.Instance.GetInstance(itemData.PoolId, ItemSocket.position, Quaternion.identity).GetComponent<ItemBase>();
            item.transform.SetParent(ItemSocket);
            item.Initialize(this, itemData);
            PlacedItem = item;
        }   
       
        private void SetStateCollection() 
        {
            StatesById = new ()
            {
                { TileStateId.Locked, new TileLockedState(this) },
                { TileStateId.Revealed, new TileRevealedState(this) },
                { TileStateId.Free, new TileFreeState(this) }
            };
        }
    }
}
