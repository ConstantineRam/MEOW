using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SaveKotansPopUp : GenericPopup
{
  private const PopupId myPopupId = PopupId.SaveKotans;

  [SerializeField]
  Text SlotsLeft;

  [SerializeField]
  private SessionResultButton[] resultButtons;

  //---------------------------------------------------------------------------------------------------------------
  private void UpdateSlots()
  {
    if (Game.ActionRoot.CustomKotanStorage.MaxSlots - Game.ActionRoot.CustomKotanStorage.AmountOfKotans() < 0)
    {
      Debug.Log("Unexpected error. negative amount in MaxSlots - Amount of Cats.");
    }
    this.SlotsLeft.text = "Book "+ (Math.Max(0, Game.ActionRoot.CustomKotanStorage.MaxSlots - Game.ActionRoot.CustomKotanStorage.AmountOfKotans() )) + "/" + Game.ActionRoot.CustomKotanStorage.MaxSlots+ " free";
  }

  //---------------------------------------------------------------------------------------------------------------
  private void Start()
  {
    if (this.resultButtons.Length < Game.ActionRoot.SessionResult.MaxResults())
    {
      Debug.LogError("Save cats PopUp has less amount of slots than game allows to get.");
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void Activate(object data)
  {
    List<CustomKotamData> UnsavedKotans = Game.ActionRoot.SessionResult.GetUnsavedCats();

    if (UnsavedKotans.Count < 1)
    {
      this.CloseSelf();
      return;
    }

    this.UpdateSlots();

    int i = -1;
    foreach (CustomKotamData cat in UnsavedKotans)
    {
      i++;
      this.resultButtons[i].InitAndOpen(cat, 0, myPopupId, true);
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public sealed override void CloseSelf()
  {
    Game.StateManager.SetState(GameState.Menu);

  }

  //---------------------------------------------------------------------------------------------------------------
  public void SaveKotans()
  {
    bool NotEnoughSpace = false;
    foreach (SessionResultButton catBtn in resultButtons)
    {
      CustomKotamData cat = catBtn.GetKotanData();
      if (!Game.ActionRoot.CustomKotanStorage.PushKotan(cat))
      {
        NotEnoughSpace = true;
      }
    }
    Game.ActionRoot.CustomKotanStorage.SaveArray();
    this.UpdateSlots();

    this.CloseSelf();

    if (NotEnoughSpace)
    {
      //buy the game lol.
    }

    //this
  }
} 


