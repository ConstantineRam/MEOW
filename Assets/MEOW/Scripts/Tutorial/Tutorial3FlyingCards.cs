using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tutorial3FlyingCards : ActionTutorialPopUp
{
  [SerializeField]
  private float FlyTime = 1f;
  

  [SerializeField]
  private RectTransform Card1;
  [SerializeField]
  private RectTransform Card2;
  [SerializeField]
  private RectTransform Card3;
  [SerializeField]
  private RectTransform Card4;

  [SerializeField]
  private RectTransform Target1;
  [SerializeField]
  private RectTransform Target2;
  [SerializeField]
  private RectTransform Target3;
  [SerializeField]
  private RectTransform Target4;


  private Tween Card1tw;
  private Tween Card2tw;
  private Tween Card3tw;
  private Tween Card4tw;

  private Vector3 Coords1;
  private Vector3 Coords2;
  private Vector3 Coords3;
  private Vector3 Coords4;

  private TimerManager.Timer Card1Timer;
  private TimerManager.Timer Card2Timer;
  private TimerManager.Timer Card3Timer;
  private TimerManager.Timer Card4Timer;

  // Use this for initialization
  void Start () {
		
	}

  //---------------------------------------------------------------------------------------------------------------
  public sealed override void OnClose()
  {
    base.OnClose();
    if (this.Card1Timer != null)
    {
      this.Card1Timer.Halt();
    }
    if (this.Card2Timer != null)
    {
      this.Card2Timer.Halt();
    }
    if (this.Card3Timer != null)
    {
      this.Card3Timer.Halt();
    }
    if (this.Card4Timer != null)
    {
      this.Card4Timer.Halt();
    }

    this.KillTwinners();
  }

  //---------------------------------------------------------------------------------------------------------------
  public sealed override void Activate(object data)
  {
    this.Coords1 = this.Card1.localPosition;
    this.Coords2 = this.Card2.localPosition;
    this.Coords3 = this.Card3.localPosition;
    this.Coords4 = this.Card4.localPosition;

    this.Animation1();

  }

  //---------------------------------------------------------------------------------------------------------------
  private void KillTwinners()
  {
    this.Card1tw.Kill();
    this.Card2tw.Kill();
    this.Card3tw.Kill();
    this.Card4tw.Kill();
  }

  //---------------------------------------------------------------------------------------------------------------
  private void Animation1()
  {
    this.KillTwinners();

    this.Card1tw = this.Card1.DOLocalMove(this.Target1.localPosition, this.FlyTime);
    this.Card1Timer = Game.TimerManager.Start(this.FlyTime / 3, () =>
    {
      this.Animation2();
    });
  }

  //---------------------------------------------------------------------------------------------------------------
  private void Animation2()
  {
    this.Card2tw = this.Card2.DOLocalMove(this.Target2.localPosition, this.FlyTime);
    this.Card2Timer = Game.TimerManager.Start(this.FlyTime / 3, () =>
    {
      this.Animation3();
    });
  }

  //---------------------------------------------------------------------------------------------------------------
  private void Animation3()
  {
    this.Card3tw = this.Card3.DOLocalMove(this.Target3.localPosition, this.FlyTime);
    this.Card3Timer = Game.TimerManager.Start(this.FlyTime / 3, () =>
    {
      this.Animation4();
    });
  }

  //---------------------------------------------------------------------------------------------------------------
  private void Animation4()
  {
    this.Card4tw = this.Card4.DOLocalMove(this.Target4.localPosition, this.FlyTime);
  }
}
