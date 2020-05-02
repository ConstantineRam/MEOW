using System;
using System.Collections.Generic;
using UnityEngine;


public class CustomKotamData : KotanData
{

  [Serializable]
  public struct SerialisedKotanData
  {
    public int head;
    public int tail;
    public int frontBody;
    public int backBody;
    public string name;
    public string quote;
  }

  //---------------------------------------------------------------------------------------------------------------

  public const string NO_NAME = "";
  public const string NO_QUOTE = "";

  private string CustomName;
  public string GetName { get { return this.CustomName; } }
  public string SetName { set { this.CustomName = value; } }

  private string CustomQuote;
  public string GetQuote { get { return this.CustomQuote; } }
  public string SetQuote { set { this.CustomQuote = value; } }


  //---------------------------------------------------------------------------------------------------------------
  private KotanPart GetPartByNum(int PartNum, KPart PartType)
  {
    if (Game.StateManager.CurrentState == GameState.Action)
    {
      return Game.ActionRoot.KotansStorage.GetPartByNum(PartNum, PartType);
    }

    if (Game.StateManager.CurrentState == GameState.Menu)
    {
      return Game.MenuRoot.KotansStorage.GetPartByNum(PartNum, PartType);
    }
    return null;
  }
  //---------------------------------------------------------------------------------------------------------------
  public void Clone(KotanData dataToClone)
  {
    this.SetName = "";
    this.SetQuote = "";

    this.addData(dataToClone.GetData(KPart.head), KPart.head);
    this.addData(dataToClone.GetData(KPart.tail), KPart.tail);
    this.addData(dataToClone.GetData(KPart.frontBody), KPart.frontBody);
    this.addData(dataToClone.GetData(KPart.backBody), KPart.backBody);

  }


  //---------------------------------------------------------------------------------------------------------------
  public void DeSerialiseKotan(SerialisedKotanData data)
  {
    this.addData(this.GetPartByNum(data.head, KPart.head) , KPart.head);
    this.addData(this.GetPartByNum(data.tail, KPart.tail), KPart.tail);
    this.addData(this.GetPartByNum(data.frontBody, KPart.frontBody), KPart.frontBody);
    this.addData(this.GetPartByNum(data.backBody, KPart.backBody), KPart.backBody);

    this.SetQuote = data.quote;
    this.SetName = data.name;


  }

  //---------------------------------------------------------------------------------------------------------------
  public SerialisedKotanData SerialiseKotan()
  {
    SerialisedKotanData result = new SerialisedKotanData();

    result.name = this.GetName;
    result.quote = this.GetQuote;

    result.head = this.GetData(KPart.head).Num;
    result.tail = this.GetData(KPart.tail).Num;
    result.frontBody = this.GetData(KPart.frontBody).Num;
    result.backBody = this.GetData(KPart.backBody).Num;

    return result;
  }
}
