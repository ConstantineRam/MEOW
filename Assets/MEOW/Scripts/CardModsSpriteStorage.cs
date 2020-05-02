using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModsSpriteStorage : MonoBehaviour
{

  [SerializeField]
  private MSprites[] ModSprite;


	
	void Start ()
  {
    if (this.ModSprite.Length < 1)
    {
      Debug.LogError("Mod sprite storage has no sprites");
    }
	}

  public Sprite GetSprite(CardTypes ct, CardMod mod)
  {
    MSprites str = this.ModSprite[(int) ct];
    return str.sprite  [(int) mod];
  }
}
[System.Serializable]
struct MSprites
  {
 public Sprite[] sprite;
}