using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ACAtCardData : ACardData
{

  [SerializeField]
  private int points;
  public int Points { get { return points; } }
  public int SetPoints { set { points = value; } }
  public bool isBlack { get { if (this.Points < 1) { return true; } return false; } }

  public int UnlocksAt;



}

