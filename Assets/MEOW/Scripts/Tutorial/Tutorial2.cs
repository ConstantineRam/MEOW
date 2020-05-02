
using UnityEngine;

public class Tutorial2 : TutorialPopup
{

  //---------------------------------------------------------------------------------------------------------------
  protected sealed override void onExitEvent()
  {
    Game.MenuRoot.OnTutorialPutCatBackToChair();
  }
}
