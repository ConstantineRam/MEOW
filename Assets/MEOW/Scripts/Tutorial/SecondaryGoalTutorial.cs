using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryGoalTutorial : ActionTutorialPopUp
{
  public sealed override void Activate(object data)
  {
    Game.ActionRoot.TaskController.ShowSecondary();
  }
}
