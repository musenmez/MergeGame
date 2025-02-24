using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Runtime
{
    public class CustomerGlowEffect : MonoBehaviour
    {
        public bool IsEnabled { get; private set; }

        [SerializeField] private Customer customer;
        [SerializeField] private Transform body;
        [SerializeField] private CanvasGroup canvasGroup;

        private Tween _rotateTween;
        private Tween _scaleTween;
        private Tween _fadeTween;

        private void OnEnable()
        {
            customer.OnServeActivated.AddListener(EnableEffect);
            customer.OnServeDisabled.AddListener(DisableEffect);
            Initialize();
        }

        private void OnDisable()
        {
            customer.OnServeActivated.RemoveListener(EnableEffect);
            customer.OnServeDisabled.RemoveListener(DisableEffect);
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
            canvasGroup.alpha = 0;
            SetBody();

            ScaleTween(1, 0.5f);
            FadeTween(1, 0.5f);
            RotateTween(2f);
        }

        public void DisableEffect()
        {
            if (!IsEnabled)
                return;

            IsEnabled = false;
            FadeTween(0, 0.25f);
            ScaleTween(0, 0.25f, () => Dispose());
        }

        private void SetBody()
        {
            body.gameObject.SetActive(true);
            body.localScale = Vector3.zero;
            body.localRotation = Quaternion.identity;
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
