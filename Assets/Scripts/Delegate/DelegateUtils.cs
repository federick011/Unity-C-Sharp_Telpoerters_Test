using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DelegateUtils
{
    public delegate void OnCreateGround();
    public static OnCreateGround OnCreateGroundScene;

    public delegate void OnShowVictory(bool statePanel);
    public static OnShowVictory OnShowVictoryPanel;
    public static OnShowVictory OnShowTeleportingPanel;
    public static OnShowVictory OnChangeProgressBarVBalue;
}
