using Assets.Scripts.Utils.ExtensionMethods;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SwitchSceneScreen : MonoBehaviour
{
  [SerializeField]
  private Image splashImage;

  private Tween anim;

  private void Awake()
  {
    splashImage.color = splashImage.color.WithAlpha(0);
  }

  public void FadeIn(Action callBack = null)
  {
    gameObject.SetActive(true);
    anim.Kill();
    anim = splashImage.DOFade(1, 0.03f)
      .OnComplete(() =>
      {
        if (callBack != null) callBack();
      });
  }

  public void FadeOut(Action callBack = null)
  {
    anim.Kill();
    anim = splashImage.DOFade(0, 0.2f)
      .SetDelay(0.1f)
      .OnComplete(() =>
      {
        if (callBack != null) callBack();
        gameObject.SetActive(false);
      });
  }
}