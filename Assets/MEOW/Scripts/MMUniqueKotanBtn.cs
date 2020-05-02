using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMUniqueKotanBtn : MonoBehaviour
{
  [SerializeField]
  int KotanCollectionNum;

  //---------------------------------------------------------------------------------------------------------------
  public void OnOpenKotan ()
  {


    int UnlockedNum = Game.MenuRoot.UniqueKotanStorage.GetPosByNum(KotanCollectionNum);

    if (UnlockedNum == UniqueKotanPopUp.UniqueKotanPopupData.NO_DATA_SET)
    {
      Debug.LogError("Unexpected Error. Locked Cat got a click from MM!");
      return;
    }
    Debug.Log(UnlockedNum);
    Game.AudioManager.PlaySound(AudioId.CatSound);
    UniqueKotanPopUp.UniqueKotanPopupData popUpData = new UniqueKotanPopUp.UniqueKotanPopupData(UnlockedNum);

    Game.UiManager.Open(PopupId.UniqueKotans, popUpData);
  }
}
