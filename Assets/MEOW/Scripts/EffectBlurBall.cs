using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EffectBlurBall : MonoBehaviour
{
  [SerializeField]
  private Image myImage;
  [SerializeField]
  private float minMove = -10f;
  [SerializeField]
  private float maxMove = 10f;
  [SerializeField]
  [Range(0f,1f)]
  private float Alha = 0.2f;
  protected RectTransform MyRect;

  Vector3 myNewPosition;
  Vector3 myOldPosition;
  Tween randomator;

  //---------------------------------------------------------------------------------------------------------------
  void Awake()
  {
    this.MyRect = this.GetComponent<RectTransform>();


    this.myImage.color = new Color(this.myImage.color.r, this.myImage.color.g, this.myImage.color.b, Alha);
  }
  //---------------------------------------------------------------------------------------------------------------
  void Start()
  {


    Rotator();

  }
  //---------------------------------------------------------------------------------------------------------------
  private void BackRotator()
  {
    float time = UnityEngine.Random.Range(2f, 6f);
    this.randomator.Kill();
    randomator = this.MyRect.DOLocalMove(myOldPosition, time).OnComplete(Rotator);
  }
   //---------------------------------------------------------------------------------------------------------------
    private void Rotator()
  {
    if (this.randomator != null)
    {
      this.randomator.Kill();
    }
    

    float time = UnityEngine.Random.Range(2f, 6f);
    myOldPosition = new Vector3(this.MyRect.localPosition.x, this.MyRect.localPosition.y, this.MyRect.localPosition.z);
     myNewPosition = new Vector3(this.MyRect.localPosition.x + UnityEngine.Random.Range(this.minMove, this.maxMove), this.MyRect.localPosition.y + UnityEngine.Random.Range(this.minMove, this.maxMove), this.MyRect.localPosition.z + UnityEngine.Random.Range(this.minMove, this.maxMove));
    randomator = this.MyRect.DOLocalMove(myNewPosition, time).OnComplete(BackRotator);

  }


}
