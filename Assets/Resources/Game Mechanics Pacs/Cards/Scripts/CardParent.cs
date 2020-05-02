using UnityEngine;

public abstract class CardParent : MonoBehaviour
{
  private RectTransform myRectTransform;
  public RectTransform GetRectTransform { get { return myRectTransform; } }

  public abstract void OnLeftParent(ACard leftCard);

  //---------------------------------------------------------------------------------------------------------------
  protected virtual void Awake()
  {
    this.myRectTransform = this.GetComponent<RectTransform>();
  }
}
