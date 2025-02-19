using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Game.Runtime
{
    public class ItemOutline : MonoBehaviour, IBeginDragHandler, IPointerDownHandler, IPointerUpHandler, IDeselectHandler
    {
        public bool IsActive { get; private set; }

        [SerializeField] protected Transform body;

        private Tween _punchTween;

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            ActivateOutline();
            EventSystem.current.SetSelectedGameObject(gameObject, eventData);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!IsActive)
                return;

            PunchBody();
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            DisableOutline();
        }
        
        public void OnDeselect(BaseEventData eventData)
        {
            DisableOutline();
        }

        private void ActivateOutline() 
        {
            IsActive = true;
            body.gameObject.SetActive(true);
        }

        private void DisableOutline() 
        {
            IsActive = false;
            _punchTween.Complete();
            body.gameObject.SetActive(false);
        }

        private void PunchBody() 
        {
            _punchTween.Complete();
            _punchTween = body.DOPunchScale(Vector3.one * 0.2f, 0.2f, vibrato: 2).SetEase(Ease.OutBounce);
        }
    }
}
