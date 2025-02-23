using Game.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    [CustomEditor(typeof(PanelBase), true)]
    public class PanelBaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PanelBase panelBase = (PanelBase)target;
            if (GUILayout.Button("Toggle Panel"))
            {
                TogglePanel(panelBase);
            }
        }

        private void TogglePanel(PanelBase panelBase)
        {
            if (panelBase.CanvasGroup.interactable)
                panelBase.SetCanvasGroup(false);
            else
                panelBase.SetCanvasGroup(true);
        }
    }
}
