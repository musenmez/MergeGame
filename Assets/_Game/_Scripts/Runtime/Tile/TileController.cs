using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class TileController : MonoBehaviour
    {
        public static TileController Instance = null;
        public Tile[,] Grid { get; private set; } = new Tile[COLUMN, ROW];
        [field: SerializeField] public List<Tile> Tiles { get; private set; } = new();

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
            int index = 0;
            for (int y = 0; y < ROW; y++)
            {
                for (int x = 0; x < COLUMN; x++)
                {
                    Grid[x, y] = Tiles[index];
                    Tiles[index].Initialize(new Vector2Int(x, y));
                    index++;
                }
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
                tile.RevelaTile();
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
    }
}
