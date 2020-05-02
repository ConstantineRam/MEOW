using UnityEngine;
using UnityEngine.UI;

public class OkPopup : GenericPopup
{

  [SerializeField]
  private Text infoText;

  public override void Activate(object data)
  {
    if (data is string)
    {
      infoText.text = (string) data;
    }
  }

  public void ButtonClick()
  {
    CloseSelf();
  }
}
