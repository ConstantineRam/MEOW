using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MMTotalScore : MonoBehaviour
{
  [SerializeField]
  private GameObject MyParent;

  private Text MyText;
  private void Awake()
  {
    this.MyText = this.GetComponent<Text>();
  }
  // Use this for initialization
  void Start ()
  {
    if (Game.Settings.BestScore == 0)
    {
      this.MyParent.gameObject.SetActive(false);
      return;
    }
    int nextToUnlock = Game.MenuRoot.UniqueKotanStorage.NextToUnlock();
    if (nextToUnlock == UniqueKotanStorage.AllUnlocked)
    {
      this.MyText.text = Game.Settings.BestScore.ToString();
    }
    else
    {
     
   this.MyText.text = Game.Settings.BestScore.ToString() + "/"+Game.MenuRoot.UniqueKotanStorage.Cost(nextToUnlock).ToString();
      
    }

   
  }
	

}
