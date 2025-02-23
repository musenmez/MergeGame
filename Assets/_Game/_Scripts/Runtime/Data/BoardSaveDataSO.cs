using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "Board Save Data", menuName = "Game/Data/Board Save Data")]
    public class BoardSaveDataSO : ScriptableObject
    {
        public GridSaveData GridData
        {
            get
            {
                return JsonUtility.FromJson<GridSaveData>(PlayerPrefs.GetString(PlayerPrefsKeys.BoardSaveData, GetInitialSaveData()));
            }
            private set
            {
                PlayerPrefs.SetString(PlayerPrefsKeys.BoardSaveData, JsonUtility.ToJson(value));
            }
        }

        public GridSaveData InitialGrid = new();

        public const int ROW = 5;
        public const int COLUMN = 5;

        public void SaveData(GridSaveData gridSaveData) 
        {
            GridData = gridSaveData;
        }

        private string GetInitialSaveData() 
        {
            return JsonUtility.ToJson(InitialGrid);
        }

        private void Reset()
        {
            InitialGrid.Tiles.Clear();
            int index = 0;
            for (int y = 0; y < ROW; y++)
            {
                for (int x = 0; x < COLUMN; x++)
                {
                    TileSaveData saveData = new(x, y, TileStateId.Locked, "");
                    InitialGrid.Tiles.Add(saveData);
                    index++;
                }
            }
        }
    }
}
