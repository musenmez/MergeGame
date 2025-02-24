using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [System.Serializable]
    public class TileSaveData
    {
        public int X;
        public int Y;
        public TileStateId TileState = TileStateId.Locked;
        public string ItemId = "";

        public TileSaveData(int x, int y, TileStateId tileState, string itemId)
        {
            X = x;
            Y = y;
            TileState = tileState;
            ItemId = itemId;
        }
    }
}
