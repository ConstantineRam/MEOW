using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuDeck : GenericPopup
{
  [SerializeField]
  private RectTransform Content;

  [SerializeField]
  private float rowExtra;
  private int Row = 1;

  private RectTransform LastObject;

  //--------------------------------------------------------------------------------------------------------------
  private void ProcessMagicCat(int CatNum)
  {

    if (Game.MenuRoot.UniqueKotanStorage.HasLevelBonusInc(CatNum))
    {
      CardBtn card = (CardBtn) Game.PoolManager.Pop(ObjectPoolName.ACardBtn, this.Content);
      card.MakeBonusInc(CatNum);
      card.RectTransform.localScale = Vector3.one;

      card.RectTransform.anchoredPosition = new Vector3((card.RectTransform.sizeDelta.x / 2 * -1 + (card.RectTransform.sizeDelta.x / 2)), -1 * card.RectTransform.sizeDelta.y * this.Row); //* - (this.rowExtra * (this.Row - 1))
      this.Row++;
      this.LastObject = card.RectTransform;
    }

    List<ACAtCardData> LevelCards = GameData.CardStorage.GetDataForMagicCatLevel(CatNum);
    if (LevelCards == null)
    {
      return;
    }

    if (LevelCards.Count == 0)
    {
     // Debug.LogError("No cards for "+ CatNum);
      return;
    }

    foreach (ACAtCardData CatCard in LevelCards)
    {
      CardBtn card = (CardBtn) Game.PoolManager.Pop(ObjectPoolName.ACardBtn, this.Content);
      card.SetCard(CatCard);
      card.RectTransform.localScale = Vector3.one;

      card.RectTransform.anchoredPosition = new Vector3((card.RectTransform.sizeDelta.x /2 * -1 + (card.RectTransform.sizeDelta.x / 2)), -1 * card.RectTransform.sizeDelta.y * this.Row  ); //* - (this.rowExtra * (this.Row - 1))
      this.Row++;
      this.LastObject = card.RectTransform;
    }
  }
  //--------------------------------------------------------------------------------------------------------------
  private void EnlargeScaleArea()
  {
    if (this.LastObject == null)
    {
      Debug.LogError("cat book has no Cat btns!");
      return;
    }

    this.Content.sizeDelta = new Vector2(this.Content.sizeDelta.x, Math.Abs(this.LastObject.anchoredPosition.y) +120);
  }
  //--------------------------------------------------------------------------------------------------------------
  private void FillWithBtns()
  {
    this.Row = 1; 
    for (int i = 0; i < Game.MenuRoot.UniqueKotanStorage.MaxAmount();i++ ) //-1 if we want to show starting hand.
    {
      this.ProcessMagicCat(i);
    }
  }

  #region Generic PopUp Inherited
  //--------------------------------------------------------------------------------------------------------------
  public sealed override void Activate(object data)
  {
    this.FillWithBtns();
    this.EnlargeScaleArea();
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
    APoolable[] btns = this.Content.GetComponentsInChildren<APoolable>();

    if (btns.Length < 1)
    {
      Debug.Log("MenuDeck has no Cat btns!");
      base.CloseSelf();
      return;
    }


    foreach (APoolable btn in btns)
    {
      btn.transform.SetParent(null);
      btn.ReturnToPool();
    }

    base.OnClose();

  }



  #endregion
}
