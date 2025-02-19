using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Runtime
{
    public abstract class ItemBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] protected Transform body;
        [SerializeField] protected CanvasGroup canvasGroup;
        [SerializeField] protected GameObject outline;

        private Transform _defaultParent;

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            _defaultParent = transform.parent;
            transform.SetParent(transform.root);
            canvasGroup.blocksRaycasts = false;
            Debug.Log("Begin Drag");
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
            Debug.Log("On Drag");
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            transform.SetParent(_defaultParent);
            Debug.Log("End Drag");
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("On PointerDown");
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("On PointerUp");
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("On POinter Exit");
        }
    }
}
