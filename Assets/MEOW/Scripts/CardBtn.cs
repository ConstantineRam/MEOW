using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardBtn : APoolable
{
  [SerializeField]
  private Image CardImage;
  [SerializeField]
  private Text Points;
  [SerializeField]
  private Text BonusPoints;
  [SerializeField]
  private Text UnlockText;
  [SerializeField]
  private Image LockedImage;
  [SerializeField]
  private Sprite BonusPicture;

  private RectTransform rectTransform;
  public RectTransform RectTransform { get { return rectTransform; } }

  //---------------------------------------------------------------------------------------------------------------
   void Awake()
  {
    this.rectTransform = this.GetComponent<RectTransform>();
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void OnReturnedToPool()
  {
    
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void OnPop ()
  {

    base.OnPop();
    this.rectTransform = this.GetComponent<RectTransform>();
    this.gameObject.SetActive(true);
	}

  //---------------------------------------------------------------------------------------------------------------
  public void MakeBonusInc(int CatLevel)
  {
    this.CardImage.sprite = this.BonusPicture;
    this.Points.text = "";
    this.BonusPoints.text = "x" + Game.MenuRoot.UniqueKotanStorage.GetBonusPointsForLevel(CatLevel);

    if (Game.MenuRoot.UniqueKotanStorage.GetTopUnlockedKotanPosition() >= CatLevel)
    {
      this.LockedImage.gameObject.SetActive(false);
      this.UnlockText.text = "Unlocked by " + Game.MenuRoot.UniqueKotanStorage.GetNameNum(Game.MenuRoot.UniqueKotanStorage.GetNumByPos(CatLevel)) + ".";
    }
    else
    {
      this.LockedImage.gameObject.SetActive(true);
      this.UnlockText.text = "needs " + CatLevel.ToString() + " Magic Cats to unlock.";
    }
    }

  //---------------------------------------------------------------------------------------------------------------
  public void SetCard(ACAtCardData CatCard)
  {
    if (CatCard == null)
    {
      Debug.Log("got null at SetCard for card btn");
      return;
    }

    // Material m = this.CardImage.material;
    //m.SetFloat("_EffectAmount", 1f);
    this.BonusPoints.text = "";
    this.CardImage.sprite = CatCard.GetGraphicData().FrontImage;
    this.Points.text = CatCard.Points.ToString();
    if (Game.MenuRoot.UniqueKotanStorage.GetTopUnlockedKotanPosition() >= CatCard.UnlocksAt)
    {
      this.LockedImage.gameObject.SetActive(false);

      if (CatCard.UnlocksAt == UniqueKotanStorage.NothingUnlocked)
      {
        this.UnlockText.text = "Unlocked in dreams.";
      }
      else
      {

        this.UnlockText.text = "Unlocked by " + Game.MenuRoot.UniqueKotanStorage.GetNameNum(Game.MenuRoot.UniqueKotanStorage.GetNumByPos(CatCard.UnlocksAt)) + ".";
      }

    }
    else
    {
      this.LockedImage.gameObject.SetActive(true);
      this.UnlockText.text = "needs "+ (CatCard.UnlocksAt).ToString() + " Magic Cats to unlock.";
    }
  }
}
