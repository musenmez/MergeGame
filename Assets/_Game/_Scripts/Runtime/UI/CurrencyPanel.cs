using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Runtime
{
    public class CurrencyPanel : PanelBase
    {
        public static CurrencyPanel Instance = null;
        private int CurrencyAmount => CurrencyManager.CurrenyAmount;

        [field: Header("Currency Panel"), SerializeField] public Transform CoinTarget { get; private set; }
        [SerializeField] private Transform punchBody;
        [SerializeField] private TextMeshProUGUI currencyText;

        private Tween _punchTween;

        private void Awake()
        {
            Instance = this;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (Managers.Instance == null) return;

            CurrencyManager.Instance.OnCurrencyAmountChanged.AddListener(UpdateCurrencyText);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (Managers.Instance == null) return;

            CurrencyManager.Instance.OnCurrencyAmountChanged.RemoveListener(UpdateCurrencyText);
        }
        public void PunchCoinIcon()
        {
            _punchTween.Complete();
            _punchTween = punchBody.DOPunchScale(Vector3.one * 0.2f, 0.2f, vibrato: 1).SetEase(Ease.Linear);
        }

        private void UpdateCurrencyText() 
        {
            currencyText.SetText(CurrencyAmount.ToString());
        }
    }
}
