using UnityEngine.UI;
using UnityEngine;

public class PopUpKotanCard : RemaningPoUp
{
  [SerializeField]
  private Kotan myKotan;

  [SerializeField]
  private Text KotanName;

  [SerializeField]
  private Text DontForget;

  [SerializeField]
  private Button SaveButton;
  private KotanPopupData myData;


  //---------------------------------------------------------------------------------------------------------------
  public class KotanPopupData
  {
    private CustomKotamData kotanData;
    public CustomKotamData KotanData { get { return kotanData; } }

    private PopupId popUpCallback = PopupId.NoPopUp;
    public PopupId PopUpCallback { get { return popUpCallback; } }
    public PopupId SetPopUpCallback { set { popUpCallback = value; } }

    //---------------------------------------------------------------------------------------------------------------
    public KotanPopupData(CustomKotamData newData, PopupId popUpCallback = PopupId.NoPopUp)
    {
      this.kotanData = newData;

      if (this.KotanData == null)
      {
        Debug.LogError("No Kotan Data was created for PopUp.");
      }
    }
   
  }

  //---------------------------------------------------------------------------------------------------------------
  public sealed override void OnRenameFinished()
  {
    base.OnRenameFinished();
    this.DontForget.gameObject.SetActive(true);
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void Activate(object data)
  {
    if (data == null)
    {
      this.CloseSelf();
    }

    if (data != null && data is KotanPopupData)
    {
      this.myData = (KotanPopupData) data;
    }

    this.myCurrentData = this.myData.KotanData;

    this.PrepareNamingFields();
    this.myKotan.ActivateKotan(this.myCurrentData);
    this.myKotan.TurnOn();

    this.DontForget.gameObject.SetActive(false);

    this.SaveButton.interactable = true;
    if (this.CustomKotanStorage().HasKotan(this.myCurrentData))
    {
      this.SaveButton.interactable = false;
    }
    
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void CloseSelf()
  {
    if (this.myData.PopUpCallback == PopupId.NoPopUp)
    {

      base.CloseSelf();
      return;
    }

    Game.UiManager.Open(this.myData.PopUpCallback);

  }

  //---------------------------------------------------------------------------------------------------------------
  private CustomKotanStorage CustomKotanStorage()
  {
    if (Game.StateManager.CurrentState == GameState.Menu)
    {
      return Game.MenuRoot.CustomKotanStorage;
    }

    if (Game.StateManager.CurrentState == GameState.Action)
    {
      return Game.ActionRoot.CustomKotanStorage;
    }

    return null;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OpenCatBook()
  {


  }

  //---------------------------------------------------------------------------------------------------------------
  public void SaveKotan()
  {
    if (!this.CustomKotanStorage().PushKotan(this.myCurrentData))
    {
      Debug.Log("No Slots in kotan storage.");
      //no free space. buy game ^_^
    }
    else
    {
      this.SaveButton.interactable = false;
    }

    this.CustomKotanStorage().SaveArray();

    this.DontForget.gameObject.SetActive(false);

  }

}
