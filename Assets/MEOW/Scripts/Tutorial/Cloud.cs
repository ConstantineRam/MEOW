using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Cloud : MonoBehaviour
{

  private Vector3 HideLocation;
  [SerializeField]
  private Vector3 ShowLocation;

  private Image cloudImage;
  private RectTransform myRect;

	// Use this for initialization
	void Awake ()
  {
    this.cloudImage = this.GetComponent<Image>();

    if (this.cloudImage == null)
    {
      Debug.LogError("No Image component.");
    }

    
    this.myRect = this.GetComponent<RectTransform>();
    this.HideLocation = myRect.localPosition;

  }
  //---------------------------------------------------------------------------------------------------------------
  public void Show(float Time)
  {
    this.myRect.DOLocalMove(this.ShowLocation, Time);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void SetShowCoords(Vector3 newCoords)
  {
    this.ShowLocation = newCoords;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void Hide(float Time)
  {
    this.myRect.DOLocalMove(this.HideLocation, Time);
  }
}
