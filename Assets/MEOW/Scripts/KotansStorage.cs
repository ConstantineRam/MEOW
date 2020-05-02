using System;
using System.Collections.Generic;
using UnityEngine;

public class KotansStorage : MonoBehaviour
{
  [SerializeField]
  KotanPart[] heads;

  [SerializeField]
  KotanPart[] frontBody;

  [SerializeField]
  KotanPart[] backBody;

  [SerializeField]
  KotanPart[] tails;

  KotanPart[][] storage;

  //---------------------------------------------------------------------------------------------------------------
  void Awake()
  {
    this.storage = new KotanPart[(int) KPart.last + 1][];
    this.storage[(int) KPart.head] = this.heads;
    this.storage[(int) KPart.frontBody] = this.frontBody;
    this.storage[(int) KPart.backBody] = this.backBody;
    this.storage[(int) KPart.tail] = this.tails;


    for (int i = 0; i < (int) KPart.last + 1; i++)
    {
      for (int j = 0; j < this.storage[i].Length; j++)
      {
        this.storage[i][j].SetNum = j;
  }
      
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public KotanPart GetPartByNum(int PartNum, KPart PartType)
  {
    if (PartNum < 0)
    {
      Debug.LogError("PartNum < 0");
      return null;
    }

    if (PartNum > this.storage[ (int) PartType].Length-1 )
    {
      Debug.LogError("PartNum > this.storage[ (int) PartType].Length-1 ");
      return null;
    }

    return this.storage[(int) PartType][PartNum];

  }

  //---------------------------------------------------------------------------------------------------------------
  public KotanData GetKotan(int KotanNum)
  {
    KotanData result = new KotanData();

    for (int i = 0; i < (int) KPart.last + 1; i++)
    {
      result.addData(this.storage[i][KotanNum], (KPart) i);
    }

    return result;
  }

  //---------------------------------------------------------------------------------------------------------------
  public KotanData GetRandomKotan()
  {
    KotanData result = new KotanData();
    
    for (int i = 0; i < (int) KPart.last+1; i++)
    {
      result.addData(this.storage[i][UnityEngine.Random.Range(0, this.storage[i].Length)], (KPart) i);
    }

    return result;
    

  }
}

[Serializable]
public class KotanPart
{
  [SerializeField]
  private String ID;

  private int num;
  public int Num { get { return num; }}
  public int SetNum { set { num = value; } }

  [SerializeField]
  private KPart part;
  public KPart Part { get { return this.part; } }

  [SerializeField]
  private Sprite sprite;
  public Sprite Sprite { get { return sprite; } }

  

}

