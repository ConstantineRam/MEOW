using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Zzz : MonoBehaviour
{
  [SerializeField]
  private Image[] zzz;
  private Tween[] zTween;
  [SerializeField]
  private Vector3 Scale;

  public void StartAnimation()


  {
    this.zTween = new Tween[this.zzz.Length];

    for (int i = 0; i < this.zzz.Length; i++)
    {
      zTween[i] =  zzz[i].rectTransform.DOShakeScale(3f, 0.3f, 1).SetLoops(-1);
      
    }
  }

  public void EndAnimation()
  {
    for (int i = 0; i < this.zzz.Length; i++)
    {
      zTween[i].Kill();

    }
  }
}
