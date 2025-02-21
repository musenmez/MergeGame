using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Game.Runtime
{
    public class TileOutline : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IDeselectHandler
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
            _punchTween = body.DOPunchScale(0.2f * Vector3.one , 0.2f, vibrato: 2).SetEase(Ease.Linear);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!IsActive) return;
            
            ActivateOutline();
        }
    }
}
