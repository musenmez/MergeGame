using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class PoolingManager : Singleton<PoolingManager>
    {
        public List<PoolableItem> PoolableItems { get; private set; } = new();
        public Dictionary<PoolId, Stack<PoolableItem>> PoolStacksById { get; private set; } = new();
        public Dictionary<PoolId, Pool> PoolsById { get; private set; } = new();    

        [SerializeField] private PoolDatabaseSO poolDatabase;

        private void Awake()
        {
            SetPoolCollection();
            SetInitialPoolStacks();
        }

        public PoolableItem GetInstance(PoolId poolId)
        {
            return GetInstance(poolId, Vector3.zero, Quaternion.identity);
        }

        public PoolableItem GetInstance(PoolId poolId, Vector3 position, Quaternion rotation)
        {
            if (!PoolStacksById.ContainsKey(poolId))
            {
                Debug.LogError($"Id: {poolId} does not exist!");
                return null;
            }

            PoolableItem poolableItem = PopItem(poolId);
            poolableItem.transform.position = position;
            poolableItem.transform.rotation = rotation;
            poolableItem.gameObject.SetActive(true);
            poolableItem.Initialize();
            return poolableItem;
        }

        public void PushItem(PoolableItem poolableItem)
        {
            if (!PoolStacksById.ContainsKey(poolableItem.PoolId) || PoolStacksById[poolableItem.PoolId].Contains(poolableItem))
                return;

            PoolStacksById[poolableItem.PoolId].Push(poolableItem);
        }

        public void DisposeAll()
        {
            for (int i = PoolableItems.Count - 1; i >= 0; i--)
            {
                if (PoolableItems[i] == null)
                {
                    PoolableItems.RemoveAt(i);
                    continue;
                }

                PoolableItems[i].transform.SetParent(null);
            }

            for (int i = 0; i < PoolableItems.Count; i++)
            {
                if (PoolableItems[i] == null)
                    continue;

                PoolableItems[i].Dispose();
            }
        }

        private PoolableItem PopItem(PoolId poolID)
        {
            int stackCount = PoolStacksById[poolID].Count;
            for (int i = 0; i < stackCount; i++)
            {
                PoolableItem poolableItem = PoolStacksById[poolID].Pop();
                if (poolableItem != null)
                    return poolableItem;
            }

            return CreatePoolableItem(poolID);
        }

        private PoolableItem CreatePoolableItem(PoolId poolID)
        {
            if (!PoolsById.ContainsKey(poolID))
                return null;

            PoolableItem poolableItem = Instantiate(PoolsById[poolID].Prefab).GetComponent<PoolableItem>();
            poolableItem.transform.SetParent(transform);
            poolableItem.gameObject.SetActive(false);
            poolableItem.SetPoolID(poolID);
            PoolableItems.Add(poolableItem);
            return poolableItem;
        }

        private void SetPoolCollection()
        {
            foreach (var pool in poolDatabase.Pools)
            {
                if (!PoolsById.ContainsKey(pool.Prefab.PoolId))
                {
                    PoolsById.Add(pool.Prefab.PoolId, pool);
                }
            }
        }

        private void SetInitialPoolStacks()
        {
            foreach (PoolId poolID in PoolsById.Keys)
            {
                if (PoolStacksById.ContainsKey(poolID))
                    continue;

                Stack<PoolableItem> poolStack = new();
                for (int i = 0; i < PoolsById[poolID].InitialSize; i++)
                {
                    PoolableItem poolableItem = CreatePoolableItem(PoolsById[poolID].Prefab.PoolId);
                    poolStack.Push(poolableItem);
                }

                PoolStacksById.Add(poolID, poolStack);
            }
        }
    }
}
