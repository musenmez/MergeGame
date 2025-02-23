using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class TileController : MonoBehaviour
    {
        public static TileController Instance = null;
        public Tile[,] Grid { get; private set; } = new Tile[COLUMN, ROW];
        public List<Tile> Tiles { get; private set; } = new();

        [field: SerializeField] private BoardSaveDataSO BoardSaveData;
        [SerializeField] private Transform tileContainer;

        private readonly List<Vector2Int> _neighbourCoordiantes = new()
        {
            new Vector2Int(-1,0),
            new Vector2Int(1,0),
            new Vector2Int(0,-1),
            new Vector2Int(0,1)
        };

        private const int ROW = 5;
        private const int COLUMN = 5;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            GameManager.Instance.OnLevelStarted.AddListener(Initialize);
        }

        private void OnDisable()
        {
            GameManager.Instance.OnLevelStarted.RemoveListener(Initialize);
        }

        private void Initialize()
        {
            GridSaveData gridSaveData = BoardSaveData.GridData;
            foreach (TileSaveData tileSaveData in gridSaveData.Tiles)
            {
                Tile tile = PoolingManager.Instance.GetInstance(PoolId.Tile, tileContainer.position, Quaternion.identity).GetPoolComponent<Tile>();
                tile.transform.SetParent(tileContainer);
                tile.transform.localScale = Vector3.one;
                tile.Initialize(tileSaveData);

                Tiles.Add(tile);
                Grid[tileSaveData.X, tileSaveData.Y] = tile;
            }
        }

        public void RevealNeighbours(Tile sourceTile)
        {
            List<Tile> neighbours = new();
            foreach (Vector2Int coordinate in _neighbourCoordiantes)
            {
                Tile tile = GetTile(coordinate + sourceTile.GridCoordinate);
                if (tile == null)
                    continue;

                neighbours.Add(tile);
            }

            foreach (Tile tile in neighbours)
            {
                tile.RevealTile();
            }
        }

        public Tile GetRandomEmptyTile() 
        {
            List<Tile> tiles = new(Tiles);
            tiles.Shuffle();

            Tile emptyTile = null;
            foreach (Tile tile in tiles)
            {
                if (tile.IsAvailable)
                {
                    emptyTile = tile;
                    break;
                }
            }
            return emptyTile;
        }        

        private Tile GetTile(Vector2Int coordinate) 
        {
            try
            {
                return Grid[coordinate.x, coordinate.y];
            }
            catch
            {
                return null;
            }
        }

        private void SaveBoard() 
        {
            GridSaveData gridSaveData = new();
            foreach (var tile in Tiles)
            {
                string itemId = tile.PlacedItem == null ? "" : tile.PlacedItem.Data.ItemId;
                TileSaveData saveData = new(tile.GridCoordinate.x, tile.GridCoordinate.y, tile.CurrentStateId, itemId);
                gridSaveData.Tiles.Add(saveData);
            }
            BoardSaveData.SaveData(gridSaveData);
        }

        void OnApplicationQuit()
        {
            SaveBoard();
        }
    }
}
