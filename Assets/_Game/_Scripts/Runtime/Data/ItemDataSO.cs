using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Game/Data/Item Data")]
    public class ItemDataSO : ScriptableObject
    {
        public string ItemId;
        public ItemType Type;
        public PoolId PoolId;
        public Sprite ColoredVisual;
        public Sprite GrayedVisual;
    }
}
