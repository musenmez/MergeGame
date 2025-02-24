using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Game.Runtime
{
    public class FloatingCoin : MonoBehaviour
    {
        public int Reward { get; private set; }

        private const float COLLECT_ANIM_DURATION = 0.5f;

        private Sequence _collectSeq;

        public void Initialize(int reward) 
        {
            Reward = reward;
            CollectAnimation();
        }

        private void CollectAnimation() 
        {
            transform.SetParent(CurrencyPanel.Instance.CoinTarget);

            _collectSeq.Kill();
            _collectSeq = DOTween.Sequence();
            _collectSeq.AppendInterval(Random.Range(0f, 0.2f))
            .Append(transform.DOLocalMove(Vector3.zero, COLLECT_ANIM_DURATION).SetEase(Ease.InOutSine)).OnComplete(() =>
            {
                CurrencyManager.Instance.AddCurrency(Reward);
                CurrencyPanel.Instance.PunchCoinIcon();
                Dipose();
            });
        }

        private void Dipose() 
        {
            gameObject.SetActive(false);
        }
    }
}
