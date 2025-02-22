using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class TileController : MonoBehaviour
    {
        public static TileController Instance = null;
        [field: SerializeField] public List<Tile> Tiles { get; private set; } = new();

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

        private void Initialize() 
        {
            foreach (Tile tile in Tiles)
            {
                tile.Initialize();
            }
        }

    }
}
