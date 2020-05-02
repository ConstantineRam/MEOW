using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyBtnText : MonoBehaviour {
  private Text MyText;
  private void Awake()
  {
    this.MyText = this.GetComponent<Text>();
  }

  void Start ()
  {

    this.CheckPremium();

  }

  public void CheckPremium()
  {
    if (Game.Settings.IsPremium)
    {
      this.MyText.text = "meow";
    }
  }
}
