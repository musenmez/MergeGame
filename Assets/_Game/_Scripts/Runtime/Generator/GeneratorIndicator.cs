using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Game.Runtime
{
    public class GeneratorIndicator : MonoBehaviour
    {
        public bool IsEnabled { get; private set; }

        [SerializeField] private Transform body;
        [SerializeField] private Generator generator;

        private const float ANIMATION_DURATION = 1f;

        private Tween _breathTween;

        private void OnEnable()
        {
            generator.OnInitialized.AddListener(Initialize);
            generator.OnStatusChanged.AddListener(Initialize);
        }

        private void OnDisable()
        {
            generator.OnInitialized.RemoveListener(Initialize);
            generator.OnStatusChanged.RemoveListener(Initialize);
        }        

        private void Initialize() 
        {
            if (generator.IsGeneratorAvailable && generator.Status == ItemStatus.Unlocked) 
                EnableIndicator();
            else 
                DisableIndicator();
        }

        private void EnableIndicator() 
        {
            if (IsEnabled)
                return;

            IsEnabled = true;
            body.gameObject.SetActive(true);
            BreathAnimation();
        }

        private void DisableIndicator() 
        {
            IsEnabled = false;
            _breathTween.Kill();
            body.gameObject.SetActive(false);
        }

        private void BreathAnimation() 
        {
            body.localScale = Vector3.one;
            _breathTween.Kill();
            _breathTween = body.DOScale(Vector3.one * 1.1f, ANIMATION_DURATION).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
