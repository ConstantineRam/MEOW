//---------------------------------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderInsideBlock : CardHolderController
{

  [HideInInspector]
  public Side mySide;
  [HideInInspector]
  public int myPosition;
  [HideInInspector]
  public HolderBlock myHolder;

  private float myX1;
  private float myX2;
  private float myY1;
  private float myY2;

  private bool busyPushing = false;

  //---------------------------------------------------------------------------------------------------------------
  protected sealed override void Awake()
  {
    base.Awake();
    this.SetRefreshState = RefreshState.RefreshStopped;
    this.SetAllowToDisplaceCards = false;
    this.SetAutoReplenishCardsFromDeck = false;
    this.SetAllowGrabCards = false;
    this.SetAllowToAcceptCardDrop = true;
  }
  //---------------------------------------------------------------------------------------------------------------
  protected override void Start ()
  {
    base.Start();

    Game.Events.DragedOver.Listen(OnMouseEnter);
    Game.Events.DragStarted.Listen(CalculateMyCoords);

  }
  
		

  //---------------------------------------------------------------------------------------------------------------
  // Update is called once per frame
  protected override void Update ()
  {
    base.Update();	
	}

  //---------------------------------------------------------------------------------------------------------------
  public void CalculateMyCoords()
  {
    this.myX1 = this.GetRectTransform.position.x + (this.GetRectTransform.rect.xMin * Game.ActionRoot.CardManager.GetScaleFactor);
    this.myX2 = this.GetRectTransform.position.x + (this.GetRectTransform.rect.xMax * Game.ActionRoot.CardManager.GetScaleFactor);
    this.myY1 = this.GetRectTransform.position.y + (this.GetRectTransform.rect.yMin * Game.ActionRoot.CardManager.GetScaleFactor);
    this.myY2 = this.GetRectTransform.position.y + (this.GetRectTransform.rect.yMax * Game.ActionRoot.CardManager.GetScaleFactor);
  }

  //---------------------------------------------------------------------------------------------------------------
  void OnMouseEnter(float x, float y)
  {

    if ( this.myX1 > x)
    {
      return;
    }
    if ((this.myY1) > y)
    {
      return;
    }

    if ((this.myX2) < x)
    {
      return;
    }
    if ((this.myY2) < y)
    {
      return;
    }

    //
    ACard myCard = this.GetComponentInChildren<ACard>();
    if (myCard == null)
    {
      return;
    }

    this.DoPushing(myCard, this.mySide);


  }

  //---------------------------------------------------------------------------------------------------------------
  public bool DoPushing(ACard card, Side PushSide)
  {
    return this.DoPushing(card, false, false, PushSide);
  }


    //---------------------------------------------------------------------------------------------------------------
    public bool DoPushing(bool CleanUpPush = false)
  {
    ACard myCard = this.GetComponentInChildren<ACard>();
    if (myCard == null)
    {
      return false;
    }

    return this.DoPushing(myCard, true, CleanUpPush);
  }
  //---------------------------------------------------------------------------------------------------------------
  public bool DoPushing(ACard card, bool DeclineIfNotEmpty = false, bool CleanUpPush = false, Side PushSide = Side.Ignore)
  {
    this.busyPushing = true;
    bool result = this.myHolder.ProcessPush(card, mySide, PushSide, myPosition, DeclineIfNotEmpty, CleanUpPush);
    this.busyPushing = false;

    return result;
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool AskToAcceptPush(ACard newCard, bool DeclineIfNotEmpty = false)
  {
    if (this.busyPushing)
    {
      return false; // to prevent stack overflow of asking each other to push card if both neighbours have cards.
    }

    if (!this.HasCard) // its empty. can easily get the card.
    {
      if (this.myPosition != 0)
      {
        // if this holder isn't closest to the center it should try to send it there to prevent having gaps.
        if (this.DoPushing(newCard, this.mySide))
        {
          return true;
        }
        else
        { // if it not possible, this holder will take card.
          this.PushCard(newCard);
          return true;
        }
      }
      else
      {
        this.PushCard(newCard);
        return true;
      }

      
    }

    if (DeclineIfNotEmpty)
    {
      return false;
    }

    ACard cardToPush = this.GetComponentInChildren<ACard>();
    if (cardToPush == null)
    {
      Debug.LogError("Unexpected error. card to push dissapeared.");
      return false;
    }

    if (this.DoPushing (cardToPush, this.mySide))
    {
      return true; // processed by holder
    }

    return false;
  }

  //---------------------------------------------------------------------------------------------------------------
  protected sealed override void OnCardPushed()
  {
    base.OnCardPushed();


    if (this.GetCard.FreshCardInBlock)
    { // report to card Block that new card just landed.

      this.GetCard.FreshCardInBlock = false;
      this.myHolder.OnCardLanded(this);
    }

    
  }


}

public enum Side
{
  Left = 0,
  Right = 1,
  Ignore = 2


}
