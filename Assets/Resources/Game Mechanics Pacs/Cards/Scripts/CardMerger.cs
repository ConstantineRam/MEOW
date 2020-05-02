using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------
// This cardholder acts as special effect object with game mechanics elements.
// Merger has other cardholders as child holders and will wait until they fill to fire up it's feature.
// You can use it to:
// a. send card(s) here to destroy them in puff.
// b. a + turn them into another cards than will be send to another Card Holder.
//---------------------------------------------------------------------------------------------------------------
public class CardMerger : CardHolderController
{

  [Tooltip("If true. Merger will wait for OnStartMerge to be called to fire up. Ignoring holders state.")]
  [SerializeField]
  private bool WaitForCommand;
  [Tooltip("Merger will fire up its feature WHEN all holders will have cards.")]
  [SerializeField]
  private CardHolderController[] MyHolders;
  //[Tooltip("If holder block is set, merger will send it message to stop processing cards if merging starts. *Could be empty.*")]
  //[SerializeField]
  //private HolderBlock MyHolderBlock;
  //private bool HasHolderBlock {get { if (MyHolderBlock == null) { return false; } return true; }}

  [SerializeField]
  private float CheckForMergeDelay = 0;
  [Tooltip("The delay beetween separate cards to begin rotation and miniaturisation.")]
  [SerializeField]
  private float CardRotationDelay = 0.05f;
  [SerializeField]
  private Vector3 CardRotation;
  [Tooltip("The Card Rotation time. Cards will start shrink at 50% of it and will be des.")]
  [SerializeField]
  private float CardRotationTime = 3f;
  private float CardStartToShringTime;
  private Vector3 ShringResult;
  private Vector3 PoofScale;

  [Tooltip("Attach an child that will be a POOF special effect for merging. It should have RectTransform.")]
  [SerializeField]
  private GameObject Poof;
  protected bool HasPoof { get { if (this.Poof == null) { return false; } return true; } }
  private RectTransform PoofRectTransform;
  private bool CanCheck = true;
  private bool MergeInProcess = false;

  //---------------------------------------------------------------------------------------------------------------
  protected override void Awake()
  {
    base.Awake();
    this.SetRefreshState = RefreshState.RefreshStopped;
    this.SetAllowToDisplaceCards = false;
    this.SetAutoReplenishCardsFromDeck = false;
    this.SetAllowGrabCards = false;
    this.SetAllowToAcceptCardDrop = false;

    if (this.HasPoof)
    {
      this.PoofRectTransform = Poof.GetComponent<RectTransform>();
      this.Poof.SetActive(false);
    }
    
    

    this.CardStartToShringTime = this.CardRotationTime / 2;
    this.ShringResult = new Vector3(0, 0, 0);
    this.PoofScale = new Vector3(10, 10, 10);

    if (this.HasDeck())
    {
      Debug.LogError("Merger holder was assigned a deck!");
    }

    if (this.StoreInputIntoDeck)
    {
      Debug.LogError("Merger holder got Store Input Into Deck rule set TRUE.");
    }

    if (this.MyHolders.Length == 0)
    {
      Debug.LogError("Card merger "+name+" has no holders assigned!");
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void OnLeftParent(ACard leftCard)
  {
    // Card merger don't care.
  }

  //---------------------------------------------------------------------------------------------------------------
  protected override void Update()
  {
    // We don't run base Update.
    if (this.WaitForCommand)
    {
      return;
    }
    
    if (!this.CanCheck)
    {
      return;
    }

    if (this.MergeInProcess)
    {
      return;
    }

    this.CanCheck = false;

    Game.TimerManager.Start(this.CheckForMergeDelay, () =>
    {
      this.CanCheck = true;
    });

    if (!this.MyHoldersGotCards())
    {
      return;
    }



    StartMerge();
  }

  //---------------------------------------------------------------------------------------------------------------
  private bool MyHoldersGotCards()
  {
    
    for (int i = 0; i < this.MyHolders.Length; i++)
    {
      if (!this.MyHolders[i].HasCard)
      {
        return false;
      }
    }

    return true;
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Call this to start merge manually;
  /// </summary>
  public void OnStartMerge()
  {
    if (!this.WaitForCommand)
    {
      Debug.LogError("Manual merge was called, however merger has WaitForCommand == false. Command ignored.");
      return;
    }

    this.StartMerge();
  }

  //---------------------------------------------------------------------------------------------------------------
  private void StartMerge()
  {

    this.MergeInProcess = true;
    Game.Events.OnCardMergeStarted.Invoke();
    for (int i = 0; i < this.MyHolders.Length; i++)
    {
      ACard card = this.MyHolders[i].GetCard;
      if (card == null)
      {
        if (!this.WaitForCommand)
        {
          Debug.LogError("unexpected error. Automatic merge started, but one of the holders lacks card.");
        }
        continue;
      }
      card.SetParent(this);
      card.MoveToParent();

    }

    this.MergeRoutines();
  }
  //---------------------------------------------------------------------------------------------------------------
  private void RotateCards(ACard[] cards, int position = 0)
  {

    if (position == cards.Length)
    {
      return;
    }
    ACard card = cards[position];
    
    card.GetCardRectTransform.DOPunchRotation(CardRotation, this.CardRotationTime, 11);
    Game.TimerManager.Start(this.CardStartToShringTime, () =>
    {
      card.GetCardRectTransform.DOScale (this.ShringResult, this.CardStartToShringTime).SetAutoKill();
    });

    int newPosition = position + 1;
    Game.TimerManager.Start(this.CardRotationDelay, () =>
    {
      this.RotateCards(cards, newPosition);
    });

  }

  //---------------------------------------------------------------------------------------------------------------
  private void MergeRoutines()
  {
    ACard[] cards = this.GetComponentsInChildren<ACard>();
    if (cards.Length < 1)
    {
      if (!this.WaitForCommand)
      {
        Debug.LogError("Unexpected Error. Automatic card merge was started (" + this.name + "), but no cards we found, despite check was passed before.");
      }

    }
    else
    {
      this.RotateCards(cards);
    }
      

    Game.TimerManager.Start(this.CardStartToShringTime, () =>
    {
      if (this.HasPoof)
      {
        this.Poof.SetActive(true);
        this.PoofRectTransform.DOScale(this.ShringResult, 0.001f);
        this.PoofRectTransform.SetAsLastSibling();
        this.PoofRectTransform.DOScale(this.PoofScale, this.CardStartToShringTime);
      }
      
      Game.TimerManager.Start(this.CardStartToShringTime, () =>
      {
        foreach (ACard card in cards)
        {
          DOTween.Complete(card.GetCardRectTransform);
          card.ReturnToPool();
        }

        if (this.HasPoof)
        {
          this.PoofRectTransform.DOScale(this.ShringResult, 0.4f);
          Game.TimerManager.Start(this.CardStartToShringTime, () =>
          {
            DOTween.Complete(this.PoofRectTransform);
            this.Poof.SetActive(false);
            this.FinalizeMerge();

          });
        }
        else
        {
          this.FinalizeMerge();
        }


      });
    });

  }

  //---------------------------------------------------------------------------------------------------------------
  private void FinalizeMerge()
  {
    this.MergeInProcess = false;
    Game.Events.OnCardMergeEnded.Invoke();


  }
}
