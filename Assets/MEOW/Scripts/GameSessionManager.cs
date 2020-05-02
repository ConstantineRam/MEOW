using System;
using System.Collections.Generic;
using UnityEngine;
// every session is theoretically winnable if we have enough cards of a color in a deck.

public class GameSessionManager : MonoBehaviour
{
  [SerializeField]
  private int sessionLenght;
  public int SessionLenght { get { return sessionLenght; } }

  [SerializeField]
  private SessionResult sessionResult;
  [SerializeField]
  private PlayerHandController playerHand;
  [SerializeField]
  private TaskController taskController;

  [SerializeField]
  private CatCardHolderBlock CatCardHolderBlock;

  [SerializeField]
  int ChangeNextPointLevel = 5;
  [SerializeField]
  int DonwstepForPoints = 10;
  [SerializeField]
  int ChangeForSpecial = 40;
  [SerializeField]
  int MinChangeForSpecial = 14;
  [SerializeField]
  int ChangeForSecondSpecial = 20;
  [SerializeField]
  int MinChangeForSecondSpecial = 5;
  [SerializeField]
  int ChangeForThirdSpecial = 4;

  [SerializeField]
  int ChangeToLosePoint = 10;

  [SerializeField]
  int ChangeToGainPoint = 15;

  [SerializeField]
  int SpoilChance = 14;
  [SerializeField]
  int MinSpoilChance = 3;

  [SerializeField]
  int FreeCats = 6;

  Queue<CardTypes> task1;
  Queue<CardTypes> task2;

  private bool TutorialDeck = false;


  CardTypes[] task1ar;
  bool[] hasBlackCard;
  CardTypes[] task2ar;
  int[] reservedTypes;

  //---------------------------------------------------------------------------------------------------------------
  void Awake()
  {
    this.TutorialDeck = false;
    if (Game.TutorialManager.GetLastStage < (int) TutorialId.Stage3_ActionStart0)
    {
      this.TutorialDeck = true;
      SpoilChance = -1;
      Debug.Log("Black cards were disabled. Tutirial deck is in action.");
    }

    this.task1 = new Queue<CardTypes>();
    this.task2 = new Queue<CardTypes>();
    PrepareTask();
  }

  //---------------------------------------------------------------------------------------------------------------
  void Start()
  {


  }

