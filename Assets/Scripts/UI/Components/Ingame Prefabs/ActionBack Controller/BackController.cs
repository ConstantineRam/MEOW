using System;
using UnityEngine.UI;
using UnityEngine;

[Serializable]
class BackCollection
{
  [SerializeField]
  public Sprite myBack;
  [SerializeField]
  public Color MyColor;
}

public class BackController : MonoBehaviour
{
  [Tooltip("If set to true switches background randomly every time scene was loaded.")]
  [SerializeField]
  private bool RandomBack = true;

  [SerializeField]
  private BackCollection[] backCollection;


  private int CurrentSelection;
  private Image MyBackImage;
  private const int SELECT_RANDOM = -1;

  //---------------------------------------------------------------------------------------------------------------
  void Awake ()
  {
    this.ChangeBack();
  }

  //---------------------------------------------------------------------------------------------------------------
  // Allows to switch background manually instead of random
  public void SwitchBack(int backIndex)
  {
    if (backIndex < 0)
    {
      Debug.LogError("BackController.SwitchBack got index below 0.");
      return;
    }

    if (backIndex > backCollection.Length)
    {
      Debug.LogError("BackController.SwitchBack got index above back collection size.");
      return;
    }

    this.ChangeBack(backIndex);
  }
  //---------------------------------------------------------------------------------------------------------------
  private int GetRandomIndex()
  {
    if (backCollection.Length == 1)
    {
      return 0;
    }
    int result = UnityEngine.Random.Range(0, backCollection.Length);
    while (result == CurrentSelection)
    {
      result = UnityEngine.Random.Range(0, backCollection.Length);
    }

    return result;
  }

  //---------------------------------------------------------------------------------------------------------------
  private void ChangeBack(int backIndex = SELECT_RANDOM)
  {
    UnityEngine.Random.InitState(System.Environment.TickCount);
    this.MyBackImage = this.GetComponent<Image>();
    if (backIndex == SELECT_RANDOM)
    {
      CurrentSelection = GetRandomIndex();
    }
    else
    {
      CurrentSelection = backIndex;
    }
    MyBackImage.sprite = backCollection[CurrentSelection].myBack;
  }

  //---------------------------------------------------------------------------------------------------------------
  public Color GetColor()
  {
    return this.backCollection[CurrentSelection].MyColor;
  }


}
