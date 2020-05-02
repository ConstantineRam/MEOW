using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KotanBtn : Kotan
{


  [SerializeField]
  private Image BackImage;
  public Sprite SetBackSprite { set { this.BackImage.sprite = value; } }

 // private RectTransform rectTransform;
 // public RectTransform RectTransform { get { return rectTransform; } }

  private bool locked = false;
  public bool Locked { get { return locked; } set { locked = value; } }

  private CustomKotamData customKotamData;
  public CustomKotamData CustomKotamData { set { customKotamData = value; } }

  private int uniqueKotanNum;
  public int UniqueKotanNum { get { return uniqueKotanNum; } set { uniqueKotanNum = value; } }

  private Text catName;
  public string CatName { get { return catName.text; } set { catName.text = value; } }
  //---------------------------------------------------------------------------------------------------------------
  protected sealed override void Awake()
  {
    base.Awake();

  //  this.rectTransform = this.GetComponent<RectTransform>();
    this.catName = this.GetComponentInChildren<Text>();
    this.catName.text = "";
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OnBtnClicked ()
  {
    if (this.Locked)
    {
      // 
      if (this.UniqueKotanNum == CatBook.STANDARD)
      {
        Game.UiManager.Open(PopupId.BuyGame);
      }
      return;
    }


    Game.AudioManager.PlaySound(AudioId.ClickSound);


    if (this.UniqueKotanNum != CatBook.STANDARD)
    {
      UniqueKotanPopUp.UniqueKotanPopupData uniqueKotanPopUp = new UniqueKotanPopUp.UniqueKotanPopupData(this.UniqueKotanNum);

      Game.UiManager.Open(PopupId.UniqueKotans, uniqueKotanPopUp, PopupId.CatBook);
      return;
    }

    // if its not a Magic Cat
    PopUpKotanCard.KotanPopupData data = new PopUpKotanCard.KotanPopupData(this.customKotamData);
    Game.UiManager.Open(PopupId.CatCard, data, PopupId.CatBook);
  }

  //---------------------------------------------------------------------------------------------------------------
  public sealed override void OnPop()
  {
    base.OnPop();
    this.rectTransform = this.GetComponent<RectTransform>();
  }

  //---------------------------------------------------------------------------------------------------------------
  public sealed override void OnReturnedToPool()
  {
    this.locked = false;
    this.TurnOff();

  }
}
