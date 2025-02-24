using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Runtime
{
    public class UIElementPunch : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Transform punchBody;

        private Tween _punchTween;
        private Vector3 _defaultScale;

        private void Awake()
        {
            _defaultScale = punchBody.localScale;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PunchScale();
        }

        private void PunchScale()
        {
            _punchTween.Complete();
            _punchTween = punchBody.DOPunchScale(_defaultScale * 0.2f, 0.25f, vibrato: 0).SetEase(Ease.Linear);
        }
    }
}
