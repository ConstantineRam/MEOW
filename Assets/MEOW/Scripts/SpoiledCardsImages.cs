using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoiledCardsImages : MonoBehaviour
{
  [SerializeField]
  Sprite[] storage;

  [SerializeField]
  int[] score;


  public struct SpoilResult
  {
    public int score;
    public Sprite bitmap;
  }

  //---------------------------------------------------------------------------------------------------------------
  void Start ()
  {
    if (this.storage.Length < 1)
    {
      Debug.LogError("Spoiled cards storage has no images.");
    }

    if (this.storage.Length != this.score.Length)
    {
      Debug.LogError("Spoiled cards storage has wrong amount of images and scores.");
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public SpoilResult GetRandom ()
  {
    SpoilResult result = new SpoilResult();
    int RandomResult = UnityEngine.Random.Range(0, this.storage.Length);

    result.score = this.score[RandomResult];
    result.bitmap = this.storage[RandomResult];
    return result;
	}




}


