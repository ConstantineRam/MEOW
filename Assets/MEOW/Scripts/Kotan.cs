using UnityEngine.UI;
using UnityEngine;

public class Kotan : APoolable
{
  [SerializeField]
  private Image[] DrawParts;

  private KotanData kotanData;
  public bool HasKotanData { get { if (kotanData == null) { return false; } return true; } }

  protected RectTransform rectTransform;
  public RectTransform RectTransform { get { return rectTransform; } }

  //---------------------------------------------------------------------------------------------------------------
  public override void OnReturnedToPool()
  {

  }

  //---------------------------------------------------------------------------------------------------------------
  protected virtual void Awake ()
  {
    this.TurnOff();
    this.rectTransform = this.GetComponent<RectTransform>();

	}

  //---------------------------------------------------------------------------------------------------------------
  public KotanData GetKotanData()
  {
    return this.kotanData;
  }


  //---------------------------------------------------------------------------------------------------------------
  public void TurnOff()
  {
    for (int i = 0; i < (int) KPart.last + 1; i++)
    {
      this.DrawParts[(int) i].color = new Color (0, 0, 0, 0);
    }
    this.rectTransform = this.GetComponent<RectTransform>();

    this.kotanData = null;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void TurnOn()
  {
    for (int i = 0; i < (int) KPart.last + 1; i++)
    {
      this.DrawParts[(int) i].color = new Color(255, 255, 255, 255);
    }
    this.rectTransform = this.GetComponent<RectTransform>();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ActivateKotan(KotanData newData)
  {
    this.rectTransform = this.GetComponent<RectTransform>();
    this.kotanData = newData;

    if (!this.HasKotanData)
    {
      Debug.LogError("null at kotan data!");
      return;
    }

    //this.gameObject.SetActive(true);
    for (int i = 0; i < (int) KPart.last+1; i++)
    {
      this.DrawParts[(int) i].sprite = this.kotanData.GetSprite((KPart) i);
    }

  }

}


