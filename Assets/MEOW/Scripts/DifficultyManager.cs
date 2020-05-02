using System;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
  [Tooltip("the amount of wins that considered as middle ground. No changes is applied. Everything bellow gets bonuses, everything above get penalties.")]
  [SerializeField]
  private int MedianState;

  [Tooltip("Top amount of wins and loses taken under cosideration.")]
  [SerializeField]
  private int MaxCount = 10;

  //[Tooltip("How fast difficulty should change chances.")]
  //[SerializeField]
  //private int EscalationStep = 1;

  //---------------------------------------------------------------------------------------------------------------
  private int getDifference()
  {
   return Math.Abs(MedianState - Game.Settings.WinCount);
  }

  //---------------------------------------------------------------------------------------------------------------
  private int ApplyMods(int initialChance, CheckType checkType)
  {
    int CurrentWins = Game.Settings.WinCount;
    int newChance = initialChance;

    if (CurrentWins == MedianState)
    {
      return newChance;
    }

    int step = Math.Max(1, initialChance / 6);
    int Mod = step * this.getDifference();
    if (checkType == CheckType.Negative)
    {
      if (CurrentWins > MedianState)
      {
        newChance = newChance + Mod;
      }
      else
      {
        newChance = newChance - Mod;
      }

      return newChance;
    }


    if (CurrentWins > MedianState)
    {
      newChance = newChance - Mod;
    }
    else
    {
      newChance = newChance + Mod;
    }
    return newChance;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void RegisterWin()
  {
    Debug.Log("RegisterWin");
    if (Game.Settings.WinCount < this.MedianState)
    {
      Debug.Log("Win count " + Game.Settings.WinCount);
      Game.Settings.WinCount = Game.Settings.WinCount + 1;
      return;
    }

    if (this.getDifference() >= this.MaxCount)
    {
      Debug.Log("Win count limit riched.");
      return;
    }

    Game.Settings.WinCount = Game.Settings.WinCount + 1;
    Debug.Log("Win count " + Game.Settings.WinCount);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void RegisterLose()
  {
    Debug.Log("RegisterLose");
    if (Game.Settings.WinCount > this.MedianState)
    {
      Game.Settings.WinCount = Game.Settings.WinCount - 2;
      Debug.Log("Win count " + Game.Settings.WinCount);
      return;
    }

    if (this.getDifference() >= this.MaxCount)
    {
      Debug.Log("Win count limit riched.");
      return;
    }

    Game.Settings.WinCount = Game.Settings.WinCount - 1;

    Debug.Log("Win count " + Game.Settings.WinCount);
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool isAboveMediane()
  {
    if (Game.Settings.WinCount > this.MedianState)
    {
      return true;
    }

    return false;
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="initialChance"></param>
  /// <param name="checkType"></param>
  /// <param name="MinPercent"> ignored if == 0 </param>
  /// <returns></returns>
  public bool ChanceOf100(int initialChance, CheckType checkType, int MinPercent = 0)
  {


    int newChance = this.ApplyMods(initialChance, checkType);
    //  Debug.Log("Win count " + Game.Settings.WinCount);
    //  Debug.Log(" initial "+ initialChance +". new chance "+ newChance);

    newChance = Math.Max(MinPercent, initialChance);



    if (UnityEngine.Random.Range(0, 100) < newChance)
    {
  //    Debug.Log(checkType + " true");
      return true;
    }
  //  Debug.Log(checkType + " false");
    return false;
    
  }


  public enum CheckType
  {
    Positive = 0,
    Negative = 1
  }
}
