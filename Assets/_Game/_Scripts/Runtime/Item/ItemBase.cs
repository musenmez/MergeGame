using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

namespace Game.Runtime
{
    public abstract class ItemBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDeselectHandler
    {
        protected GraphicRaycaster _raycaster;
        protected GraphicRaycaster GraphicRaycaster => _raycaster == null ? _raycaster = GetComponentInParent<GraphicRaycaster>() : _raycaster;
        public bool IsActive { get; private set; }
        public Tile CurrentTile { get; protected set; }
        public ItemDataSO Data { get; protected set; }
        public UnityEvent OnInitialized { get; } = new();
        public UnityEvent OnTileStateChanged { get; } = new();

        [SerializeField] protected Transform body;
        [SerializeField] protected CanvasGroup canvasGroup;

        protected Tween _movementTween;
        protected Sequence _jumpSeq;

        public virtual void Initialize(Tile tile, ItemDataSO data) 
        {
            IsActive = true;
            CurrentTile = tile;
            Data = data;
            OnInitialized.Invoke();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(gameObject, eventData);
            CurrentTile.OnPointerDown(eventData);
            CurrentTile.Select();
        }

        public virtual void OnPointerUp(PointerEventData eventData) 
        {
            CurrentTile.OnPointerUp(eventData);
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            _movementTween.Kill();
            SetParentRoot();
            canvasGroup.blocksRaycasts = false;
            CurrentTile.OnItemBeginDrag();
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            Drop(eventData);
            if(IsActive) CurrentTile.OnItemEndDrag();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            CurrentTile.Deselect();
        }

        public virtual void Dispose() 
        {
            IsActive = false;
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
            else if (tile.CurrentStateId == TileStateId.Unlocked && tile.PlacedItem == null)
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

        public virtual void PlaceItem(Tile tile, float duration = 0.1f, bool isJumpAnimEnabled = false) 
        {
            if (CurrentTile != null)
                CurrentTile.RemoveItem(this);

            CurrentTile = tile;
            CurrentTile.PlaceItem(this);

            MovementTween(CurrentTile.ItemSocket.position, duration);
            if (isJumpAnimEnabled) JumpTween(duration);
        }

        protected virtual Tile GetTile(PointerEventData eventData) 
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

        protected virtual void MovementTween(Vector3 targetPosition, float duration) 
        {
            SetParentRoot();
            _movementTween.Kill();
            _movementTween = transform.DOMove(targetPosition, duration).SetEase(Ease.InOutSine).OnComplete(() => 
            {
                transform.SetParent(CurrentTile.ItemSocket);
            });
        }

        protected virtual void JumpTween(float duration)
        {
            body.localScale = Vector3.zero;
            _jumpSeq.Kill();
            _jumpSeq = DOTween.Sequence();
            _jumpSeq.Append(body.DOScale(Vector3.one * 1.5f, duration * 0.3f).SetEase(Ease.InSine))
            .Append(body.DOScale(Vector3.one, duration * 0.7f).SetEase(Ease.OutSine));
        }

        protected virtual void SetParentRoot() 
        {
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
        }

        protected virtual bool IsSwitchAvailable(Tile tile) => tile.CurrentStateId == TileStateId.Unlocked && tile.PlacedItem != null;
        protected virtual bool IsMergeAvailable(Tile tile, out ItemDataSO nextItemData)
        {
            nextItemData = ItemDataManager.Instance.GetMergeData(Data);
            return tile.CurrentStateId != TileStateId.Locked && tile.PlacedItem != null && tile.PlacedItem.Data.ItemId == Data.ItemId && nextItemData != null;
        }
    }
}
