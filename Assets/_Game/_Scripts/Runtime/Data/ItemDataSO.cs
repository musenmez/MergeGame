using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Game/Data/Item Data")]
    public class ItemDataSO : ScriptableObject
    {
        public string ItemId;
        public Sprite ColoredVisual;
        public Sprite GrayedVisual;
    }
}
