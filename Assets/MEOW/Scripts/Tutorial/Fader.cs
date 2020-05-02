using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fader : MonoBehaviour
{

  [SerializeField]
  private bool Hidden;

  private CanvasGroup myGroup;
	void Awake ()
  {
    this.myGroup = this.GetComponent<CanvasGroup>();
    if (this.myGroup == null)
    {
      Debug.LogError("No Canvas component.");
    }

    if (Hidden)
    {
      this.myGroup.alpha = 0;
    }
    else
    {
      this.myGroup.alpha = 1;
    }
	}

  //---------------------------------------------------------------------------------------------------------------
  public void Show(float Time)
  {
    this.myGroup.DOFade(1, Time);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void Hide(float Time)
  {
    this.myGroup.DOFade(0, Time);
  }
}
