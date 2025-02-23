using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Runtime
{
    public class CurrencyManager : Singleton<CurrencyManager>
    {
        public static int CurrenyAmount
        {
            get
            {
                return PlayerPrefs.GetInt(PlayerPrefsKeys.CurrencyAmount, 0);
            }
            private set
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.CurrencyAmount, value);
            }
        }
        public UnityEvent OnCurrencyAmountChanged { get; } = new();
        public UnityEvent OnSuccessRewardClaimed { get; } = new();

        public void AddCurrency(int amount)
        {
            CurrenyAmount += amount;
            OnCurrencyAmountChanged.Invoke();
        }
    }
}