  //---------------------------------------------------------------------------------------------------------------
  public bool CheckStageForPotentialLose (int stage)
  {
    if (stage > this.sessionLenght)
    {
      return false;
    }
    CardTypes CardToFind = this.task1ar[stage - 1];

    if (!this.playerHand.HasCardType(CardToFind, true) &&!Game.Settings.IsTutorialActive)
    {
      GameLostPopUp.GameLostPopUpData GameLostPopUpData = new GameLostPopUp.GameLostPopUpData();
      GameLostPopUpData.lostReason = GameLostPopUp.LostReason.UsedAllCards;
      Game.ActionRoot.DifficultyManager.RegisterLose();
      Game.UiManager.Open(PopupId.GameOver, GameLostPopUpData);
      return true;
    }

    return false;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void CheckPotentialLose()
  {
    int StageToCheck = sessionResult.GetStage();

    if (!this.CheckStageForPotentialLose(StageToCheck))
    {
     // this.CheckStageForPotentialLose(StageToCheck + 1); checking NEXT stage leads to frustrating situations when game stops for a goal that wasn't shown yet.
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public CardTypes GetCurrentStageTask()
  {
    return this.task1ar[sessionResult.GetStage() - 1];
  }

  //---------------------------------------------------------------------------------------------------------------
  public CardTypes GetCurrentStageBonusTask()
  {
    return this.task2ar[sessionResult.GetStage() - 1];
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ShowTask (int stage = 0)
  {
    if (stage == 0)
    {
      stage = sessionResult.GetStage();
    }

    this.taskController.Hide(0.7f);
    if (stage > this.sessionLenght)
    {
      return;
    }

    Game.TimerManager.Start(0.9f, () =>
    {
      CardTypes hint = CardTypes.NoType;
      if (stage < this.sessionLenght)
      {
        hint = this.task1ar[stage];
      }
           
      this.taskController.Show(this.task1ar[stage-1], this.task2ar[stage-1], hint);
      if (hint != CardTypes.NoType)
      {
        this.taskController.UnHide(0.8f);
      }
      else
      {
        this.taskController.UnHide(0.8f, false);
      }
      
    });

  }

  //---------------------------------------------------------------------------------------------------------------
  private ACAtCardData GetCardData(CardTypes type)
  {
    //int chance = this.ChangeNextPointLevel;
    //int pointsNeeded = 1;
    //while (TryIncPoints(pointsNeeded, chance))
    //{
    //  chance -= this.DonwstepForPoints;
    //  pointsNeeded++;
    //}
    //if (TryIncPoints(pointsNeeded, chance))
    //{ pointsNeeded++; }
    return Game.ActionRoot.CardManager.CardStorage.GetAvaliableDataByType(type);

  }

  //---------------------------------------------------------------------------------------------------------------
  private bool isSpoilAllowed(CardTypes type, int Stage)
  {
    if (this.task1ar[Stage - 1] == type)
    {
      return false;
    }

    if (this.task2ar[Stage - 1] == type)
    {
      return false;
    }

    if (this.hasBlackCard[Stage - 1])
    {
      if (Game.ActionRoot.DifficultyManager.ChanceOf100(70, DifficultyManager.CheckType.Positive) )
      {
        return false;
      }
        
    }

    return true;
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool CheckStageNum(int stage)
  {
    if (stage > this.sessionResult.MaxResults())
    {
      return false;
    }

    if (stage < 1)
    {
      return false;
    }

    return true;
  }

  //---------------------------------------------------------------------------------------------------------------
  public CardTypes GetSecondaryGoal(int stage)
  {
    if (!this.CheckStageNum(stage))
    {
      return CardTypes.NoType;
    }

    return this.task2ar[stage - 1];
  }

  //---------------------------------------------------------------------------------------------------------------
  public CardTypes GetMainGoal(int stage)
  {
    if (!this.CheckStageNum(stage))
    {
      Debug.LogError("wrong  stage "+ stage);
      return CardTypes.NoType;
    }

    return this.task1ar[stage - 1];
  }


  //---------------------------------------------------------------------------------------------------------------
  public ACatCard GetACard(CardTypes type, int Stage, Deck PushDeck, CardFeatureRequirment Blacks = CardFeatureRequirment.enabled, CardFeatureRequirment SideBonuses = CardFeatureRequirment.enabled, CardFeatureRequirment Jockers = CardFeatureRequirment.enabled)
  {
    if (!this.CheckStageNum(Stage))
    {
      Debug.LogError("Wrong session stage. (" + Stage + ")");
      return null;
    }

    if (type == CardTypes.NoType)
    {
      Debug.LogError("Wrong type was requested.");
      return null;
    }

    ACAtCardData newCardData = this.GetCardData(type);

    if (newCardData == null)
    {
      Debug.LogError("Unexpected Error. Can't get a card from storage.");
      return null;
    }

    // okay, we got a card. Let's create it.
    ACatCard CreatedCard = (ACatCard) Game.ActionRoot.CardManager.CreateACard(newCardData, PushDeck);


    if (isSpoilAllowed(type, Stage))
    {

      if (this.SpoilChance > 0 && Blacks == CardFeatureRequirment.enabled)
      {

        if ( Game.ActionRoot.DifficultyManager.ChanceOf100(this.SpoilChance, DifficultyManager.CheckType.Negative, this.MinSpoilChance) )
        {
          CreatedCard.Spoil();
          this.hasBlackCard[Stage - 1] = true;

        }

        return CreatedCard;
      }
    }
    
    if (Game.Settings.IsTutorialActive)
    {
      CreatedCard.AddCardMods(0, SideBonuses, Jockers, Blacks);
      return CreatedCard;
    }

    if (!Game.ActionRoot.DifficultyManager.ChanceOf100(this.ChangeForSpecial, DifficultyManager.CheckType.Positive, this.MinChangeForSpecial) )
    {
     return CreatedCard;
    }
    
    CreatedCard.AddCardMods(1);
  

    if (!Game.ActionRoot.DifficultyManager.ChanceOf100(this.ChangeForSecondSpecial, DifficultyManager.CheckType.Positive, this.MinChangeForSecondSpecial) )
    {
      return CreatedCard;
    }

    CreatedCard.AddCardMods(1);

    if (!Game.ActionRoot.DifficultyManager.ChanceOf100(this.ChangeForThirdSpecial, DifficultyManager.CheckType.Positive))
    {
      return CreatedCard;
    }

    CreatedCard.AddCardMods(1);

    return CreatedCard;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void PrepareTask()
  {
    CardTypes task1Prev = CardTypes.NoType;
    CardTypes task2Prev = CardTypes.NoType;
    CardTypes task1New = CardTypes.NoType;
    CardTypes task2New = CardTypes.NoType;

    // at first we set up a color goal for each turn, which is to be random.
    // however, we are sure that main goal will never be the same as prev turn and secondary goal would be different.
    // it guaranies our game will not request 7 blue cards when our deck limit is 6, for example.

    CardTypes LastTaskPossible = CardTypes.LastType; // no blacks in task
    //if (this.TutorialDeck)
    //{
    //  LastTaskPossible = CardTypes.LastType;
    //}


    CardTypes LastTaskFor1 = CardTypes.LastType;
    for (int i = 0; i < sessionResult.MaxResults(); i++)
    {
      task1Prev = task1New;
      task2Prev = task2New;

      task1New = (CardTypes) UnityEngine.Random.Range((int) CardTypes.FirstType, (int) LastTaskFor1 + 1);
      while (task1New == task1Prev)
      {
        task1New = (CardTypes) UnityEngine.Random.Range((int) CardTypes.FirstType, (int) LastTaskFor1 + 1);
      }

      task2New = (CardTypes) UnityEngine.Random.Range((int) CardTypes.FirstType, (int) LastTaskPossible + 1);
      while (task1New == task2New)
      {
        task2New = (CardTypes) UnityEngine.Random.Range((int) CardTypes.FirstType, (int) LastTaskPossible + 1);
      }

      this.task1.Enqueue(task1New);
      this.task2.Enqueue(task2New);
    }

    this.task1ar = this.task1.ToArray();
    this.task2ar = this.task2.ToArray();
    this.hasBlackCard = new bool[this.task1ar.Length];
    // but it's not enough. We have to reserve these cards for the player. At end of all we want player to be moderately challenged, but not mangled by the game.
    // So, it's important that deck have a potential card for a task (it doesn't guarantied, that Player will not use them all before, of course)
    // so, for example, if task for turn 3 is Blue and black we WILL assure that layer 3 have it. + plus a bit extra to be sure.
  }


  
}

public enum CardFeatureRequirment
{
 enabled = 0,
 disabled = 1,
 forced = 2
}
