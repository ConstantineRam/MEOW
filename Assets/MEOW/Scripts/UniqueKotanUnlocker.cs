using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniqueKotanUnlocker : MonoBehaviour
{
  [SerializeField]
  private Image[] CatImages;

	// Use this for initialization
	void Start ()
  {
    if (this.CatImages.Length < Game.MenuRoot.UniqueKotanStorage.MaxAmount())
    {
      Debug.LogError("Not enough Cat images");
      return;
    }
    for (int i = 0; i < CatImages.Length; i++)
    {
        this.CatImages[i].gameObject.SetActive(false);
    }

    for (int i = 1; i < Game.MenuRoot.UniqueKotanStorage.MaxAmount()+1; i++)
    {
      if (Game.MenuRoot.UniqueKotanStorage.isUnlocked(i))
        {
        this.CatImages[ Game.MenuRoot.UniqueKotanStorage.GetNumByPos(i) -1].gameObject.SetActive(true);
        }
    }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
