using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCardsTutorial : TutorialPopup
{
  public sealed override void Activate(object data)
  {
    Game.ActionRoot.PlayerHandController.FillCardsForTutorial(6);
  }

}
