using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

  public static CardStorage CardStorage { get; private set; }
  //---------------------------------------------------------------------------------------------------------------
  void Awake()
  {
    DontDestroyOnLoad(this.gameObject);
  }

  //---------------------------------------------------------------------------------------------------------------
  void Start ()
  {
    CardStorage = GetComponentInChildren<CardStorage>();
	}


}
