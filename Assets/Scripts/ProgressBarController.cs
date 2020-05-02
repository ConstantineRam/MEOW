using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ProgressBarController : MonoBehaviour
{
  
  [Tooltip("The main sprite that works fills Progress Bar")]
  [SerializeField]
  private Image BarSprite;
  [Tooltip("Mask sprite that drawn above the BarSprite")]
  [SerializeField]
  private Image TopMaskSprite;
  [Tooltip("Optional; Sprite that drawn below the BarSprite")]
  [SerializeField]
  private Image BottomMaskSprite;

  [Tooltip("Set Progress bar to be active from left to right (horisontal), bottom to top (vertical)")]
  [SerializeField]
  private eBarDrawDirection BarDrawDirection;

  [Tooltip("If set to True Progress Bar will receave click and adjust self. Could be used to receive input from Player.")]
  [SerializeField]
  private bool ReceivesInput = false;

  [Tooltip("If set to True Progress Bar will show numeric information like 60/120. Horisontal only.")]
  [SerializeField]
  private bool ShowText = false;

  //[Tooltip("If set to True Progress Bar will slowly show it changed status right away, but move the slider with a delay.")]
  //public bool Animated = false;

  private int MaxAmount = 0;
  private int CurrentAmount = 0;
  private int CurrentPercentage = 0;

 //------------------------------------------------------------------------------------------------------------------------------------
  void Awake ()
  {
    this.BarSprite.material = new Material(this.BarSprite.material);
    this.BarSprite.material.SetFloat("_Alpha", 1);
    this.BarSprite.material.SetFloat("_Slider", 1);
  }

  //------------------------------------------------------------------------------------------------------------------------------------
  public int GetPercent ()
  {
    return this.CurrentPercentage;
	}

  //------------------------------------------------------------------------------------------------------------------------------------
  public void SetPercent(int percentageToSet)
  {
    if (percentageToSet < 0)
    {
      this.CurrentPercentage = 0;
      return;
    }

    if (percentageToSet > 100)
    {
      this.CurrentPercentage = 100;
      return;
    }

    this.CurrentPercentage = percentageToSet;
  }

  //------------------------------------------------------------------------------------------------------------------------------------
  public int GetMaxAmount()
  {
    return this.MaxAmount;
  }

  //------------------------------------------------------------------------------------------------------------------------------------
  public int GetCurrentAmount()
  {
    return this.CurrentAmount;
  }

  //------------------------------------------------------------------------------------------------------------------------------------
  public void SetAmount(int maxAmountToSet, int currentAmountToSet)
  {
    this.SetMaxAmount(maxAmountToSet);
    this.SetCurrentAmount(currentAmountToSet);
  }

  //------------------------------------------------------------------------------------------------------------------------------------
  public void SetCurrentAmount(int currentAmountToSet)
  {
    this.CurrentAmount = currentAmountToSet;
    if (this.GetCurrentAmount() > this.GetMaxAmount())
    {
      Debug.LogError("Progress Bar "+ this.name + " got invalid current amount "+ this.GetCurrentAmount() + " when max amount is == "+ this.GetMaxAmount());
    }
  }

  //------------------------------------------------------------------------------------------------------------------------------------
  public void SetMaxAmount(int maxAmountToSet)
  {
    this.MaxAmount = maxAmountToSet;
    if (this.GetCurrentAmount() > this.GetMaxAmount())
    {
      Debug.LogError("Progress Bar " + this.name + " got invalid max amount " + this.GetMaxAmount() + " when current amount is == " + this.GetCurrentAmount());
    }
  }

  //------------------------------------------------------------------------------------------------------------------------------------
  private void calculatePercentage()
  {
    if (this.GetMaxAmount() == 0)
    {
      this.CurrentPercentage = 0;
      return;
    }

    this.CurrentPercentage = this.GetCurrentAmount() / this.GetMaxAmount();
  }

  //------------------------------------------------------------------------------------------------------------------------------------
  private void UpdateBar()
  {

  }
}


public enum eBarDrawDirection
{
  Horisontal = 0,
  Vertical = 1
}
