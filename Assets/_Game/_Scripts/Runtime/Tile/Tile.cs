using Game.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Game.Runtime
{
    public class Tile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool IsAvailable => CurrentStateId == TileStateId.Unlocked && PlacedItem == null;
        public ItemBase PlacedItem { get; private set; } = null;
        public TileStateBase CurrentState { get; private set; }
        public TileStateId CurrentStateId { get; private set; }
        public Vector2Int GridCoordinate { get; private set; }
        public Dictionary<TileStateId, TileStateBase> StatesById { get; private set; } = new();

        [field : SerializeField] public ItemDataSO InitialItemData { get; private set; }
        [field: SerializeField] public TileStateId InitialState { get; private set; } = TileStateId.Locked;
        [field: Header("Components"), SerializeField] public RectTransform ItemSocket { get; private set; }
        [field : SerializeField] public Image LockedVisual { get; private set; }
        [field : SerializeField] public Image RevealedVisual { get; private set; }
        [field : SerializeField] public TileIndicator Indicator { get; private set; }
        [field: SerializeField] public TileHighlight Highlight { get; private set; }

        public void Initialize(Vector2Int gridCoordinate)
        {
            if (InitialItemData != null)
                CreateItem(InitialItemData);

            GridCoordinate = gridCoordinate;
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

            if (PlacedItem != null)
            {
                PlacedItem.UpdateStatus();
            }
        }

        public void OnPointerDown(PointerEventData eventData) 
        {
            CurrentState?.OnPointerDown(eventData);
        }

        public void OnPointerUp(PointerEventData eventData) 
        {
            CurrentState?.OnPointerUp(eventData);
        }

        public void OnItemBeginDrag() 
        {
            CurrentState?.OnItemBeginDrag();
        }

        public void OnItemEndDrag() 
        {
            CurrentState?.OnItemEndDrag();
        }

        public void Select() 
        {
            CurrentState?.Select();
        }

        public void Deselect() 
        {
            CurrentState?.Deselect();
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
            Deselect();
        }        
        
        public void RevealTile() 
        {
            if (CurrentStateId != TileStateId.Locked)
                return;

            SetState(TileStateId.Revealed);
        }

        public void UnlockedTile() 
        {
            if (CurrentStateId == TileStateId.Unlocked)
                return;

            SetState(TileStateId.Unlocked);
        }

        public void CreateItem(ItemDataSO itemData) 
        {
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
                { TileStateId.Unlocked, new TileUnlockedState(this) }
            };
        }
    }
}
