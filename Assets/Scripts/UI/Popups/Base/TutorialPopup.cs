using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEditor;

public class TutorialPopup : GenericPopup
{
  [SerializeField]
  private TutorialId stage;

  [SerializeField]
  private TutorialOnExit OnExitAction;

  //---------------------------------------------------------------------------------------------------------------
  protected virtual void onExitEvent()
  {
  }

  //---------------------------------------------------------------------------------------------------------------
  public sealed override void CloseSelf()
  {

    if (this.OnExitAction == TutorialOnExit.callNextStage)
    {
      Game.TutorialManager.ShowStage(stage + 1);
      return;
    }
    
    base.CloseSelf();
    this.onExitEvent();
  }
}

//---------------------------------------------------------------------------------------------------------------
public enum TutorialOnExit
{
  doNothing = 0,
  callNextStage = 1,

}