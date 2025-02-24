using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Game.Runtime
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private Transform body;
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private CanvasGroup canvasGroup;

        private const float MOVEMENT_OFFSET = 25f;

        private Sequence _textSeq;

        public void Initialize(string text) 
        {
            transform.localScale = Vector3.one;
            canvasGroup.alpha = 1;
            textMesh.SetText(text);
            FloatingAnimation();
        }

        private void FloatingAnimation() 
        {
            _textSeq.Kill();
            _textSeq = DOTween.Sequence();
            _textSeq.Join(transform.DOMove(transform.position + Vector3.up * MOVEMENT_OFFSET, 0.25f)).SetEase(Ease.Linear)
            .Append(canvasGroup.DOFade(0, 0.25f).SetEase(Ease.Linear)).OnComplete(Dispose);
        }

        private void Dispose() 
        {
            gameObject.SetActive(false);
        }
    }
}
