using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Shaker : MonoBehaviour {

  [SerializeField]
  private Image[] images;
  private Tween[] zTween;
  [SerializeField]
  private float Strenght;
  [SerializeField]
  private float Time;

  public void StartAnimation()


  {
    this.zTween = new Tween[this.images.Length];

    for (int i = 0; i < this.images.Length; i++)
    {
      zTween[i] = images[i].rectTransform.DOShakeScale(Time, Strenght, 1, 90, false).SetLoops(-1);

    }
  }

  public void EndAnimation()
  {
    for (int i = 0; i < this.images.Length; i++)
    {
      zTween[i].Kill();

    }
  }
}
