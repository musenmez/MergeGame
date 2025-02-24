using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System;

namespace Game.Runtime
{
    public abstract class ItemBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDeselectHandler, IPointerClickHandler
    {
        protected GraphicRaycaster _raycaster;
        protected GraphicRaycaster GraphicRaycaster => _raycaster == null ? _raycaster = GetComponentInParent<GraphicRaycaster>() : _raycaster;
        public bool IsActive { get; protected set; }
        public bool IsInteracted { get; protected set; }
        public ItemStatus Status { get; protected set; }
        public Tile CurrentTile { get; protected set; }
        public ItemDataSO Data { get; protected set; }
        public UnityEvent OnInitialized { get; } = new();
        public UnityEvent OnStatusChanged { get; } = new();
        public UnityEvent OnInteractionStarted { get; } = new();
        public UnityEvent OnInteractionStopped { get; } = new();

        [SerializeField] protected Transform body;
        [SerializeField] protected Transform punchBody;
        [SerializeField] protected CanvasGroup canvasGroup;

        protected ItemBase _lastInteractTarget;
        protected Tween _movementTween;
        protected Tween _punchTween;
        protected Sequence _jumpSeq;

        public virtual void Initialize(Tile tile, ItemDataSO data, bool punchItem = true) 
        {
            IsActive = true;
            CurrentTile = tile;
            Data = data;

            _lastInteractTarget = null;
            transform.localScale = Vector3.one;
            body.localScale = Vector3.one;
            
            SetStatus();
            if(punchItem) Punch();
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
            CurrentTile.OnItemBeginDrag();
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
            CheckInteractions(eventData);
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            EndInteractions();
            Drop(eventData);
            if(IsActive) CurrentTile.OnItemEndDrag();
        }

        public virtual void OnDeselect(BaseEventData eventData)
        {
            CurrentTile.Deselect();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Punch();
        }

        public virtual void UpdateStatus() 
        {
            SetStatus();
            OnStatusChanged.Invoke();
        }   
        
        public virtual void Dispose() 
        {
            IsActive = false;
            transform.SetParent(null);
            gameObject.SetActive(false);
            EndInteractions();
            RemoveItem();
        }

        protected virtual void SetStatus()
        {
            if (CurrentTile == null)
                return;

            switch (CurrentTile.CurrentStateId)
            {
                case TileStateId.Locked:
                    Status = ItemStatus.Locked;
                    break;

                case TileStateId.Revealed:
                    Status = ItemStatus.Revealed;
                    break;

                case TileStateId.Unlocked:
                    Status = ItemStatus.Unlocked;
                    break;

                default:
                    break;
            }
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
                Merge(tile, nextItemData);
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
            RemoveItem();

            CurrentTile = tile;
            CurrentTile.PlaceItem(this);

            if (isJumpAnimEnabled) JumpTween(duration);
            MovementTween(CurrentTile.ItemSocket.position, duration, onComplete: ()=> 
            {
                transform.SetParent(CurrentTile.ItemSocket);
                canvasGroup.blocksRaycasts = true;
                Punch();
            });
        }

        protected virtual void RemoveItem() 
        {
            if (CurrentTile != null)
                CurrentTile.RemoveItem(this);
        }

        protected virtual void Merge(Tile tile, ItemDataSO nextItemData) 
        {
            tile.PlacedItem.Dispose();
            tile.CreateItem(nextItemData);
            tile.UnlockedTile();
            TileController.Instance.RevealNeighbours(tile);
            Dispose();
        }

        protected virtual void StartInteraction() 
        {
            if (IsInteracted)
                return;

            IsInteracted = true;
            OnInteractionStarted.Invoke();
        }

        protected virtual void StopInteraction() 
        {
            if (!IsInteracted)
                return;

            IsInteracted = false;
            OnInteractionStopped.Invoke();
        }

        protected virtual void CheckInteractions(PointerEventData eventData) 
        {
            Tile tile = GetTile(eventData);
            if (tile == null || CurrentTile == tile || !IsMergeAvailable(tile, out _))
            {
                EndInteractions();
                return;
            }

            if (_lastInteractTarget == tile.PlacedItem)
                return;

            EndInteractions();
            tile.PlacedItem.StartInteraction();
            _lastInteractTarget = tile.PlacedItem;
        }

        protected virtual void EndInteractions() 
        {
            if (_lastInteractTarget != null)
                _lastInteractTarget.StopInteraction();

            _lastInteractTarget = null;
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

        protected virtual void MovementTween(Vector3 targetPosition, float duration, Action onComplete = null) 
        {
            SetParentRoot();
            _movementTween.Kill();
            _movementTween = transform.DOMove(targetPosition, duration).SetEase(Ease.InOutSine).OnComplete(() => 
            {
                onComplete?.Invoke();
            });
        }

        protected virtual void JumpTween(float duration, float multiplier = 1.5f)
        {
            body.localScale = Vector3.zero;
            _jumpSeq.Kill();
            _jumpSeq = DOTween.Sequence();
            _jumpSeq.Append(body.DOScale(Vector3.one * multiplier, duration * 0.3f).SetEase(Ease.InSine))
            .Append(body.DOScale(Vector3.one, duration * 0.7f).SetEase(Ease.OutSine));
        }

        protected virtual void Punch()
        {
            _punchTween.Complete();
            _punchTween = punchBody.DOPunchScale(0.25f * Vector3.one, 0.5f, vibrato: 8, elasticity: 0.5f).SetEase(Ease.Linear);
        }

        protected virtual void SetParentRoot() 
        {
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            canvasGroup.blocksRaycasts = false;
        }

        protected virtual bool IsSwitchAvailable(Tile tile) => tile.CurrentStateId == TileStateId.Unlocked && tile.PlacedItem != null;
        protected virtual bool IsMergeAvailable(Tile tile, out ItemDataSO nextItemData)
        {
            nextItemData = ItemDataManager.Instance.GetMergeData(Data);
            return tile.CurrentStateId != TileStateId.Locked && tile.PlacedItem != null && tile.PlacedItem.Data.ItemId == Data.ItemId && nextItemData != null;
        }
    }
}
