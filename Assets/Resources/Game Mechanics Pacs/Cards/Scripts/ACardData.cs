using System.Collections;
using System;
using UnityEngine;

[Serializable]
public class ACardData
{
  [SerializeField]
  private String cardName;
  public String SetCardName { set { this.cardName = value; } }
  public String GetCardName { get { return this.cardName; } }

  [SerializeField]
  private CardTypes cardType;
  public CardTypes SetcCardType { set { this.cardType = value; } }

  [SerializeField]
  private CardGraphics cardGraphics;
  public CardGraphics SetCardGraphics { set { this.cardGraphics = value; } }




  //---------------------------------------------------------------------------------------------------------------
  public virtual CardGraphics GetGraphicData ()
  {
    return cardGraphics;

  }

  //---------------------------------------------------------------------------------------------------------------
  public CardTypes GetCardType()
  {
    return this.cardType;
  }
}


[Serializable]
public class CardGraphics
{
  public Sprite Cover;
  public Sprite Border;
  public Sprite FrontImage;
  public Sprite BackImage;

}
