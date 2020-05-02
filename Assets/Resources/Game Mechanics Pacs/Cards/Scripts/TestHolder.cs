using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHolder : CardHolderController
{
  [SerializeField]
  private CardHolderController TrashPile;
  private bool allowUpdate = true;


  //---------------------------------------------------------------------------------------------------------------
  protected override void OnUpdate()
  {
    if (!this.allowUpdate)
    {
      return;
    }

    this.allowUpdate = false;
    Game.TimerManager.Start(1.8f, () =>
    {
      this.allowUpdate = true;
    });

    ACard checkCard = this.GetComponentInChildren<ACard>();
    if (checkCard == null)
    {
      return;
    }


    TrashPile.PushCard(checkCard);


  }
}
