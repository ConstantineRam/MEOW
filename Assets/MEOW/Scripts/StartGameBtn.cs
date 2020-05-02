using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartGameBtn : MonoBehaviour
{
  [SerializeField]
  [Range(0.001f,0.9f)]
  private float ScaleDepth = 0.5f;
  [SerializeField]
  private float ScaleTime = 1f;
  [SerializeField]
  private float FloatPower = 1.2f;
  [SerializeField]
  private float FloatTime = 1f;
  [SerializeField]
  private float RotationAngle;
  [SerializeField]
  private float RotationTime;

  [SerializeField]
  private Image BtnImage;
  [SerializeField]
  private Image ShadeImage;

  [SerializeField]
  private SweepAnimationComponent SweepShader;

  Tween BtnFloat;
  Tween BtnScaler;
  Tween ShadeScaler;
  Tween Rotator;

  TimerManager.Timer FloatTimer;
  TimerManager.Timer ScaleTimer;
  TimerManager.Timer RotationTimer;

  Vector3 ScaleBtn;
  Vector3 ScaleShade;

  private float currentY;
  private float FloatDelay;
  private float ScaleDelay;
  private float RotationDelay;
  private Vector3 BackRotation;
  private Vector3 FrontRotation;

  private RectTransform MyRect;


  //---------------------------------------------------------------------------------------------------------------
  public void OnDestroy()
  {
    if (this.FloatTimer != null)
    {
      this.FloatTimer.Halt();
    }

    if (this.ScaleTimer != null)
    {
      this.ScaleTimer.Halt();
    }
    
    if (this.RotationTimer != null)
    {
      this.RotationTimer.Halt();
    }
    

    this.BtnFloat.Kill();
    this.BtnScaler.Kill();
    this.ShadeScaler.Kill();
    this.Rotator.Kill();
  }

  //---------------------------------------------------------------------------------------------------------------
  void Start ()
  {
    if (Game.MenuRoot.UniqueKotanStorage.IsItTimeToBuyGame())
    {
      return;
    }


    this.SweepShader.Activate();

      this.MyRect = this.GetComponent<RectTransform>();

    ScaleBtn = new Vector3 (1+this.ScaleDepth, 1+this.ScaleDepth, 1);
    ScaleShade = new Vector3(1 - this.ScaleDepth, 1 - this.ScaleDepth, 1);
    this.currentY = this.transform.position.y;

    this.FloatDelay = this.FloatTime * 1.01f;

    this.ScaleDelay = this.ScaleTime * 1.01f;
    this.RotationDelay = this.RotationTime * 1.1f;
    this.BackRotation = new Vector3(0, 0, this.RotationAngle * -1);
    this.FrontRotation = new Vector3(0, 0, this.RotationAngle);

    this.FloatUp();
    this.RotateFront();
    this.ScaleUp();
  }

  //---------------------------------------------------------------------------------------------------------------
  private void ScaleUp()
  {
    this.BtnScaler.Kill();
    this.ShadeScaler.Kill();

    this.BtnScaler = this.BtnImage.rectTransform.DOScale(this.ScaleBtn, this.ScaleTime);
    this.ShadeScaler = this.ShadeImage.rectTransform.DOScale(this.ScaleShade, this.ScaleTime);

    this.RotationTimer = Game.TimerManager.Start(this.ScaleDelay, ScaleDown);
  }

  //---------------------------------------------------------------------------------------------------------------
  private void ScaleDown()
  {
    this.BtnScaler.Kill();
    this.ShadeScaler.Kill();

    this.BtnScaler = this.BtnImage.rectTransform.DOScale(Vector3.one, this.ScaleTime);
    this.ShadeScaler = this.ShadeImage.rectTransform.DOScale(Vector3.one, this.ScaleTime);

    this.RotationTimer = Game.TimerManager.Start(this.ScaleDelay, ScaleUp);
  }

  //---------------------------------------------------------------------------------------------------------------
  private void RotateFront()
  {
    this.Rotator.Kill();
    this.Rotator = this.MyRect.DORotate (this.FrontRotation, this.RotationTime);

    this.RotationTimer = Game.TimerManager.Start(this.RotationDelay, RotateBack);
  }


  //---------------------------------------------------------------------------------------------------------------
  private void RotateBack()
  {
    this.Rotator.Kill();
    this.Rotator = this.MyRect.DORotate(this.BackRotation, this.RotationTime);

    this.RotationTimer = Game.TimerManager.Start(this.RotationDelay, RotateFront);
  }
  //---------------------------------------------------------------------------------------------------------------
  private void FloatUp()
  {
    this.BtnFloat.Kill();
    this.BtnFloat = this.transform.DOMoveY (this.currentY + this.FloatPower, this.FloatTime);

    this.FloatTimer = Game.TimerManager.Start(this.FloatDelay, FloatDown);
  }

  //---------------------------------------------------------------------------------------------------------------
  private void FloatDown()
  {
    this.BtnFloat.Kill();
    this.BtnFloat = this.transform.DOMoveY(this.currentY - this.FloatPower, this.FloatTime);

    this.FloatTimer = Game.TimerManager.Start(this.FloatDelay, FloatUp);
  }

  //---------------------------------------------------------------------------------------------------------------


}
