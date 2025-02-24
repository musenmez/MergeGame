using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace Game.Runtime
{
    public class ItemMergeEffect : MonoBehaviour
    {
        public bool IsEnabled { get; private set; }

        [SerializeField] private ItemBase item;
        [SerializeField] private Transform body;
        [SerializeField] private CanvasGroup canvasGroup;

        private Tween _rotateTween;
        private Tween _scaleTween;
        private Tween _fadeTween;

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void Initialize() 
        {
            IsEnabled = false;
            body.gameObject.SetActive(false);
        }

        public void EnableEffect() 
        {
            if (IsEnabled)
                return;

            IsEnabled = true;

            body.localScale = Vector3.zero;
            canvasGroup.alpha = 0;

            ScaleTween(1, 0.5f);
            FadeTween(1, 0.5f);
            RotateTween(2f);
        }

        public void DisableEffect() 
        {
            if (!IsEnabled)
                return;

            IsEnabled = false;
            ScaleTween(0, 0.5f);
            FadeTween(0, 0.5f);
        }

        private void ScaleTween(float endValue, float duration, Action onComplete = null) 
        {
            _scaleTween.Kill();
            _scaleTween = body.DOScale(endValue, duration).SetEase(Ease.Linear).OnComplete(() => onComplete?.Invoke());
        }

        private void FadeTween(float endValue, float duration)
        {
            _fadeTween.Kill();
            _fadeTween = canvasGroup.DOFade(endValue, duration).SetEase(Ease.Linear);
        }

        private void RotateTween(float duration)
        {
            _rotateTween.Kill();
            _rotateTween = body.DOLocalRotate(Vector3.forward * 90f, duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        }

        private void Dispose() 
        {
            IsEnabled = false;
            body.gameObject.SetActive(false);
            _scaleTween.Kill();
            _rotateTween.Kill();
            _fadeTween.Kill();
        }
    }
}
