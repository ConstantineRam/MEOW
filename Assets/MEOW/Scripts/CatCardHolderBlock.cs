using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class CatCardHolderBlock : HolderBlock
{
  [SerializeField]
  private int doubleBonus;
  [SerializeField]
  private int TwodoublesExtraBonus;
  [SerializeField]
  private int tripleBonus;
  [SerializeField]
  private int quadrupleBonus;

  [SerializeField]
  private ScorePanelController HandScore;

  [SerializeField]
  private SessionResult resultStorage;

  [SerializeField]
  private ScorePanelController scorePanelController;

  private Fader MyFader;
  private int bonusPoints =0;
  public int BonusPoints { get { return bonusPoints; } }

  private bool mainTaskFulfilled;
  public bool MainTaskFulfilled { get { return mainTaskFulfilled; } }
  private bool secondaryTaskFulfilled;
  public bool SecondaryTaskFulfilled { get { return secondaryTaskFulfilled; } }

  //---------------------------------------------------------------------------------------------------------------
  protected sealed override void Awake()
  {
    base.Awake();
    Game.Events.SummoningFinalized.Listen(ResetHandScore);
    this.MyFader = this.GetComponent<Fader>();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void TurnOffSlotsImages()
  {

    this.MyFader.Hide(2f);
  }
  //---------------------------------------------------------------------------------------------------------------
  public void ResetHandScore()
  {
    this.HandScore.Reset();
  }

  //---------------------------------------------------------------------------------------------------------------
  public sealed override void OnCardLanded(HolderInsideBlock holder)
  {
    if (!holder.HasCard)
    {
      Debug.LogError("OnCardLanded got holder with no card in it.");
      return;
    }

    ACard ourCard = holder.GetCard;
    if (ourCard != null)
    {
      Game.ActionRoot.TaskController.CheckGoal(ourCard.GetCardType());
    }
    

    MoveCardsClose();

    // Our fresh card may moved already.
    holder = this.FindHolderWithACard(ourCard);
    

    if (holder == null)
    {
      Debug.LogError("OnCardLanded got null at holder or fresh card was lost.");
      return;
    }

    

    int ResultedPoints = this.RecalculatePoints(holder);
    this.HandScore.UpdatePoints(ResultedPoints);

    if (this.isAllHolderFilled())
    {
      this.mainTaskFulfilled = false;
      this.secondaryTaskFulfilled = false;

      if (this.HasCardOfType(Game.ActionRoot.GameSessionManager.GetMainGoal(resultStorage.GetStage())))
      {

        this.mainTaskFulfilled = true;
      }

      if (this.HasCardOfType(Game.ActionRoot.GameSessionManager.GetSecondaryGoal(resultStorage.GetStage())))
      {
        this.secondaryTaskFulfilled = true;
      }

      float delay = 0.3f;
      if (this.FinalRecalculation())
      {

        delay = 1.8f;
      }

      // final animation is to be played. We don't need to show Mr. Cat reaction this time.
      Game.TimerManager.Start(delay, () =>
      {
        this.CardMerger.OnStartMerge();
      });

      return;
    }
    if (!Game.ActionRoot.PlayerHandController.HandHasCards() && Game.Settings.IsTutorialActive)
    {
      Game.ActionRoot.ContinueTutorial();
      return;
    }


    if (!Game.TutorialManager.WasStageShown(TutorialId.Tutorial101_SameCards))
    {
      if (HasCardsOfSameType())
      {
        Game.TutorialManager.ShowStage(TutorialId.Tutorial101_SameCards, TutorialStage.independent);
      }
      
    }
    


    //if (ResultedPoints < 0)
    //{
    //  Game.ActionRoot.MrCat.ChangeState(1, 1.8f);
    //}

    //if (ResultedPoints > 0)
    //{
    //  Game.ActionRoot.MrCat.ChangeState(2, 1.8f);
    //}

  }
  //---------------------------------------------------------------------------------------------------------------
  private bool HasCardsOfSameType()
  {
    int[] TotalCardTypes = new int[(int) CardTypes.LastBlack + 1];

    foreach (HolderInsideBlock holder in myLeftHolders)
    {

      CatHolderInsideBlock CatHoler = (CatHolderInsideBlock) holder;
      if (!CatHoler.HasCard)
      {
        continue;
      }
      TotalCardTypes[(int) CatHoler.GetCard.GetCardType()]++;
    }


    foreach (HolderInsideBlock holder in myRightHolders)
    {
      CatHolderInsideBlock CatHoler = (CatHolderInsideBlock) holder;
      if (!CatHoler.HasCard)
      {
        continue;
      }
      TotalCardTypes[(int) CatHoler.GetCard.GetCardType()]++;
    }

    for (int i = 0; i < (int) CardTypes.LastBlack + 1; i++)
    {
      if (TotalCardTypes[i] < 2)
      {
        continue;
      }

      return true;
    }

    return false;
  }

  //---------------------------------------------------------------------------------------------------------------
  private void CalculateBonus(int[] TotalCardTypes)
  {
    if (!this.SecondaryTaskFulfilled)
    {
      return;
    }

    int resultAmount = TotalCardTypes[(int) Game.ActionRoot.GameSessionManager.GetSecondaryGoal(resultStorage.GetStage())];
    this.bonusPoints += resultAmount * Game.Settings.BonusValue;
    this.HandScore.PushPoints(resultAmount);

    string msg = "";

    if (resultAmount == 1)
    {
      msg = "BONUS +" + (resultAmount * Game.Settings.BonusValue).ToString();
    }


    if (resultAmount == 2)
    {
      msg = "GOOD BONUS +" + (resultAmount * Game.Settings.BonusValue).ToString();
    }

    if (resultAmount == 3)
    {
      msg = "AWESOME BONUS +" + (resultAmount * Game.Settings.BonusValue).ToString();
    }
    FloatingMsg newMsg = Game.PoolManager.Pop(ObjectPoolName.AFlotingMsg) as FloatingMsg;
    newMsg.Show(msg, Color.white);

  }

  //---------------------------------------------------------------------------------------------------------------
  private bool FinalRecalculation()
  {



    bool result = false;
    bool HasDouble = false;
    string msg = "";
    this.bonusPoints = 0;
    int[] TotalCardTypes = new int[ (int) CardTypes.LastBlack + 1];

    foreach (HolderInsideBlock holder in myLeftHolders)
    {
      
      CatHolderInsideBlock CatHoler = (CatHolderInsideBlock) holder;
      int bonusPoints = CatHoler.GetCard.GetPoints();
      this.bonusPoints += bonusPoints;
     // this.HandScore.PushPoints(bonusPoints);
      TotalCardTypes[(int) CatHoler.GetCard.GetCardType()]++;
    }


    foreach (HolderInsideBlock holder in myRightHolders)
    {
      CatHolderInsideBlock CatHoler = (CatHolderInsideBlock) holder;
      int bonusPoints = CatHoler.GetCard.GetPoints();
      this.bonusPoints += bonusPoints;
   //   this.HandScore.PushPoints(bonusPoints);
      TotalCardTypes[(int) CatHoler.GetCard.GetCardType()]++;
    }

    for (int i = 0; i < (int) CardTypes.LastBlack + 1; i++)
    {
      if (TotalCardTypes[i] < 2)
      {
        continue;
      }

      result = true;


      switch (TotalCardTypes[i])
      {
        case 2:

          if (HasDouble)
          {
            this.bonusPoints += this.TwodoublesExtraBonus * Game.Settings.BonusValue;
            this.HandScore.PushPoints(this.TwodoublesExtraBonus);
            msg = "SUPER DOUBLE +" + ( (this.doubleBonus + this.TwodoublesExtraBonus) * Game.Settings.BonusValue).ToString();
          }
          else
          {
            this.bonusPoints += this.doubleBonus * Game.Settings.BonusValue;
            this.HandScore.PushPoints(this.doubleBonus);
            msg = "DOUBLE +" + (this.doubleBonus * Game.Settings.BonusValue).ToString();
          }
          
          
          HasDouble = true;
        break;
        case 3:
          msg = "TRIPLE +" + (this.tripleBonus * Game.Settings.BonusValue).ToString();
          this.bonusPoints += this.tripleBonus * Game.Settings.BonusValue;
          this.HandScore.PushPoints(this.tripleBonus);
          break;

        case 4:
          msg = "QUADRUPLE +"+ (this.quadrupleBonus * Game.Settings.BonusValue).ToString();
          this.bonusPoints += this.quadrupleBonus * Game.Settings.BonusValue;
          this.HandScore.PushPoints(this.quadrupleBonus);
          break;

        default:
          msg = "MEOW!";
        break;
      }


    }

    if (result)
    {
      FloatingMsg newMsg = Game.PoolManager.Pop(ObjectPoolName.AFlotingMsg) as FloatingMsg;
      newMsg.Show(msg, Color.white);
    }

    Game.TimerManager.Start(1.0f, () =>
    {
      this.CalculateBonus(TotalCardTypes);
    });


    return result;
  }

  //---------------------------------------------------------------------------------------------------------------
  private void ApplyJoker(ACatCard card)
  {
    if (!card.HasMod(CardMod.joker))
    {
      return;
    }

    CardTypes jokerType = card.GetMod(CardMod.joker);

    foreach (HolderInsideBlock holder in myLeftHolders)
    {

      CatHolderInsideBlock CatHoler = (CatHolderInsideBlock) holder;
      if (!CatHoler.HasCard)
      {
        continue;
      }

      ACatCard checkCard = CatHoler.GetCard;
      if (checkCard == card)
      {
        continue; // this card get its self bonus through other method(GetJokersAmount).
      }
      if (checkCard.GetCardType() != jokerType)
      {
        continue;
      }

      checkCard.PushPoints(Game.Settings.BonusValue);
    }


    foreach (HolderInsideBlock holder in myRightHolders)
    {
      CatHolderInsideBlock CatHoler = (CatHolderInsideBlock) holder;
      if (!CatHoler.HasCard)
      {
        continue;
      }

      ACatCard checkCard = CatHoler.GetCard;
      if (checkCard == card)
      {
        continue; // this card get its self bonus through other method(GetJokersAmount).
      }
      if (checkCard.GetCardType() != jokerType)
      {
        continue;
      }

      checkCard.PushPoints(Game.Settings.BonusValue);
    }
  }
  //---------------------------------------------------------------------------------------------------------------
  private int GetJokersAmount(CardTypes type)
  {
    int result = 0;
    foreach (HolderInsideBlock holder in myLeftHolders)
    {

      CatHolderInsideBlock CatHoler = (CatHolderInsideBlock) holder;
      if (!CatHoler.HasCard)
      {
        continue;
      }

      if (CatHoler.GetCard.HasMod(CardMod.joker, type))
      {
        result++;
        CatHoler.GetCard.ScaleBump();
      }
     
    }


    foreach (HolderInsideBlock holder in myRightHolders)
    {
      CatHolderInsideBlock CatHoler = (CatHolderInsideBlock) holder;
      if (!CatHoler.HasCard)
      {
        continue;
      }

      if (CatHoler.GetCard.HasMod(CardMod.joker, type))
      {
        result++;
        CatHoler.GetCard.ScaleBump();
      }
    }

    return result * Game.Settings.BonusValue;
  }
  //---------------------------------------------------------------------------------------------------------------
  private void ApplySideMods(CatHolderInsideBlock left, CatHolderInsideBlock right)
  {
    ACatCard LeftCard = left.GetCard;
    ACatCard RightCard = right.GetCard;

    if (LeftCard == null)
    {
      return;
    }

    if (RightCard == null)
    {
      return;
    }

    if (LeftCard.HasMod(CardMod.right))
    {
      if (LeftCard.GetMod(CardMod.right) == RightCard.GetCardType())
      {
        RightCard.PushPoints(Game.Settings.BonusValue);
      }
    }

    if (RightCard.HasMod(CardMod.left))
    {
      if (RightCard.GetMod(CardMod.left) == LeftCard.GetCardType())
      {
        LeftCard.PushPoints(Game.Settings.BonusValue);
      }
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  private void CheckSide(CatHolderInsideBlock CatHoler)
  {
    CatHolderInsideBlock LeftH = null;
    CatHolderInsideBlock RightH = null;
    if (CatHoler.mySide == Side.Right)
    {
      

      if (CatHoler.myPosition == 0)
      {
        LeftH = this.myLeftHolders[0] as CatHolderInsideBlock;
        RightH = this.myRightHolders[1] as CatHolderInsideBlock;
      }
      else
      {
        LeftH = this.myRightHolders[0] as CatHolderInsideBlock;
      }
    }

    if (CatHoler.mySide == Side.Left)
    {

     if (CatHoler.myPosition == 0)
      {
        LeftH = this.myLeftHolders[1] as CatHolderInsideBlock;
        RightH = this.myRightHolders[0] as CatHolderInsideBlock;
      }
      else
      {
        RightH = this.myLeftHolders[0] as CatHolderInsideBlock;
      }
    }

    if (LeftH != null)
    {
      this.ApplySideMods(LeftH, CatHoler);
    }

    if (RightH != null)
    {
      this.ApplySideMods(CatHoler, RightH);
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  private int RecalculatePoints(HolderInsideBlock holder)
  {
    if (!holder.HasCard)
    {
      Debug.LogError("RecalculatePoints was called for holder "+ holder.name + ", but it has no card.");
      return 0;
    }
    CatHolderInsideBlock CatHoler = (CatHolderInsideBlock) holder;
    ACatCard card = CatHoler.GetCard;

    // Check Other Jokers on the table;
    int JokerBonus = this.GetJokersAmount(card.GetCardType());

    if (JokerBonus > 0)
    {
      card.PushPoints(JokerBonus);
    }

    ApplyJoker(card);

    this.CheckSide(CatHoler);

    return card.GetPoints();
  }

  //---------------------------------------------------------------------------------------------------------------
  private void ReportScore(int Score)
  {
    this.scorePanelController.PushPoints(Score);

  }
}
