using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UnlockCardsAnimationObject : APoolable
{
  [SerializeField]
  private Image CardImage;
  [SerializeField]
  private Text Points;
  [SerializeField]
  private CanvasGroup MyCanvasGroup;
  [SerializeField]
  private RectTransform RectTransform;
  [SerializeField]
  private Sprite BonusPicture;

  [SerializeField]
  private Text BonusPoints;

  [HideInInspector]
  public RectTransform rectTransform;

  private Vector3 Punch;
  private Vector3 Scaling;
  void Awake()
  {
    this.Punch = new Vector3(0, 0, 40f);
    this.Scaling = new Vector3(2.5f, 2.5f, 0);
  }

  //---------------------------------------------------------------------------------------------------------------
  public sealed override void OnPop()
  {
    this.MyCanvasGroup.alpha = 1;
    this.gameObject.SetActive(true);
    this.rectTransform = this.GetComponent<RectTransform>();
    this.RectTransform.localScale = Vector3.one;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void MakeBonusInc(int CatLevel)
  {
    this.CardImage.sprite = this.BonusPicture;
    this.Points.text = "";
    this.BonusPoints.text = "x" +Game.MenuRoot.UniqueKotanStorage.GetBonusPointsForLevel(CatLevel);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void SetCard(ACAtCardData CatCard)
  {
    if (CatCard == null)
    {
      Debug.Log("got null at SetCard for card animation object.");
      return;
    }

    this.CardImage.sprite = CatCard.GetGraphicData().FrontImage;
    this.Points.text = CatCard.Points.ToString();
    this.BonusPoints.text = "";
  }

  //---------------------------------------------------------------------------------------------------------------
  public void Animate(float AnimationTime, float rotation)
  {
    float HalfTimer = AnimationTime / 2;
    Vector3 RotationAngle = new Vector3(0, 0, rotation);
    this.RectTransform.DORotate(RotationAngle, HalfTimer);
    //this.RectTransform.DOPunchRotation(this.Punch, HalfTimer);
    this.RectTransform.DOScale(this.Scaling, AnimationTime);
    Game.TimerManager.Start(HalfTimer, () =>
   {
     this.MyCanvasGroup.DOFade(0, HalfTimer);
   });

    Game.TimerManager.Start(AnimationTime + 0.3f, () =>
      {

        this.ReturnToPool();
      });
  }
  //---------------------------------------------------------------------------------------------------------------
  public sealed override void OnReturnedToPool()
  {
  
  }
}
