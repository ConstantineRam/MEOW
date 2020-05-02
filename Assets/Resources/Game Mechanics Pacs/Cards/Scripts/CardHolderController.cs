using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class CardHolderController : CardParent, IDropHandler
{
  [Tooltip("If true this holder will accept card drop.")]
  [SerializeField]
  private bool allowToAcceptCardDrop = true;
  public bool AllowToAcceptCardDrop { get { return allowToAcceptCardDrop; } }
  public bool SetAllowToAcceptCardDrop { set { allowToAcceptCardDrop = value; } }
  [Tooltip("if true new card dropped into this holder will replace existing. Old card will be disposed via Card Disposer. Dispose has priority over store in a deck.")]
  [SerializeField]
  private bool allowToDisplaceCards = false;
  public bool AllowToDisplaceCards { get { return allowToDisplaceCards; } }
  protected bool SetAllowToDisplaceCards { set{ allowToDisplaceCards = value; } }
  [SerializeField]
  [Tooltip("If true Player can grab cards.")]
  private bool allowGrabCards = true;
  public bool AllowGrabCards { get { return allowGrabCards; } }
  protected bool SetAllowGrabCards { set { allowGrabCards = value; } }

  [SerializeField]
  [Tooltip("If true cardholder will automatically flip top card one it hasn't enough.")]
  private bool AutoReplenishCardsFromDeck = true;
  protected bool SetAutoReplenishCardsFromDeck { set { AutoReplenishCardsFromDeck = value; } }
  [SerializeField]
  [Tooltip("Initial delay until holder will start refreshing self after the scene launch. Automatically or not.")]
  private float InitialRefreshDelay = 3f;
  [SerializeField]
  [Tooltip("Ingame refresh delay beetween check for refresh by cardholder.")]
  private float IngameRefreshDelay = 1.2f;
  [SerializeField]
  [Tooltip("If set to refresh once, card holder will pull cards once and stop.")]
  private RefreshState myRefreshState = RefreshState.RefreshStopped;
  public RefreshState GetRefreshState { get { return myRefreshState; } }
  public RefreshState SetRefreshState { set { myRefreshState = value; } }

  [Tooltip("Add a deck to this holder to allow it replenish self from it.")]
  [SerializeField]
  private Deck MyDeck;
  public Deck GetDeck { get { return MyDeck; } }

  [Tooltip("Holder sends the size of its deck attached and its own card to this Text field. *Could be left empy*")]
  [SerializeField]
  private Text CardsSizeReport;

  // NOTE: initially it was planned for holder to have more than 1 card. However, later on I found out its a bad idea and its way better to have 1 card per holder.
  //[Tooltip("Hand size detrrmine an amout of cards Card Holder will try to maintain by pulling cards from the deck if it has one AND/OR will reject card drop if has over a hnd limit.")]
  //[SerializeField]
  //private int handSize = 1; // one is enough.
  //public int HandSize { get { return handSize; } }

  [SerializeField]
  [Tooltip("If true all cards this holder is moved to its deck if it has one.")]
  private bool storeInputIntoDeck = false;
  public bool StoreInputIntoDeck { get { return storeInputIntoDeck; } }

  [Tooltip("Affects 'Store Input Into Deck' If true sends cards it receives to deck only if hand is full (has a card).")]
  [SerializeField]
  private bool storeRespectHandSize = false;
  public bool StoreRespectHandSize { get { return storeRespectHandSize; } }

  [Tooltip("If has no card activate this back. (disabled)")] 
  [SerializeField]
  private GameObject backObject;

  // determines which delay we use. initial or ingame.
  private bool initialCheck = true;
  private bool AllowRefreshCheck = false;

  //[HideInInspector]


  //-- Internal variables --
  private ACard myCard;
  private ACard SetCard { set { myCard = value; } }
  public ACard GetCard { get { return myCard; } }
  public bool HasCard { get { if (GetCard == null) { return false; } return true; } }


  private Image MyBack;

  #region Unity Basics
  //---------------------------------------------------------------------------------------------------------------
  protected override void Awake()
  {
    base.Awake();

    if (this.backObject != null)
    {
   //   this.backObject.SetActive(false);
    }


    if (this.StoreInputIntoDeck)
    {
      if (!this.HasDeck())
      {
        Debug.LogError("Cardholder "+this.name + " was set store input into Deck, but wasn't assigned one. Switched Store Input Into Deck to FALSE.");
        this.storeInputIntoDeck = false;
      }
    }

  }

  //---------------------------------------------------------------------------------------------------------------
  private void ReportCardsAmount()
  {
    if (this.CardsSizeReport == null)
    {
      return;
    }

    int cardsAmount = 0;
    if (this.HasCard)
    {
      cardsAmount++;
    }

    if (this.HasDeck())
    {
      cardsAmount = cardsAmount + this.GetDeck.CardsAmount();
    }

    if (cardsAmount == 0)
    {
      this.CardsSizeReport.text = "";
      return;
    }
    this.CardsSizeReport.text = cardsAmount.ToString();
  }


  //---------------------------------------------------------------------------------------------------------------
  protected virtual void Start()
  {
    Game.TimerManager.Start(this.InitialRefreshDelay, () =>
    {
      this.AllowRefreshCheck = true;
    });


  }

  //---------------------------------------------------------------------------------------------------------------
  protected virtual void Update()
  {
    this.OnUpdate();

    this.ReportCardsAmount();

    if (!this.HasDeck())
    {
      return; // no need to check if don't have deck. All Updates are deck related.
    }


    if (!this.AllowRefreshCheck)
    {
      return;
    }

    if (this.myRefreshState == RefreshState.RefreshStopped)
    {
      return;
    }


    if (this.HasCard)
    {
      return;
    }

    if (!this.GetDeck.HasAtLeastOneCard())
    {
      return; 
    }

    ReplenishSelfFromDeck();
  }

  #endregion
  //---------------------------------------------------------------------------------------------------------------
  public bool HolderAndDeckHasCard()
  {

    if (this.HasCard)
    {
      return true;
    }

    if (!this.HasDeck())
    {
      return false;
    }

    if (this.GetDeck.HasAtLeastOneCard())
    {
      return true;
    }

    return false;
  }
  //---------------------------------------------------------------------------------------------------------------
  public override void OnLeftParent(ACard leftCard)
  {
    if (leftCard == this.GetCard)
    {
      this.SetCard = null;
      return;
    }

    if (this.GetCard == null)
    {
      Debug.LogError("Error card " + leftCard.name + " reported OnLeftParent to cardholder " + this.name + ", but its not a child of this card holder. Holder child is null.");
    }
    else
    {
      Debug.LogError("Error card " + leftCard.name + " reported OnLeftParent to cardholder " + this.name + ", but its not a child of this card holder. Holder child is " + this.GetCard.name);
    }
    
  }

  //---------------------------------------------------------------------------------------------------------------
  protected virtual void OnCardPushed()
  {

  }

  //---------------------------------------------------------------------------------------------------------------
  private void CheckBack()
  {
    return;

    if (this.backObject == null)
    {
      return;
    }

    if (!this.HasCard)
    {
      this.backObject.SetActive(true);
      return;
    }

    this.backObject.SetActive(false);
  }

  //---------------------------------------------------------------------------------------------------------------
  protected virtual void OnUpdate()
  {
  }

    //---------------------------------------------------------------------------------------------------------------
    private bool StoreCardIntoDeck(ACard storedCard)
  {
    if (storedCard == null)
    {
      return false;
    }

    if (this.StoreRespectHandSize)
    {

      if (!this.HasCard)
      {
        return false;
      }
    }

    if (!this.StoreInputIntoDeck)
    {
      return false;
    }

    if (!this.HasDeck())
    {
      return false;
    }

    this.MyDeck.PushCard(storedCard);
    return true;

  }

  //---------------------------------------------------------------------------------------------------------------
  private void ReplenishSelfFromDeck()
  {
    if (this.HasCard)
    {
      return;
    }

    if (!this.HasDeck())
    {
      return;
    }
    this.AllowRefreshCheck = false;


    ACard newCard = this.MyDeck.PullCard();

    if (newCard == null)
    {
       Debug.LogError("ReplenishSelfFromDeck; Unexpected Error. can't get card. "+this.name+".");
    }

    this.PushCard(newCard);

    if (newCard.GetCardState != CardState.FaceUp)
    {
      newCard.Flip();
    }


    if (this.GetRefreshState == RefreshState.RefreshSelfOnce)
    {
      this.myRefreshState = RefreshState.RefreshStopped;
    }

    Game.TimerManager.Start(this.IngameRefreshDelay, () =>
    {
      this.AllowRefreshCheck = true;
    });

  }



  //---------------------------------------------------------------------------------------------------------------
  public void AddCardsToDeck(List<CardTypes> newSet)
  {
    if (!this.HasDeck())
    {
      Debug.LogError("Card Holder " + this.name + " got new cards to add into deck, but cardholder has no deck.");
      return;
    }

    if (newSet.Count  == 0)
    {
      //its fine. we add cards with delay. at some point they may run out.
      return;
    }


    CardTypes newCardType = newSet.First();
    this.OurManager().CreateACard(newCardType, this.MyDeck);
    
    newSet.Remove(newCardType);
    
    Game.TimerManager.Start(0.05f, () =>
    {
      this.AddCardsToDeck(newSet);
    });

  }

  //---------------------------------------------------------------------------------------------------------------
  public void DestroyAllCards()
  {
    if (this.HasDeck())
    {
      this.GetDeck.DestroyAllCard();
    }

    if (this.HasCard)
    {
      this.GetCard.ReturnToPool();
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public void PushCardsTo(CardHolderController newHolder)
  {
    if (newHolder == null)
    {
      return;
    }

    ACard[] checkCards = this.GetComponentsInChildren<ACard>();
    if (checkCards.Length < 1)
    {
      return;
    }

    foreach (ACard card in checkCards)
    {
      newHolder.PushCard(card);
    }
  }

  ////---------------------------------------------------------------------------------------------------------------
  //public bool HasEnoughCards()
  //{
  //  if (this.HandSize == 1)
  //  {
  //    if (this.GetComponentInChildren<ACard>() == null)
  //    {
  //      return false;
  //    }

  //    return true;
  //  }

  //  ACard[] checkCards = this.GetComponentsInChildren<ACard>();
  //  if (checkCards.Length < this.HandSize)
  //  {
  //    return false;
  //  }

  //  return true;
  //}
  //---------------------------------------------------------------------------------------------------------------
  private CardManager OurManager()
  {
    return Game.ActionRoot.CardManager;
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool HasDeck()
  {
    if (this.GetDeck == null)
    {
      return false;
    }

    return true;
  }



  //---------------------------------------------------------------------------------------------------------------
  public void Refresh()
  {
    LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetRectTransform);
  }


  //---------------------------------------------------------------------------------------------------------------
  public void MoveCardsTo(CardHolderController newHolder)
  {
  }

  //---------------------------------------------------------------------------------------------------------------
  public void RefreshOnce()
  {
    if (this.myRefreshState == RefreshState.Auto)
    {
      return;
    }

    this.myRefreshState = RefreshState.RefreshSelfOnce;
  }

  //---------------------------------------------------------------------------------------------------------------
  private bool canReceiveThisDrop(GameObject checkedObject)
  {
    if (!this.AllowToAcceptCardDrop)
    {
      return false;
    }
    if (checkedObject == null)
    {
      return false;
    }

    if (checkedObject.transform.IsChildOf(this.transform))
    {
      // Already my child. Reject drop to return to status quo.
      return false;
    }

    ACard checkCard = checkedObject.GetComponent<ACard>();

    if (checkCard == null)
    {
      // We received wrong drop at all. It's not a card.
      return false;
    }

    if (this.HasCard)
    {
      if (!this.AllowToDisplaceCards) // displace isn't allowed
      {

        if (!StoreInputIntoDeck)
        { // already have card and don't save extra them into deck
          return false;
        }


      }
    }
    return true;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OnDrop(PointerEventData data)
  {

    if (data.pointerDrag != null)
    {
      if (!this.canReceiveThisDrop(data.pointerDrag))
      {
        return;
      }
            
      ACard checkCard = data.pointerDrag.GetComponent<ACard>();
      if (checkCard != null)
      {
        this.PushCard(checkCard);
      }
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public void PushCard(ACard newCard, bool ForcePosition = false)
  {
    if (newCard == null)
    {
      Debug.LogError("Cardholder (" + this.name + ") got new card to push, but its null");
      return;
    }

    if (this.HasCard)
    {
      if (this.AllowToDisplaceCards)
      {
        Game.ActionRoot.CardManager.DisposeCard(this.GetCard);
      }
      else
      {
        if (this.StoreInputIntoDeck)
        {
          if (!this.StoreCardIntoDeck(this.GetCard))
          {
            Debug.Log("Cardholder " + this.name + " attempted to return card deck, but couldn't. Card is to be destroyed.");
            newCard.ReturnToPool();
          }

        }
      }

    }

    this.SetCard = newCard;

    newCard.EnableGrab(this.allowGrabCards);


    newCard.SetParent(this);

    
    newCard.MoveToParent(ForcePosition);
    this.OnCardPushed();

  }




}

//---------------------------------------------------------------------------------------------------------------
public enum RefreshState
{
  [Description("Card holder will refresh self automatically.")]
  Auto = 0,
  [Description("Card holder will refresh self once and then switch to RefreshStopped")]
  RefreshSelfOnce = 1,
  [Description("Card holder will not refresh self.")]
  RefreshStopped = 2
}
