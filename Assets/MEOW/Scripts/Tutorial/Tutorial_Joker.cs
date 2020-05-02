using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tutorial_Joker : ActionTutorialPopUp
{
  [SerializeField]
  private RectTransform BonusCard;

  [SerializeField]
  private Image BonusFeature;

  [SerializeField]
  private Text BonusText0;
  [SerializeField]
  private Text BonusText1;
  [SerializeField]
  private Text BonusText2;

  [SerializeField]
  private CanvasGroup PlusOneText0;
  [SerializeField]
  private CanvasGroup PlusOneText1;
  [SerializeField]
  private CanvasGroup PlusOneText2;
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
    animation.Append(PlusOneText0.DOFade(1, 0.2f));
    animation.Append(BonusText0.DOText("1", 0.3f));
    animation.Append(PlusOneText0.DOFade(0, 0.2f));

    animation.Append(PlusOneText1.DOFade(1, 0.2f));
    animation.Append(BonusText1.DOText("1", 0.3f));
    animation.Append(PlusOneText1.DOFade(0, 0.2f));

    animation.Append(PlusOneText2.DOFade(1, 0.2f));
    animation.Append(BonusText2.DOText("1", 0.3f));
    animation.Append(PlusOneText2.DOFade(0, 0.2f));
  }
}
