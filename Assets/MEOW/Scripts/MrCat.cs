using System;
using UnityEngine.UI;
using UnityEngine;

public class MrCat : MonoBehaviour
{
  [Header("State #0 is to be starting state.")]

  [SerializeField]
  private CatState[] state;

  [SerializeField]
  private Image myImage;

  private int currentState = 0;
  private int currentFrame = 0;
  public const int DefaultState = 0;
  void Awake ()
  {
    this.PlayState();
	}

  //---------------------------------------------------------------------------------------------------------------
  public void SetAlpha(float alpha)
  {
    this.myImage.color = new Color(1, 1, 1, alpha);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ChangeState(int newState, float time)
  {
    if (!this.CheckNewState(newState))
    {
      return;
    }
    this.ChangeState(newState);
    Game.TimerManager.Start(time, () => { this.ChangeState(DefaultState); });
  }
  //---------------------------------------------------------------------------------------------------------------
  public void ChangeState(int newState)
  {
    if (!this.CheckNewState(newState))
    {
      return;
    }

    this.currentState = newState;
    this.currentFrame = 0;
  }
  //---------------------------------------------------------------------------------------------------------------
  private bool CheckNewState(int newState)
  {
    if (newState > this.state.Length)
    {
      Debug.LogError("newState > this.state.Length");
      return false;
    }

    if (newState < 0)
    {
      Debug.LogError("newState < 0");
      return false;
    }

    return true;
  }

  //---------------------------------------------------------------------------------------------------------------
  private void PlayState()
  {
    this.currentFrame++;

    if ((this.state[this.currentState].frame.Length-1) < this.currentFrame)
    {
      this.currentFrame = 0;
    }

    this.myImage.sprite = this.state[this.currentState].frame[currentFrame];
    Game.TimerManager.Start(this.state[this.currentState].SwitchTime, this.PlayState);
  }

}

[Serializable]
public class CatState
{
  [SerializeField]
  private float switchTime;
  public float SwitchTime { get { return switchTime; } }
  
  public Sprite[] frame;


}