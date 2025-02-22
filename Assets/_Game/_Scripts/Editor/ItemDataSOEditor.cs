using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Game.Runtime;

namespace Game.Editor
{
    [CustomEditor(typeof(ItemDataSO))]

    public class ItemDataSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            ItemDataSO itemData = (ItemDataSO)target;
            EditorGUILayout.LabelField("ItemId", itemData.ItemId);
            DrawDefaultInspector();
        }
    }
}
