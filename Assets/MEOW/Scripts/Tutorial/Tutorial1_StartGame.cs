using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1_StartGame : TutorialPopup
{


  //---------------------------------------------------------------------------------------------------------------
  protected sealed override void onExitEvent()
  {
    Game.MenuRoot.OnTutorialEnableGameStart();
  }
}
