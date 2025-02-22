using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Game/Data/Item Data")]
    public class ItemDataSO : ScriptableObject
    {
        public string ItemId => $"{Type}_{Version}_{Level}";
        public ItemType Type;
        public PoolId PoolId;
        
        [Space]
        public int Version;
        public int Level;
        
        [Space]
        public Sprite ColoredVisual;
        public Sprite GrayedVisual;
    }
}
