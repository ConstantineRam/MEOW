using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyGameBtnAnimator : MonoBehaviour
{
  [SerializeField]
  private SweepAnimationComponent SweepShader;

  private Shaker shaker;

  void Awake()
  {
    this.shaker = this.GetComponent<Shaker>();  
  }

  void Start ()
  {
    if (!Game.MenuRoot.UniqueKotanStorage.IsItTimeToBuyGame())
    {
      return;
    }


    this.SweepShader.Activate();
    this.shaker.StartAnimation();
  }
	

}
