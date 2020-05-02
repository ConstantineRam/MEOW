using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuHelloweenManager : MonoBehaviour
{
  [SerializeField]
  private Image[] blurBalls;

  [SerializeField]
  private Sprite HalloweenBlur;

  //---------------------------------------------------------------------------------------------------------------
  public void StartHelloween()
  {
    foreach (Image i in blurBalls)
    {
      i.sprite = HalloweenBlur;
    }
  }
}
