
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EffectLight : MonoBehaviour
{
  [SerializeField]
  private Image myImage;
  [SerializeField]
  private float minAlpha = 0.3f;
  [SerializeField]
  private float maxAlpha = 0.6f;
  [SerializeField]
  private float Time = 1.5f;


  private RectTransform MyRect;
  private float HalfTime;


  TimerManager.Timer timer;
  //---------------------------------------------------------------------------------------------------------------
  void Awake()
  {
    this.MyRect = this.GetComponent<RectTransform>();
    this.HalfTime = Time / 2;
  }

  //---------------------------------------------------------------------------------------------------------------
  void Start()
  {
    Blender();
  }


  //---------------------------------------------------------------------------------------------------------------
  private void Blender()
  {

    this.myImage.color = new Color(this.myImage.color.r, this.myImage.color.g, this.myImage.color.b, UnityEngine.Random.Range(this.minAlpha, this.maxAlpha));
    timer = Game.TimerManager.Start(UnityEngine.Random.Range(HalfTime, Time), () => { Blender(); });
  }


  //---------------------------------------------------------------------------------------------------------------
  public void OnDestroy()
  {
    timer.Halt();

  }
}
