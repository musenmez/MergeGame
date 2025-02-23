using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Game.Runtime
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PanelBase : MonoBehaviour, IPanel
    {
        protected CanvasGroup _canvasGroup = null;
        public CanvasGroup CanvasGroup => _canvasGroup == null ? _canvasGroup = GetComponent<CanvasGroup>() : _canvasGroup;

        public bool IsOpened { get; protected set; }
        public PanelId PanelId { get => panelID; protected set => panelID = value; }
        public UnityEvent OnStartPanelOpening { get; } = new UnityEvent();
        public UnityEvent OnPanelOpened { get; } = new UnityEvent();
        public UnityEvent OnStartPanelClosing { get; } = new UnityEvent();
        public UnityEvent OnPanelClosed { get; } = new UnityEvent();

        [SerializeField] protected PanelId panelID;

        [Header("Show")]
        [SerializeField] protected float fadeInDuration = 0.2f;
        [SerializeField] protected float fadeInDelay;

        [Header("Hide")]
        [SerializeField] protected float fadeOutDuration = 0.2f;
        [SerializeField] protected float fadeOutDelay;

        protected const float MAX_ALPHA = 1f;
        protected const float MIN_ALPHA = 0f;

        protected Tween _alphaTween;

        protected virtual void OnEnable()
        {
            if (Managers.Instance == null) return;

            UIManager.Instance.AddPanel(this);
        }

        protected virtual void OnDisable()
        {
            if (Managers.Instance == null) return;

            UIManager.Instance.RemovePanel(this);
        }

        public virtual void ShowPanel()
        {
            if (IsOpened)
                return;

            IsOpened = true;
            OnStartPanelOpening.Invoke();
            FadeTween(MAX_ALPHA, fadeInDuration, fadeInDelay, onStart: () => SetCanvasGroup(true), onComplete: () => OnPanelOpened.Invoke());
        }

        public virtual void HidePanel()
        {
            IsOpened = false;
            OnStartPanelClosing.Invoke();
            FadeTween(MIN_ALPHA, fadeOutDuration, fadeOutDelay, onStart: () => SetCanvasGroup(false), onComplete: () => OnPanelClosed.Invoke());
        }

        public virtual void SetCanvasGroup(bool isEnabled)
        {
            CanvasGroup.alpha = isEnabled ? MAX_ALPHA : MIN_ALPHA;
            CanvasGroup.interactable = isEnabled;
            CanvasGroup.blocksRaycasts = isEnabled;
        }

        protected virtual void FadeTween(float endValue, float duration, float delay, Action onStart = null, Action onComplete = null)
        {
            _alphaTween?.Kill();
            _alphaTween = CanvasGroup.DOFade(endValue, duration).SetDelay(delay).SetEase(Ease.Linear).SetUpdate(true).OnStart(() => onStart?.Invoke()).OnComplete(() => onComplete?.Invoke());
        }
    }
}
