using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using System;

namespace Game.Runtime
{
    public class PoolableItem : MonoBehaviour
    {
        public bool IsActive { get; private set; }
        public Vector3 DefaultScale { get; protected set; }

        [field : SerializeField] public PoolId PoolId { get; protected set; } 
        [SerializeField] protected Component poolComponent;

        protected virtual void Awake()
        {
            DefaultScale = transform.localScale;
        }

        protected virtual void OnDisable()
        {
            if (PoolingManager.Instance == null) return;
            Dispose();
        }

        public virtual void Initialize()
        {
            IsActive = true;
            SetDefaults();
        }

        public virtual void SetPoolID(PoolId poolID)
        {
            PoolId = poolID;
        }

        public virtual void Dispose()
        {
            if (!IsActive)
                return;

            IsActive = false;
            gameObject.SetActive(false);
            SetDefaults();
            PoolingManager.Instance.PushItem(this);
        }

        public virtual T GetPoolComponent<T>()
        {
            if (poolComponent.GetType() != typeof(T) && !poolComponent.GetType().IsSubclassOf(typeof(T)))
            {
                Debug.LogError("Type Casting Error!");
                return default;
            }
            return (T)Convert.ChangeType(poolComponent, typeof(T));
        }

        protected virtual void SetDefaults()
        {
            transform.localScale = DefaultScale;
        }

        protected virtual void Reset()
        {
            poolComponent = transform;
        }
    }
}
