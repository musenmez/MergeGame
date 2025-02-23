using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class UIManager : Singleton<UIManager>
    {
        public Dictionary<PanelId, IPanel> PanelsById { get; private set; } = new Dictionary<PanelId, IPanel>();

        public void ShowPanel(PanelId panelID)
        {
            if (!PanelsById.ContainsKey(panelID))
                return;

            PanelsById[panelID].ShowPanel();
        }

        public void HidePanel(PanelId panelID)
        {
            if (!PanelsById.ContainsKey(panelID))
                return;

            PanelsById[panelID].HidePanel();
        }

        public void HideAllPanels()
        {
            foreach (var panel in PanelsById.Values)
            {
                panel.HidePanel();
            }
        }

        public void AddPanel(IPanel panel)
        {
            if (PanelsById.ContainsKey(panel.PanelId))
                return;

            PanelsById.Add(panel.PanelId, panel);
        }

        public void RemovePanel(IPanel panel)
        {
            if (!PanelsById.ContainsKey(panel.PanelId))
                return;

            PanelsById.Remove(panel.PanelId);
        }
    }
}
