using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;

public class BuyGamePopUp : GenericPopup
{

  //---------------------------------------------------------------------------------------------------------------
  private void GameWasBought()
  {
    Game.MenuRoot.BuyBtnText.CheckPremium();
    Game.UiManager.Open(PopupId.ThankYou);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OnBuyGameClicked()
  {
    InAppPurchasing.Purchase(EM_IAPConstants.Product_Buy_Game);
  }

  //---------------------------------------------------------------------------------------------------------------
  private void NotBoughtYet()
  {

  }

  //---------------------------------------------------------------------------------------------------------------
  public sealed override void Activate(object data)
  {
    if (Game.Settings.IsPremium)
    {
      this.GameWasBought();
    }
    else
    {
      this.NotBoughtYet();
    }

  }
}
