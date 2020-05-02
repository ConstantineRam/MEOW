using UnityEngine.UI;
using UnityEngine;

public class CustomKotanPopUp : RemaningPoUp
{
  [SerializeField]
  private Kotan mKotan;

  [SerializeField]
  private Text KotanName;


  [SerializeField]
  private Text ConfirmDeleteWarning;

  [SerializeField]
  private Text NoCat1;
  [SerializeField]
  private Text NoCat2;

  [SerializeField]
  private Button LessBtn;
  [SerializeField]
  private Button MoreBtn;

  [SerializeField]
  private Button DeleteBtn;
  [SerializeField]
  private Button ShareBtn;


  private bool mDeleteConfirmationExpected = false;

  private int mCurrentSlot = 1;

  private const int FOUND_NOTHING = -1;

  //---------------------------------------------------------------------------------------------------------------
  protected override void Awake()
  {
    base.Awake();

    Game.Events.SwipeLeft.Listen(MoveLess);
    Game.Events.SwipeRight.Listen(MoveMore);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void CancelDeletion()
  {
    this.ConfirmDeleteWarning.gameObject.SetActive(false);
    this.mDeleteConfirmationExpected = false;
  }

  //---------------------------------------------------------------------------------------------------------------
  public sealed override void OnQuoteChangeFinished()
  {
    base.OnQuoteChangeFinished();
    SaveKotan();
  }

  //---------------------------------------------------------------------------------------------------------------
  public sealed override void OnChangeQuoteClicked()
  {
    base.OnChangeQuoteClicked();
    this.CancelDeletion();
  }

  //---------------------------------------------------------------------------------------------------------------
  public sealed override void OnRenameFinished()
  {
    base.OnRenameFinished();
    SaveKotan();
  }


  //---------------------------------------------------------------------------------------------------------------
  public sealed override void OnRenameClicked()
  {
    base.OnRenameClicked();
    this.CancelDeletion();
  }




  //---------------------------------------------------------------------------------------------------------------
  private void EmptyBook()
  {

    this.DeleteBtn.gameObject.SetActive(false);
    this.ShareBtn.gameObject.SetActive(false);
    this.KotanName.gameObject.SetActive(false);


    this.NoCat1.gameObject.SetActive(true);
    this.NoCat2.gameObject.SetActive(true);
    this.LessBtn.interactable = false;
    this.MoreBtn.interactable = false;

    this.DisableAndHideNamingFields();
    this.mKotan.TurnOff();
    this.CancelDeletion();
  }

  //---------------------------------------------------------------------------------------------------------------
  private void FindAndShowKotan()
  {
    this.NoCat1.gameObject.SetActive(false);
    this.NoCat2.gameObject.SetActive(false);
    this.CancelDeletion();

    if (!this.CustomKotanStorage().HasAtLeastOne())
    {
      this.EmptyBook();
      return;
    }

    this.mCurrentSlot = this.FindFilledSlotCloseToCurrent();
    if (this.mCurrentSlot == CustomKotanPopUp.FOUND_NOTHING)
    {
      Debug.LogError("Unexpected Error. Can't find cat inside a book, however book reported to have at least one.");
      this.EmptyBook();
      return;
    }

    this.myCurrentData = this.CustomKotanStorage().GetKotan(this.mCurrentSlot);

    if (this.myCurrentData == null)
    {
      Debug.Log("Unexpected Error. Can't find cat inside a book (got null), however book reported to have at least one.");
      this.EmptyBook();
      return;
    }

    this.mKotan.ActivateKotan(this.myCurrentData);
    this.PrepareNamingFields();

    this.mKotan.TurnOn();

  }
  //---------------------------------------------------------------------------------------------------------------
  public override void Activate(object data)
  {
    this.mCurrentSlot = 1;
    this.FindAndShowKotan();

    
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void CloseSelf()
  {
    this.CancelDeletion();
    base.CloseSelf();

  }
  //---------------------------------------------------------------------------------------------------------------
  protected sealed override void BeforeSharingStarts()
  {
    this.CancelDeletion();
  }


  //---------------------------------------------------------------------------------------------------------------
  private int FindFilledSlotCloseToCurrent()
  {
    if (!this.CustomKotanStorage().HasAtLeastOne())
    {
      return CustomKotanPopUp.FOUND_NOTHING;
    }

    int totalNums = this.CustomKotanStorage().AmountOfKotans();

    if (this.mCurrentSlot <= totalNums)
    {
      return System.Math.Max( this.mCurrentSlot, 1);
    }

    return System.Math.Min(totalNums, 1);
  }


  //---------------------------------------------------------------------------------------------------------------
  public void DeleteKotan()
  {
    if (!this.mDeleteConfirmationExpected)
    {
      this.mDeleteConfirmationExpected = true;
      this.ConfirmDeleteWarning.gameObject.SetActive(true);
      return;
    }

    this.CancelDeletion();

    this.CustomKotanStorage().DeleteKotan(this.myCurrentData);
    this.myCurrentData = null;
    this.FindAndShowKotan();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void MoveLess()
  {

    this.CancelDeletion();
    this.mCurrentSlot--;
    this.FindAndShowKotan();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void MoveMore()
  {
    this.CancelDeletion();
    this.mCurrentSlot++;
    this.FindAndShowKotan();

  }

  //---------------------------------------------------------------------------------------------------------------
  private CustomKotanStorage CustomKotanStorage()
  {
    if (Game.StateManager.CurrentState == GameState.Action)
    {
      return Game.ActionRoot.CustomKotanStorage;
    }

    if (Game.StateManager.CurrentState == GameState.Menu)
    {
      return Game.MenuRoot.CustomKotanStorage;
    }
    return null;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void SaveKotan()
  {
    this.CustomKotanStorage().PushKotan(this.myCurrentData);
    this.CustomKotanStorage().SaveArray();
  }

}
