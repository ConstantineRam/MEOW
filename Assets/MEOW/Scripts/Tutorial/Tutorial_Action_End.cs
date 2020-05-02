using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Action_End : TutorialPopup
{

  //---------------------------------------------------------------------------------------------------------------
  protected sealed override void onExitEvent()
  {
    Debug.Log("Tutorial ended. Game started.");
    Game.Settings.IsTutorialActive = false;
    Game.StateManager.SetState(GameState.Action, true, true);
  }
}
