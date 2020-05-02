using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniqueKotanPopUp : SharingPopUp
{
  [SerializeField]
  private Kotan myKotan;

  [SerializeField]
  private Button LessBtn;

  [SerializeField]
  private Button MoreBtn;

  [SerializeField]
  private Text CatName;

  [SerializeField]
  private Text CatQuote1;
  [SerializeField]
  private Text CatQuote2;
  [SerializeField]
  private Text CatQuote3;

  [SerializeField]
  private GameObject XCat;

  private int CurrentKotanPos = UniqueKotanStorage.NothingUnlocked;

  //---------------------------------------------------------------------------------------------------------------
  public class UniqueKotanPopupData
  {
    private int kotanCollectionNum = NO_DATA_SET;
    public int KotanCollectionNum { get { return kotanCollectionNum; } }

    public const int NO_DATA_SET = -1;

    public UniqueKotanPopupData(int UsedKotanCollectionNum)
    {
      this.kotanCollectionNum = UsedKotanCollectionNum;
       
    }


  }
  //---------------------------------------------------------------------------------------------------------------
  private UniqueKotanStorage GetUniqueKotanStorage()
  {
    if (Game.StateManager.CurrentState == GameState.Action)
    {
      return Game.ActionRoot.UniqueKotanStorage;
    }

    if (Game.StateManager.CurrentState == GameState.Menu)
    {
      return Game.MenuRoot.UniqueKotanStorage;
    }
    return null;
  }

  //---------------------------------------------------------------------------------------------------------------
  private void ShowKotan()
  {
    this.myKotan.TurnOn();
    this.LessBtn.interactable = false;
    this.MoreBtn.interactable = false;
    this.XCat.SetActive(false);

    if (this.CurrentKotanPos == UniqueKotanStorage.NothingUnlocked)
    {
      this.CloseSelf();
      return;
    }


    if (this.CurrentKotanPos > 1)
    {
      this.LessBtn.interactable = true;
    }

    if (this.CurrentKotanPos < GetUniqueKotanStorage().GetTopUnlockedKotanPosition())
    {
      this.MoreBtn.interactable = true;
    }


    KotanData KotanFromBook = GetUniqueKotanStorage().GetKotanData(this.CurrentKotanPos);
    if (KotanFromBook == null)
    {
      return;
    }

    //  Debug.Log("cats poo " + this.CurrentKotanPos);
    int kotanNum = GetUniqueKotanStorage().GetNumByPos(this.CurrentKotanPos);
    this.CatName.text = GetUniqueKotanStorage().GetNameNum(kotanNum);
    this.CatQuote1.text = GetUniqueKotanStorage().GetQuoteNum(kotanNum);
    this.myKotan.ActivateKotan(KotanFromBook);
    this.myKotan.TurnOn();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void MoveMore()
  {
    if (this.CurrentKotanPos >= GetUniqueKotanStorage().GetTopUnlockedKotanPosition())
    {
      return;
    }
    this.CurrentKotanPos++;
    this.ShowKotan();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void MoveLess ()
  {
    if (this.CurrentKotanPos <= 2)
    {
      return;
    }
    this.CurrentKotanPos--;
    this.ShowKotan();
  }

  //---------------------------------------------------------------------------------------------------------------
  private void NoKotansUnlocked ()
  {
    this.XCat.SetActive(true);
    this.myKotan.TurnOff();
    this.CatName.text = "MEOW!";
    CatQuote1.supportRichText = true;
    this.CatQuote1.text = "Get at least <color=#01458C>" + GetUniqueKotanStorage().FirstKotanCost().ToString() + "</color> score and";
    this.CatQuote2.text = "successfully finish one game to ";
    this.CatQuote3.text = "unlock first Cat Friend!";
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void Activate(object data)
  {
    if (data == null)
    {
      this.CloseSelf();
      return;
    }
    base.Activate(data);

    UniqueKotanPopupData myData = (UniqueKotanPopupData) data;



    this.LessBtn.interactable = false;
    this.MoreBtn.interactable = false;
    this.XCat.SetActive(false);

    int KotanPosition = myData.KotanCollectionNum;

    if (myData.KotanCollectionNum == UniqueKotanPopUp.UniqueKotanPopupData.NO_DATA_SET)
    {
      KotanPosition = GetUniqueKotanStorage().GetTopUnlockedKotanPosition();
      if (KotanPosition == UniqueKotanStorage.NothingUnlocked)
      {
        this.NoKotansUnlocked();
        return;
      }
    }


    
    

    this.CatName.text = "MEOW!";
    this.CatQuote1.text = "";
    this.CurrentKotanPos = KotanPosition;
    this.ShowKotan();
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void CloseSelf()
  {
    base.CloseSelf();

    if (!this.HasCallback)
    {
      Game.AudioManager.PlaySound(AudioId.BookClose);
    }
  }


}
