using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Deck : CardParent
{
  [Tooltip("Deck sends its size to this Text field. *Could be left empy*")]
  [SerializeField]
  private Text DeckSizeReport;


  private Stack<ACard> cards;
  private int cardsCounter = 0; // used to create a twin with card pile

  

  void Awake ()
  {
    base.Awake();
    cards = new Stack<ACard>();

  }

  //---------------------------------------------------------------------------------------------------------------
  private CardManager OurManager()
  {
    return Game.ActionRoot.CardManager;
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void OnLeftParent(ACard leftCard)
  {
    this.cardsCounter--;
    this.ReportCardsAmount();
  
  }

  //---------------------------------------------------------------------------------------------------------------
  public void DestroyAllCard()
  {
    ACard cardToDestroy = this.PullCard();
    while (cardToDestroy != null)
    {
      cardToDestroy.ReturnToPool();
      cardToDestroy = this.PullCard();
    }
    this.cardsCounter = 0;
  }

  //---------------------------------------------------------------------------------------------------------------
  public int CardsAmount()
  {
    return this.cards.Count;
  }


  //---------------------------------------------------------------------------------------------------------------
  private void ReportCardsAmount()
  {
    if (this.DeckSizeReport == null)
    {
      return;
    }

    this.DeckSizeReport.text = this.CardsAmount().ToString();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void PushCard(ACard newCard, bool noFlight = false)
  {
    this.cardsCounter++;
    this.ReportCardsAmount();

    if (newCard == null)
    {
      Debug.Log("Deck got null at card data.");
      return;
    }
  
    if (newCard.GetCardState == CardState.FaceUp)
    {
      newCard.Flip();
    }


    this.cards.Push(newCard);
    if (noFlight)
    {

      newCard.SetParent(this);
      newCard.MoveToParent(true);
      if (this.OurManager().GetAllowArtisticRotation)
      {
        newCard.ChangeArtisticRotation();
      }
      newCard.ArtisticPilePush(cardsCounter);
    }
    else
    {
      newCard.SetParent(this);
      newCard.MoveToParent();
      Game.TimerManager.Start(this.OurManager().CardFlyTime + 0.01f, () =>
      {
        if (this.OurManager().GetAllowArtisticRotation)
        {
          newCard.ChangeArtisticRotation();
        }
        newCard.ArtisticPilePush(cardsCounter);
        //LayoutRebuilder.ForceRebuildLayoutImmediate();
      });
    }
  

  }

  //---------------------------------------------------------------------------------------------------------------
  public ACard PullCard()
  {
    if (this.cards.Count == 0)
    {
      return null;
    }

    return this.cards.Pop();
    //return this.GetComponentInChildren<ACard>();
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool HasAtLeastOneCard()
  {
    if (this.cards.Count == 0)
    {
      return false;
    }

    return true;
  }

  //---------------------------------------------------------------------------------------------------------------

  public bool HasCardType(CardTypes checkType)
  {
    if (!this.HasAtLeastOneCard())
    {
      return false;
    }

    foreach (ACard card in this.cards)
    {
      if (card.GetCardType() == checkType)
      {
        return true;
      }
    }

    return false;
  }
}
