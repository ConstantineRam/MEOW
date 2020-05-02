//---------------------------------------------------------------------------------------------------------------
//Holder block has few holders inside allowing to control them with specified requirments and execute specific behavior if they are filled.
//
//---------------------------------------------------------------------------------------------------------------
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class HolderBlock : MonoBehaviour
{

  [SerializeField]
  private float mergingCardsUpdateDelay;

  [Header("Add holders from center to border")]
  [SerializeField]
  protected List<HolderInsideBlock> myLeftHolders;
  [SerializeField]
  protected List<HolderInsideBlock> myRightHolders;
  [SerializeField]
  private CardMerger cardMerger;
  protected CardMerger CardMerger { get { return cardMerger; } }

  private bool dragActive;
  private bool CheckCardMoveAllowed = true;
  private bool BusyMerging = false;

  private bool BusyMoving = false;
  private bool DoCardMoveCheck = false;



  //---------------------------------------------------------------------------------------------------------------
  protected virtual void Awake()
  {
    Game.Events.OnCardMergeEnded.Listen(() =>{ this.Merging(false); });
    Game.Events.OnCardMergeStarted.Listen(() => { this.Merging(true); });

    Game.Events.DragStarted.Listen(() => { this.dragActive = true; });
    //Game.Events.DragEnded.Listen(() => { });

    Game.Events.DragEnded.Listen(() => { this.ProcessCardMoveCheck (); });
    int position = 0;
    foreach (HolderInsideBlock holder in myLeftHolders)
    {
      holder.myPosition = position;
      position++;
      holder.mySide = Side.Left;
      holder.myHolder = this;
    }

    position = 0;
    foreach (HolderInsideBlock holder in myRightHolders)
    {
      holder.myPosition = position;
      position++;
      holder.mySide = Side.Right;
      holder.myHolder = this;
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Once we dropped a card, we need to make one check to move cards closer.
  /// </summary>
  public void ProcessCardMoveCheck()
  {
    this.dragActive = false;
    this.DoCardMoveCheck = true;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void Merging(bool merge)
  {
    this.BusyMerging = merge;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void StopAcceptingCards()
  {
    foreach (HolderInsideBlock holder in myLeftHolders)
    {
      holder.SetAllowToAcceptCardDrop = false;
    }


    foreach (HolderInsideBlock holder in myRightHolders)
    {
      holder.SetAllowToAcceptCardDrop = false;
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public void StartAcceptingCards()
  {
    foreach (HolderInsideBlock holder in myLeftHolders)
    {
      holder.SetAllowToAcceptCardDrop = true;
    }


    foreach (HolderInsideBlock holder in myRightHolders)
    {
      holder.SetAllowToAcceptCardDrop = true;
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  void Update ()
  {
    //if (this.dragActive)
    //{
    //  return;
    //}

    //if (!this.CheckCardMoveAllowed)
    //{
    //  return;
    //}

    //this.CheckCardMoveAllowed = false;

    //Game.TimerManager.Start(this.mergingCardsUpdateDelay, () =>
    //{
    //  this.CheckCardMoveAllowed = true;
    //});

    //if (this.BusyMerging)
    //{
    //  return;
    //}

    //if (this.BusyMoving)
    //{
    //  return;
    //}

    //if (this.DoCardMoveCheck)
    //{
    //  MoveCardsClose();
    //}
    
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool ProcessPush(ACard card, Side HolderSide, Side FromPush, int position, bool DeclineIfNotEmpty = false, bool CleanUpPush = false)
  {
    
    if (card == null)
    {
      Debug.LogError("Holder block got null at card to process");
      return false;
    }

    if (position < 0)
    {
      Debug.LogError("Holder block got position < 0 to process");
      return false;
    }

    if (position == 0 && !CleanUpPush && FromPush == HolderSide) // if the holder is in the center, at first we try to push to opposite center.
    {
      if (this.TryToPush(card, this.ReverseSide(HolderSide), 0, false))
      {
        return true;
      }

    }

    // if position isn't 0 we will try to push towards it.
    if (this.TryToPush(card, HolderSide, position-1, CleanUpPush))
    {
      return true;
    }

    // and the last attempt, we will try to push outside

    if (!CleanUpPush)
    {
      if (this.TryToPush(card, HolderSide, position + 1, DeclineIfNotEmpty))
      {
        return true;
      }
    }


    return false;
  }
  //---------------------------------------------------------------------------------------------------------------
  private bool TryToPush(ACard card, Side side, int position, bool DeclineIfNotEmpty = false)
  {
    HolderInsideBlock checkedBlock = this.GetHolderAtPos(side, position);
    if (checkedBlock == null)
    {
      return false;
    }

    if (checkedBlock.AskToAcceptPush(card, DeclineIfNotEmpty))
    {
      return true;
    }

    return false;

  }

  //---------------------------------------------------------------------------------------------------------------
  public bool isAllHolderFilled()
  {

    foreach (HolderInsideBlock holder in myLeftHolders)
    {
      if (!holder.HasCard)
      {
        return false;
      }
    }


    foreach (HolderInsideBlock holder in myRightHolders)
    {
      if (!holder.HasCard)
      {
        return false;
      }
    }

    return true;
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool HasCardOfType(CardTypes typeToFind)
  {
    foreach (HolderInsideBlock holder in myLeftHolders)
    {
      if (!holder.HasCard)
      {
        continue;
      }

      if (holder.GetCard.GetCardType() == typeToFind)
      {
        return true;
      }
    }


    foreach (HolderInsideBlock holder in myRightHolders)
    {
      if (!holder.HasCard)
      {
        continue;
      }

      if (holder.GetCard.GetCardType() == typeToFind)
      {
        return true;
      }
    }

    return false;
  }

  //---------------------------------------------------------------------------------------------------------------
  protected void MoveCardsClose()
  {
    
    this.DoCardMoveCheck = false;
    this.BusyMoving = true;

    foreach (HolderInsideBlock holder in this.myRightHolders)
    {
      if (holder.myPosition == 0)
      {
        continue;
      }
      holder.DoPushing(true);
    }

    foreach (HolderInsideBlock holder in this.myLeftHolders)
    {
      if (holder.myPosition == 0)
      {
        continue;
      }
      holder.DoPushing(true);
    }
    this.BusyMoving = false;
  }


  //---------------------------------------------------------------------------------------------------------------
  private HolderInsideBlock GetHolderAtPos(Side side, int position)
  {
    if (position < 0)
    {
      // nothing left on this side
      return null;
    }
    List<HolderInsideBlock> usedList = null;

    if (side == Side.Left)
    {
      usedList = this.myLeftHolders;
    }
    else
    {
      usedList = this.myRightHolders;
    }

    if (usedList.Count < (position + 1))
    {
      // too far
      return null;
    }

    return usedList.ElementAt(position);

  }

  //---------------------------------------------------------------------------------------------------------------
  private Side ReverseSide(Side push)
  {
    if (push == Side.Left)
    {
      return Side.Right;
    }

    return Side.Left;

  }

  //---------------------------------------------------------------------------------------------------------------
  protected HolderInsideBlock FindHolderWithACard(ACard card)
  {
    foreach (HolderInsideBlock holder in myLeftHolders)
    {

      CatHolderInsideBlock CatHoler = (CatHolderInsideBlock) holder;
      if (!holder.HasCard)
      {
        continue;
      }

      if (holder.GetCard == card)
      {
        return holder;
      }
    }


    foreach (HolderInsideBlock holder in myRightHolders)
    {
      CatHolderInsideBlock CatHoler = (CatHolderInsideBlock) holder;
      if (!holder.HasCard)
      {
        continue;
      }

      if (holder.GetCard == card)
      {
        return holder;
      }
    }

    return null;
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Called by a holder who finally got a card.
  /// </summary>
  public virtual void OnCardLanded(HolderInsideBlock holder)
  {


  }

}
