using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    [CreateAssetMenu(fileName = "Customer Save Data", menuName = "Game/Data/Customer Save Data")]
    public class CustomerSaveDataContainerSO : ScriptableObject
    {
        public CustomerSaveData CustomerData
        {
            get
            {
                return JsonUtility.FromJson<CustomerSaveData>(PlayerPrefs.GetString(PlayerPrefsKeys.CustomerSaveData, GetInitialSaveData()));
            }
            private set
            {
                PlayerPrefs.SetString(PlayerPrefsKeys.CustomerSaveData, JsonUtility.ToJson(value));
            }
        }

        public CustomerSaveData InitialCustomerData = new();
              

        public void SaveData(CustomerSaveData saveData)
        {
            CustomerData = saveData;
        }

        private string GetInitialSaveData()
        {
            return JsonUtility.ToJson(InitialCustomerData);
        }
    }
}
