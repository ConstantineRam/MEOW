using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tutorial_SideBonus : ActionTutorialPopUp
{
  [SerializeField]
  private RectTransform BonusCard;
  [SerializeField]
  private RectTransform NeighborCardCard;

  [SerializeField]
  private Image BonusFeature;

  [SerializeField]
  private Text NeighborText;

  [SerializeField]
  private CanvasGroup PlusOneText;

  [SerializeField]
  private RectTransform CardTarget;

  private Sequence animation;



  //---------------------------------------------------------------------------------------------------------------
  public sealed override void OnClose()
  {
    base.OnClose();
  }

  //---------------------------------------------------------------------------------------------------------------
  public sealed override void Activate(object data)
  {
    animation = DOTween.Sequence();
    BonusFeature.DOFade(1, 2f);
    animation.Append(BonusFeature.rectTransform.DOScale(1, 0.8f));
    animation.Append(NeighborCardCard.DOLocalMove (CardTarget.localPosition, 0.4f));
    animation.Append(PlusOneText.DOFade(1, 0.3f));
    animation.Append(NeighborText.DOText("1", 0.5f));
    animation.Append(PlusOneText.DOFade(0, 0.2f));
  }

  }
