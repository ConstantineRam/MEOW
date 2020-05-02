using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameLostPopUp : GenericPopup
{
  private GameLostPopUpData data;

  [SerializeField]
  private Text LoseText1;

  [SerializeField]
  private Text LoseText2;

  public enum LostReason
  {
    WrongCards = 0,
    LowScore = 1,
    UsedAllCards = 2
  }

  private bool IgnoreEmergencyClose = false;
  public class GameLostPopUpData
  {
    public LostReason lostReason;
  }
  public override void Activate(object data)
  {
    if (data != null && data is GameLostPopUpData)
    {
      this.data = (GameLostPopUpData) data;
    }
    else
    {
      Debug.LogError("Game Lost Popup didn't get required data.");
    }


    if (this.data.lostReason == LostReason.LowScore)
    {
      this.LoseText1.text = "The cards score is below 1.";
      this.LoseText2.text = "You need to have score over zero to succeed.";
      return;
    }
    if (this.data.lostReason == LostReason.UsedAllCards)
    {
      this.LoseText1.text = "You used up all cards that needed for the magic.";
      this.LoseText2.text = "Be careful using multiple cards of one color.";
      return;
    }

      this.LoseText1.text = "The summonig cards miss the required one.";
    this.LoseText2.text = "Check Mr. Meow hint in the rightmost bubble. The top card is the most important.";
  }

  public void OnRestartClick()
  {
    IgnoreEmergencyClose = true;
    Game.StateManager.SetState(GameState.Action, true, true);
    Game.AdsManager.Show(() => { }, AdsType.VIDEO);
  }

    public void OnExitClick()
  {
    this.CloseSelf();
    Game.AdsManager.Show(() => { }, AdsType.VIDEO);
  }

  public override void CloseSelf()
  {
    if (!IgnoreEmergencyClose)
    {
      Game.StateManager.SetState(GameState.Menu, true, false);
    }
    
  }

}
