using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utils.ExtensionMethods;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FloatingMsg : APoolable
{
  [SerializeField] private CanvasGroup canvasGroup;
  [SerializeField] private Text TextField;
  [SerializeField] private float showTime;
  [SerializeField] private Canvas canvas;
  [SerializeField] private RectTransform myRect;
  [SerializeField] private GameObject msgLocation;
  private List<Tween> tweens;



  //---------------------------------------------------------------------------------------------------------------
  public void Show(String text, Color textColor, GameObject CustomParent = null)
  {

    tweens = new List<Tween>();
    transform.SetAsLastSibling();
    this.TextField.text = text;
    if (CustomParent == null)
    {
      this.myRect.SetParent(this.msgLocation.transform);
    }
    else
    {
      this.myRect.SetParent(CustomParent.transform);
    }


    this.myRect.localRotation = new Quaternion(0, 0, 0, 0);
    this.myRect.localPosition = Vector3.zero;
    ShowAnim();
  }

  //---------------------------------------------------------------------------------------------------------------
  private void ShowAnim()
  {

    this.myRect.ScaleTo(0.4f);
    canvasGroup.alpha = 1;
    Vector3 newMove = new Vector3(0, 140, 0);
    tweens.Add(canvasGroup.DOFade(1, 0.3f));
    tweens.Add(this.myRect.DOScale(1, 0.15f).SetEase(Ease.OutBack));
    tweens.Add(this.myRect.DOLocalMove(newMove, showTime * 0.9f).SetEase(Ease.OutBack));
    Game.TimerManager.Start(showTime * 0.5f, HideAnim);
  }

  //---------------------------------------------------------------------------------------------------------------
  private void HideAnim()
  {
    tweens.Add(canvasGroup.DOFade(0, showTime * 0.5f)
      .OnComplete(ReturnToPool));
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void OnPop()
  {
    if (gameObject != null)
      gameObject.SetActive(true);

    this.myRect.Rotate(Vector3.zero);
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void OnReturnedToPool()
  {
    if (tweens != null)
      tweens.ForEach(t => t.Kill());

    //this.myRect.SetParent(null);
  }
}


