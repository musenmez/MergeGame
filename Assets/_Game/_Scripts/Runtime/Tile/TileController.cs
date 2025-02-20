using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class TileController : MonoBehaviour
    {
        [field: SerializeField] public List<Tile> Tiles { get; private set; } = new();

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
            foreach (Tile tile in Tiles)
            {
                tile.Initialize();
            }
        }
    }
}
