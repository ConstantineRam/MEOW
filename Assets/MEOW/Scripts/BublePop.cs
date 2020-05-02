using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BublePop : EffectBlurBall
{
  private Tween poper;
  private TimerManager.Timer TweenTimer;

  //---------------------------------------------------------------------------------------------------------------
  public void OnBubblePopped()
  {
    this.poper = this.MyRect.DOPunchScale(new Vector3(0.3f, 0.3f, 0), 0.2f).SetAutoKill();
    this.TweenTimer = Game.TimerManager.Start(0.3f, () => { this.gameObject.SetActive(false); Game.AudioManager.PlaySound(AudioId.Plop, 0.6f);  });
  }

  //---------------------------------------------------------------------------------------------------------------
  private void OnDestroy()
  {
    if (this.TweenTimer != null)
    {
      this.TweenTimer.Halt();
    }
    
  }
}
