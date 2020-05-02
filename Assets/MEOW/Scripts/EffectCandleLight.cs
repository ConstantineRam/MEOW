using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EffectCandleLight : MonoBehaviour
{
  [SerializeField]
  private Image myImage;
  [SerializeField]
  private float minAlpha = 0.3f;
  [SerializeField]
  private float maxAlpha = 0.6f;

  private RectTransform MyRect;

  Tween rotator;
  TimerManager.Timer timer;
  //---------------------------------------------------------------------------------------------------------------
  void Awake()
  {
    this.MyRect = this.GetComponent<RectTransform>();
  }

  //---------------------------------------------------------------------------------------------------------------
   void Start()
  {

    Rotator();
    Blender();
   }

  //---------------------------------------------------------------------------------------------------------------
  private void Rotator()
  {
  rotator.Kill();
     rotator = this.MyRect.DORotate(new Vector3(UnityEngine.Random.Range(-20f, 20f), UnityEngine.Random.Range(-20f, 20f), UnityEngine.Random.Range(-3f, 3f)), UnityEngine.Random.Range(1f, 4f)).OnComplete(Rotator);


  }

  //---------------------------------------------------------------------------------------------------------------
  private void Blender()
  {

   this.myImage.color = new Color(this.myImage.color.r, this.myImage.color.g, this.myImage.color.b, UnityEngine.Random.Range(this.minAlpha, this.maxAlpha) );
    Game.TimerManager.Start(UnityEngine.Random.Range(0.2f, 0.9f), () => { Blender(); });
  }

}
