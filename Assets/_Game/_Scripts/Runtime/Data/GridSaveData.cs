using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [System.Serializable]
    public class GridSaveData
    {
        public List<TileSaveData> Tiles = new();
    }
}
