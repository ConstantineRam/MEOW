using System.Collections;
using DG.Tweening;
using UnityEngine;
using System;

public class GhostsAnimator : MonoBehaviour
{
  [SerializeField]
  private CanvasGroup ghosts;
  Tween MyTween;
  private bool inProgress = false;
  private bool constant = false;
	// Use this for initialization
	void Start ()
  {
    constant = false;
    DateTime dt = DateTime.Now;
    if (dt.DayOfWeek == DayOfWeek.Friday)
    {
      if (dt.Day == 13)
      {
        constant = true;
      }
    }

    if (dt.Month == 11)
    {
      if (dt.Day > 29)
      {
        constant = true;
      }
    }

    if (constant)
    {
      inProgress = true;
      this.ghosts.DOFade(1, 13f).SetAutoKill();
    }

  }
  //---------------------------------------------------------------------------------------------------------------
  public void OnBallPressed()
  {
    if (inProgress)
    {
      return;
    }
    Game.AudioManager.PlaySound(AudioId.GongLow);
    this.inProgress = true;
    this.ghosts.DOFade(1, 10f).SetAutoKill();
    Game.TimerManager.Start(12f, () =>
    {
      this.ghosts.DOFade(0, 10f).SetAutoKill();
    });


      Game.TimerManager.Start(22f, () =>
    {

      this.inProgress = false;
    });
  }
  

}
