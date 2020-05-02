using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TweenPusher : MonoBehaviour
{
  [Tooltip("Only for UI elements.")]
  [SerializeField]
  private float TweenTime;

  [SerializeField]
  private float WaitTime;

  [SerializeField]
  private float InitialDelay;

  [SerializeField]
  private Vector3 PunchPower;

  private RectTransform MyRect;
  private Tween MyTween;
  private TimerManager.Timer TweenTimer;

  //---------------------------------------------------------------------------------------------------------------
  void Awake()
  {
    this.MyRect = this.GetComponent<RectTransform>();
  }

  //---------------------------------------------------------------------------------------------------------------
  void Start ()
  {
    if (this.MyRect == null)
    {
      Debug.LogError("RectTransform wasn't found.");
      return;
    }

    if (this.InitialDelay == 0)
    {
      AnimationProicess();
    }
    else
    {
      this.TweenTimer = Game.TimerManager.Start(this.InitialDelay, AnimationProicess);
      
    }

  }
  //---------------------------------------------------------------------------------------------------------------
  private void AnimationProicess()
  {
    this.TweenTimer = null;
    this.MyTween = this.MyRect.DOPunchScale(this.PunchPower, this.TweenTime).SetAutoKill();
    this.TweenTimer = Game.TimerManager.Start(this.TweenTime + this.WaitTime, AnimationProicess);
  }

  //---------------------------------------------------------------------------------------------------------------
  void OnDestroy ()
  {
    if (this.TweenTimer != null)
    {
      this.TweenTimer.Halt();
    }
    
    this.MyTween.Kill();

	}
}
