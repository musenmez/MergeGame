using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Game.Runtime
{
    public class TileIndicator : MonoBehaviour
    {
        public bool IsActive { get; private set; }

        [SerializeField] private Transform body;

        private Tween _punchTween;

        public void Activate() 
        {
            if (IsActive) return;

            IsActive = true;
            body.gameObject.SetActive(true);
        }

        public void Disable() 
        {
            IsActive = false;
            _punchTween.Complete();
            body.gameObject.SetActive(false);
        }

        public void Punch() 
        {
            if (!IsActive) return;

            _punchTween.Complete();
            _punchTween = body.DOPunchScale(0.25f * Vector3.one , 0.2f, vibrato: 1).SetEase(Ease.Linear);
        }
    }
}
