using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KotanData
{
  KotanPart[] parts;


  //---------------------------------------------------------------------------------------------------------------
  public KotanData()
  {
    this.parts = new KotanPart[(int) KPart.last + 1];
  }

  //---------------------------------------------------------------------------------------------------------------
  public void addData(KotanPart kotanData, KPart part)
  {
    if (kotanData == null)
    {
      Debug.LogError("null at KotanData.");
      return;
    }

    this.parts[(int) part] = kotanData;
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool HasAllParts()
  {
    for (int i = 0; i < (int) KPart.last+1; i++)
    {
      if (this.parts[i] == null)
      {
        return false;
      }

      if (this.parts[i].Sprite == null)
      {
        return false;
      }
     
    }

    return true;
  }

  //---------------------------------------------------------------------------------------------------------------
  public KotanPart GetData(KPart part)
  {
    return this.parts[(int) part];
  }

  //---------------------------------------------------------------------------------------------------------------
  public Sprite GetSprite(KPart part)
  {
    return this.GetData(part).Sprite;
  }
}
