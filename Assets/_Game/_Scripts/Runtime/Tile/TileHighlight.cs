using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class TileHighlight : MonoBehaviour
    {
        [SerializeField] private GameObject body;

        public void SetHighlight(bool isEnabled) 
        {
            body.SetActive(isEnabled);
        }
    }
}
