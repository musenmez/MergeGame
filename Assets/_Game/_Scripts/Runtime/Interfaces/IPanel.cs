using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public interface IPanel
    {
        PanelId PanelId { get; }
        bool IsOpened { get; }
        void ShowPanel();
        void HidePanel();
    }
}
