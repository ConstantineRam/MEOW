using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EffectStarShine : MonoBehaviour
{
  [SerializeField]
  private Image myImage;

  private RectTransform MyRect;
  private Vector3 myScale;
  private Vector3 myScaleBack;

  List<Tween> tweens;


  //---------------------------------------------------------------------------------------------------------------
  void Start ()
  {
    this.myScale = new Vector3(0, 0, 1);
    this.myScaleBack = new Vector3(1, 1, 1);
    this.MyRect = this.GetComponent<RectTransform>();
    tweens = new List<Tween>();

    this.tweens.Add(this.MyRect.DOScale(myScale, 0.001f).OnComplete(KillTweens));
    //this.KillTweens();

    this.MyRect.DORotate(new Vector3(0, 0, 180), 5).SetLoops(-1, LoopType.Yoyo);
    Delay();
  }
  //---------------------------------------------------------------------------------------------------------------
  private void KillTweens()
  {
    foreach (Tween t in tweens)
    {
      t.Kill();
    }
  }
  //---------------------------------------------------------------------------------------------------------------
  private void Back()
  {
    this.KillTweens();
    this.tweens.Add(this.MyRect.DOScale(myScaleBack, UnityEngine.Random.Range(1.5f, 3.4f)).OnComplete(Diminish));



  }
  //---------------------------------------------------------------------------------------------------------------
  private void Delay()
  {
    Game.TimerManager.Start(UnityEngine.Random.Range(0.07f, 1.9f), () => { Back(); });
  }
  //---------------------------------------------------------------------------------------------------------------
  private void Diminish ()
  {
    this.KillTweens();
    this.tweens.Add(this.MyRect.DOScale(myScale, UnityEngine.Random.Range(1.5f, 3.4f)).OnComplete(Delay));




    //

  }
}
