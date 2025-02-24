using System.Collections;
using System.Collections.Generic;
using Game.Runtime;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Game.EditMode.Tests
{
    public class ItemDataTests
    {
        [Test]
        public void DuplicatedItemIdTest()
        {
            string[] guids = AssetDatabase.FindAssets("t:ItemDataSO");
            List<ItemDataSO> items = guids
                .Select(guid => AssetDatabase.LoadAssetAtPath<ItemDataSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(item => item != null)
                .ToList();

            Dictionary<string, List<ItemDataSO>> itemDataCollectionById = new();
            foreach (ItemDataSO item in items)
            {
                string id = item.ItemId;
                if (!itemDataCollectionById.ContainsKey(id))
                {
                    itemDataCollectionById[id] = new();
                }
                itemDataCollectionById[id].Add(item);
            }

            foreach (var pair in itemDataCollectionById)
            {
                Assert.IsTrue(pair.Value.Count == 1, $"Duplicate ItemId '{pair.Key}' found in {pair.Value.Count} assets.");
            }
        }
    }
}
