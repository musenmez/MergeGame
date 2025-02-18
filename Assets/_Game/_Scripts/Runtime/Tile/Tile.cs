using Game.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime
{
    public class Tile : MonoBehaviour
    {
        public TileStateBase CurrentState { get; private set; }
        public TileStateId CurrentStateId { get; private set; }
        public Dictionary<TileStateId, TileStateBase> StatesById { get; private set; } = new();

        [field : SerializeField] public ProductDataSO InitialProductData { get; private set; }
        [field: SerializeField] public TileStateId InitialState { get; private set; } = TileStateId.Locked;
        [field: Header("Components"), SerializeField] public Image LockedVisual { get; private set; }
        [field : SerializeField] public Image RevealedVisual { get; private set; }
        [field: SerializeField] public Image ProductIcon { get; private set; }

        public void SetState(TileStateId stateID)
        {
            if (!StatesById.ContainsKey(stateID))
            {
                Debug.LogError($"State ID not exist {stateID}");
                return;
            }

            CurrentState?.Exit();
            CurrentStateId = stateID;
            CurrentState = StatesById[stateID];
            CurrentState.Enter();
        }

        private void Initialize() 
        {
            SetStateCollection();
            SetState(InitialState);
        }

        private void SetStateCollection() 
        {
            StatesById = new ()
            {
                { TileStateId.Locked, new TileLockedState(this) },
                { TileStateId.Revealed, new TileRevealedState(this) },
                { TileStateId.Free, new TileFreeState(this) }
            };
        }

        private void OnValidate()
        {
            if (InitialProductData != null)
            {
                Initialize();
            }
        }
    }
}
