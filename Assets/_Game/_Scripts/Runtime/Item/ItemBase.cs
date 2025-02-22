using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace Game.Runtime
{
    public abstract class ItemBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private GraphicRaycaster _raycaster;
        private GraphicRaycaster GraphicRaycaster => _raycaster == null ? _raycaster = GetComponentInParent<GraphicRaycaster>() : _raycaster;
        public Tile CurrentTile { get; private set; }
        public ItemDataSO Data { get; private set; }

        [SerializeField] protected Transform body;
        [SerializeField] protected CanvasGroup canvasGroup;

        private Tween _movementTween;

        public virtual void Initialize(Tile tile, ItemDataSO data) 
        {
            CurrentTile = tile;
            Data = data;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            _movementTween.Kill();
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            canvasGroup.blocksRaycasts = false;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            Drop(eventData);
        }

        public void Dispose() 
        {
            transform.SetParent(null);
            gameObject.SetActive(false);
            CurrentTile.RemoveItem(this);
        }

        protected virtual void Drop(PointerEventData eventData) 
        {
            Tile tile = GetTile(eventData);
            if (tile == null || tile.CurrentStateId == TileStateId.Locked || tile == CurrentTile)
            {
                PlaceItem(CurrentTile);
            }
            else if (tile.CurrentStateId == TileStateId.Free && tile.PlacedItem == null)
            {
                PlaceItem(tile);
            }
            else if (IsMergeAvailable(tile, out ItemDataSO nextItemData))
            {
                tile.PlacedItem.Dispose();
                Dispose();
                tile.CreateItem(nextItemData);
            }
            else if (IsSwitchAvailable(tile)) 
            {
                tile.PlacedItem.PlaceItem(CurrentTile);
                PlaceItem(tile);
            }
            else 
            {
                PlaceItem(CurrentTile);
            }
        }        

        private void PlaceItem(Tile tile) 
        {
            if (CurrentTile != null)
                CurrentTile.RemoveItem(this);

            CurrentTile = tile;
            CurrentTile.PlaceItem(this);
            MovementTween(CurrentTile.ItemSocket.position, 0.1f);
        }

        private Tile GetTile(PointerEventData eventData) 
        {
            Tile tile = null;
            List<RaycastResult> results = new();
            GraphicRaycaster.Raycast(eventData, results);

            foreach (RaycastResult result in results)
            {
                tile = result.gameObject.GetComponentInParent<Tile>();
                if (tile != null) break;
            }
            return tile;
        }

        private void MovementTween(Vector3 targetPosition, float duration) 
        {
            _movementTween.Kill();
            _movementTween = transform.DOMove(targetPosition, duration).SetEase(Ease.Linear).OnComplete(() => 
            {
                transform.SetParent(CurrentTile.ItemSocket);
            });
        }

        private bool IsSwitchAvailable(Tile tile) => tile.CurrentStateId == TileStateId.Free && tile.PlacedItem != null;
        private bool IsMergeAvailable(Tile tile, out ItemDataSO nextItemData)
        {
            nextItemData = ItemDataManager.Instance.GetMergeData(Data);
            return tile.CurrentStateId != TileStateId.Locked && tile.PlacedItem != null && tile.PlacedItem.Data.ItemId == Data.ItemId && nextItemData != null;
        }
  }
}
