using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTutorialPopUp : TutorialPopup
{
  [SerializeField]
  private int GameStageToFill;


  //---------------------------------------------------------------------------------------------------------------
  public override void OnClose()
  {
    Game.ActionRoot.PlayerHandController.FillCardsForTutorial(GameStageToFill);
  }

  }
