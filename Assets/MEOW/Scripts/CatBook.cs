using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CatBook : GenericPopup
{
  [SerializeField]
  private float columnExtra;

  [SerializeField]
  private float rowExtra;

  [SerializeField]
  private RectTransform Content;


  [SerializeField]
  private Sprite UniqueKotanBack;

  [SerializeField]
  private Sprite StandardKotanBack;

  [SerializeField]
  private Sprite LockedBack;

  [SerializeField]
  private Sprite UniqueLockedBack;

  [SerializeField]
  private GameObject TutorialObject;

  private KotanBtn LastKotan;

  private bool Column = false;
  private int Row = 1;
  public const int STANDARD = -1;


  #region Fill with Cats
  //--------------------------------------------------------------------------------------------------------------
  private void FillWithEmptySlots()
  {
    int remainingSlots = this.CustomKotanStorage().MaxSlots - this.CustomKotanStorage().AmountOfKotans();
    if (remainingSlots < 1)
    {
      return;
    }
    for (int i = 0; i < remainingSlots; i++)
    {
      this.PushEmptySlot();
    }
  }
  //--------------------------------------------------------------------------------------------------------------
  private void FillWithLockedSlots()
  {
    if (Game.Settings.IsPremium)
    {
      return;
    }

    int remainingSlots = (this.CustomKotanStorage().PremiumAmount - this.CustomKotanStorage().AmountOfKotans() ) - this.CustomKotanStorage().FremiumAmount;
    if (remainingSlots < 1)
    {
      return;
    }
    for (int i = 0; i < remainingSlots; i++)
    {
      this.PushLockedSlot();
    }
  }
  //--------------------------------------------------------------------------------------------------------------
  private void FillWithCustomCats()
  {
    if (!this.CustomKotanStorage().HasAtLeastOne())
    {
      return;
    }

    for (int i = 1; i < this.CustomKotanStorage().AmountOfKotans() + 1; i++)
    {
      CustomKotamData KotanData = this.CustomKotanStorage().GetKotan(i);
      if (KotanData == null)
      {
        Debug.LogError("Null at Kotan Data for Cat book. ("+ i+ ")");
        continue;
      }

      KotanBtn KotanBtn = this.PushKotan(KotanData, KotanData.GetName, this.StandardKotanBack);
      if (KotanBtn != null)
      {
        KotanBtn.CustomKotamData = KotanData;
      }
    }

    

  }
  //--------------------------------------------------------------------------------------------------------------
  public void OnSwitchMagicCats()
  {
    Game.Settings.CatBookMagicCatsEnabled = !Game.Settings.CatBookMagicCatsEnabled;
    Game.UiManager.Open(PopupId.CatBook);
  }
  //--------------------------------------------------------------------------------------------------------------
  private void PushEmptySlot()
  {
    this.PushKotan(null, "empty", this.StandardKotanBack, true);
  }

  //--------------------------------------------------------------------------------------------------------------
  private void PushLockedSlot()
  {
    this.PushKotan(null, "Buy Game", this.LockedBack, true);
  }

  //--------------------------------------------------------------------------------------------------------------
  private KotanBtn PushKotan(KotanData kotanData, string CatName, Sprite BackSprite, bool MakeLocked = false, int UniqueNum = STANDARD)
  {

    KotanBtn kotan = (KotanBtn) Game.PoolManager.Pop(ObjectPoolName.ACatBtn, this.Content);

    if (kotan == null)
    {
      Debug.LogError("Cat Book, can't pool kotan.");
      return null;
    }
    kotan.RectTransform.localScale = Vector3.one;
    kotan.UniqueKotanNum = UniqueNum;
    kotan.CatName = CatName;

    if (kotanData != null)
    {
      kotan.ActivateKotan(kotanData);
      kotan.TurnOn();
    }
    
    kotan.SetBackSprite = BackSprite;
    if (this.Column)
    {
      kotan.RectTransform.anchoredPosition = new Vector3((kotan.RectTransform.sizeDelta.x / 2 + this.columnExtra) +20, -1 * kotan.RectTransform.sizeDelta.y * this.Row - (this.rowExtra * (this.Row - 1)   + (this.Row * 10)) );
      this.Row++;
    }
    else
    {
      kotan.RectTransform.anchoredPosition = new Vector3((kotan.RectTransform.sizeDelta.x / 2 + this.columnExtra) * -1 - 80, -1 * kotan.RectTransform.sizeDelta.y * this.Row - (this.rowExtra * (this.Row - 1) + (this.Row * 10)));
    }

    this.Column = !this.Column;

    if (MakeLocked)
    {
      kotan.Locked = true;

      if (kotan.HasKotanData)
      {
        Debug.LogError("Cat Book got Cat btn to be set as Locked, however has a cat inside.");
      }
    }

    this.LastKotan = kotan;

    return kotan;
  }

  //--------------------------------------------------------------------------------------------------------------
  private void EnlargeScaleArea()
  {
    if (this.LastKotan == null)
    {
      Debug.LogError("cat book has no Cat btns!");
      return;
    }

    this.Content.sizeDelta = new Vector2(this.Content.sizeDelta.x, Math.Abs(this.LastKotan.RectTransform.localPosition.y ) );
  }

  //--------------------------------------------------------------------------------------------------------------
  private void FillWithUniqueCats()
  {
    if (!Game.Settings.CatBookMagicCatsEnabled)
    {
      return;
    }

    //if (!this.UniqueKotanStorage().AtLeastOneUnlocked())
    //{
    //  return;
    //}

    for (int CatNum = 1; CatNum < this.UniqueKotanStorage().MaxAmount(); CatNum++)
    {
      if (!this.UniqueKotanStorage().isUnlocked(CatNum))
      {
        this.PushKotan(null, "" + this.UniqueKotanStorage().Cost(CatNum), this.UniqueLockedBack, true);
        continue;
      }

      KotanData kotanData = this.UniqueKotanStorage().GetKotanData(CatNum);
      string CatName = this.UniqueKotanStorage().GetNameNum(this.UniqueKotanStorage().GetNumByPos(CatNum));
      Debug.Log(CatName);
      this.PushKotan(kotanData, CatName, this.UniqueKotanBack, false, CatNum);
    }

    //for (int CatNum = 1; CatNum < this.UniqueKotanStorage().GetTopUnlockedKotanPosition()+1; CatNum++)
    //{
    //  KotanData kotanData = this.UniqueKotanStorage().GetKotanData(CatNum);
    //  string CatName = this.UniqueKotanStorage().GetNameNum(this.UniqueKotanStorage().GetNumByPos(CatNum));
    //  Debug.Log(CatName);
    //  this.PushKotan(kotanData, CatName, this.UniqueKotanBack, false, CatNum);
    //}
  }
  //--------------------------------------------------------------------------------------------------------------
  private void FillWithCats()
  {
    this.FillWithUniqueCats();
    this.FillWithCustomCats();
    FillWithEmptySlots();
    this.FillWithLockedSlots();

    this.EnlargeScaleArea();
  }

  #endregion

  #region Generic PopUp Inherited
  //--------------------------------------------------------------------------------------------------------------
  public sealed override void Activate(object data)
  {
    if (Game.Settings.CatBookHelpShown)
    {
      this.TutorialObject.SetActive(false);
    }
    else
    {
      Game.Settings.CatBookHelpShown = true;
    }

    this.FillWithCats();
  }

  //--------------------------------------------------------------------------------------------------------------
  public sealed override void CloseSelf()
  {
    Game.AudioManager.PlaySound(AudioId.BookClose);
    base.CloseSelf();

  }
  //---------------------------------------------------------------------------------------------------------------
  public sealed override void OnClose()
  {
    KotanBtn[] btns = this.Content.GetComponentsInChildren<KotanBtn>();

    if (btns.Length < 1)
    {
      Debug.Log("Cat book has no Cat btns!");
      base.CloseSelf();
      return;
    }


    foreach (KotanBtn btn in btns)
    {
      btn.transform.SetParent(null);
      btn.ReturnToPool();
    }

    base.OnClose();

  }



  #endregion

  #region Storage Access
  //--------------------------------------------------------------------------------------------------------------
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

  //--------------------------------------------------------------------------------------------------------------
  private UniqueKotanStorage UniqueKotanStorage()
  {
    if (Game.StateManager.CurrentState == GameState.Menu)
    {
      return Game.MenuRoot.UniqueKotanStorage;
    }

    if (Game.StateManager.CurrentState == GameState.Action)
    {
      return Game.ActionRoot.UniqueKotanStorage;
    }
    return null;
  }

  #endregion
}
