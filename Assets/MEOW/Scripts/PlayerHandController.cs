using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandController : MonoBehaviour
{
  [SerializeField]
  private SessionResult sessionResult;

  [SerializeField]
  CardHolderController[] myHolders;


  private int cardsCounter = 0;

  private bool CheckForTutorialDelay = false;
  // Use this for initialization


  //---------------------------------------------------------------------------------------------------------------
    void Start ()
  {
    if (this.myHolders.Length < 1)
    {
      Debug.LogError("Empty CardHolders!");
    }


    if (Game.Settings.IsTutorialActive)
    {
      Debug.Log("Tutorial is active. Started.");

            if (Game.TutorialManager == null)
            {
                Debug.LogError("null at tutorial manager");
            }

     Game.TutorialManager.ResetFromStage(TutorialId.Stage3_ActionStart0);
      Game.ActionRoot.ContinueTutorial();
      return;
    }
    this.ResetHand();
    
    

  }

  //---------------------------------------------------------------------------------------------------------------
  void Update()
  {
    return;

    if (!Game.TutorialManager.IsActive)
    {
      return;
    }

    if (CheckForTutorialDelay)
    {
      return;
    }

    CheckForTutorialDelay = true;
    Game.TimerManager.Start(2f, () => { CheckForTutorialDelay = false; });


    if (!Game.TutorialManager.WasStageShown(TutorialId.Tutorial102_SideMods))
    {
      if (this.HasCardWithMod(CardMod.left, CardTypes.NoType) || this.HasCardWithMod(CardMod.right, CardTypes.NoType))
      {
        if (!Game.ActionRoot.CatCardHolderBlock.isAllHolderFilled()) // we don't want tutorial to fire right at the end of the cat summoning session,
        {
          Game.TutorialManager.ShowStage(TutorialId.Tutorial102_SideMods, TutorialStage.independent);
        }
        
      }
      return;
    }

    if (!Game.TutorialManager.WasStageShown(TutorialId.Tutorial103_JokerMods))
    {
      if (this.HasCardWithMod(CardMod.joker, CardTypes.NoType))
      {
        if (!Game.ActionRoot.CatCardHolderBlock.isAllHolderFilled()) // we don't want tutorial to fire right at the end of the cat summoning session,
        {
          Game.TutorialManager.ShowStage(TutorialId.Tutorial103_JokerMods, TutorialStage.independent);
        }
      }
      return;
    }


    if (!Game.TutorialManager.WasStageShown(TutorialId.Tutorial104_BlackCard_0))
    {
      if (this.HasCardType(CardTypes.Black))
      {
        if (!Game.ActionRoot.CatCardHolderBlock.isAllHolderFilled()) // we don't want tutorial to fire right at the end of the cat summoning session,
        {
          Game.TutorialManager.ShowStage(TutorialId.Tutorial104_BlackCard_0, TutorialStage.independent);
        }
      }
      return;
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public void TutorialGame(int TutorialStep)
  {


    this.CLoseHolders();

    switch (TutorialStep)
    {
      case 1:
        {
        Game.TutorialManager.ShowStage(TutorialId.Stage3_ActionStart0, TutorialStage.standard);
        break;
        }

      case 2:
        {
          Game.TutorialManager.ShowStage(TutorialId.Stage5_ActionStart2, TutorialStage.standard);
          break;
        }

      case 3:
        {
          Game.TutorialManager.ShowStage(TutorialId.Stage6_ActionStart3, TutorialStage.standard);
          break;
        }

      case 4:
        {
          Game.TutorialManager.ShowStage(TutorialId.Stage7_ActionStart4, TutorialStage.standard);
          break;
        }

      case 5:
        {
          Game.TutorialManager.ShowStage(TutorialId.Stage8_FirstCatSummoned, TutorialStage.standard);
          break;
        }

      case 6:
        {
          Game.TutorialManager.ShowStage(TutorialId.Stage10_TalkToMagicCat, TutorialStage.standard);
          break;
        }

      case 7:
        {
          Game.TutorialManager.ShowStage(TutorialId.Tutorial103_JokerMods, TutorialStage.standard);
          //          
          break;
        }

      default:
        {
          break;
        }
    }

  }
  //---------------------------------------------------------------------------------------------------------------
  public void FillCardsForTutorial(int stage)
  {
    switch (stage)
    {
      case 1:
        {
        //  Game.ActionRoot.GameSessionManager.ShowTask(); NOT shown for the first Tutorial round
          // one card of every single type
          for (int i = 0; i < (int) CardTypes.LastType + 1; i++)
         {
           Game.ActionRoot.GameSessionManager.GetACard((CardTypes) i, stage, this.myHolders[i].GetDeck, CardFeatureRequirment.disabled, CardFeatureRequirment.disabled, CardFeatureRequirment.disabled);
         }
         this.OpenHolders();
         break;
        }

      case 2:
        {
          // one main task type
          CardTypes TaskType = Game.ActionRoot.GameSessionManager.GetCurrentStageTask();
      
          Game.ActionRoot.GameSessionManager.GetACard((CardTypes) TaskType, stage, this.myHolders[(int) TaskType].GetDeck, CardFeatureRequirment.disabled, CardFeatureRequirment.disabled, CardFeatureRequirment.disabled);
          
          this.OpenHolders();
          break;
        }


      case 3:
        {
          // one main task and 3 bonus types. Explaining how bonus works.
          CardTypes TaskType = Game.ActionRoot.GameSessionManager.GetCurrentStageTask();
          CardTypes SecondaryTaskType = Game.ActionRoot.GameSessionManager.GetCurrentStageBonusTask();
          //Game.ActionRoot.GameSessionManager.GetACard((CardTypes) TaskType, stage, this.myHolders[(int) TaskType].GetDeck, CardFeatureRequirment.disabled, CardFeatureRequirment.disabled, CardFeatureRequirment.disabled);

          for (int i = 0; i < (int) CardTypes.LastType ; i++)
          {
            Game.ActionRoot.GameSessionManager.GetACard((CardTypes) SecondaryTaskType, stage, this.myHolders[(int) SecondaryTaskType].GetDeck, CardFeatureRequirment.disabled, CardFeatureRequirment.disabled, CardFeatureRequirment.disabled);
          }
          this.OpenHolders();
          break;
        }

      case 4:
        {
          // explaining Side bonuses.
          for (int i = 0; i < (int) CardTypes.LastType + 1; i++)
          {
            Game.ActionRoot.GameSessionManager.GetACard((CardTypes) i, stage, this.myHolders[i].GetDeck, CardFeatureRequirment.disabled, CardFeatureRequirment.forced, CardFeatureRequirment.disabled);
          }
          this.OpenHolders();
          break;
        }

      case 5:
        {
          // explaining Joker bonuses.
          for (int i = 0; i < (int) CardTypes.LastType + 1; i++)
          {
            Game.ActionRoot.GameSessionManager.GetACard((CardTypes) i, stage, this.myHolders[i].GetDeck, CardFeatureRequirment.disabled, CardFeatureRequirment.disabled, CardFeatureRequirment.forced);
          }
          this.OpenHolders();
          break;
        }

      case 6:
        {
          // Black cards Tutorial. Total two cards of main and bonus task when first in each deck is spoiled (black) card.
          CardTypes TaskType = Game.ActionRoot.GameSessionManager.GetCurrentStageTask();
          CardTypes SecondaryTaskType = Game.ActionRoot.GameSessionManager.GetCurrentStageBonusTask();
          Game.ActionRoot.GameSessionManager.GetACard((CardTypes) TaskType, stage, this.myHolders[(int) TaskType].GetDeck, CardFeatureRequirment.forced, CardFeatureRequirment.forced, CardFeatureRequirment.forced);
          ACatCard cardToSpoil = Game.ActionRoot.GameSessionManager.GetACard((CardTypes) TaskType, stage, this.myHolders[(int) TaskType].GetDeck, CardFeatureRequirment.disabled, CardFeatureRequirment.disabled, CardFeatureRequirment.disabled);
          cardToSpoil.Spoil();

          Game.ActionRoot.GameSessionManager.GetACard((CardTypes) SecondaryTaskType, stage, this.myHolders[(int) SecondaryTaskType].GetDeck, CardFeatureRequirment.forced, CardFeatureRequirment.forced, CardFeatureRequirment.forced);
          cardToSpoil = Game.ActionRoot.GameSessionManager.GetACard((CardTypes) SecondaryTaskType, stage, this.myHolders[(int) SecondaryTaskType].GetDeck, CardFeatureRequirment.disabled, CardFeatureRequirment.disabled, CardFeatureRequirment.disabled);
          cardToSpoil.Spoil();

          this.OpenHolders();
          break;
        }


      default:
        {
          Debug.LogError("wrong tutorial stage was called.");
          break;
        }
    }
  }


  //---------------------------------------------------------------------------------------------------------------

  private bool HasCardWithMod(CardMod modToFind, CardTypes CardColor)
  {
    for (int i = 0; i < this.myHolders.Length; i++)
    {
      if (!this.myHolders[i].HasCard)
      {
        continue;
      }
      ACatCard checkCard = this.myHolders[i].GetCard as ACatCard;
      if (!checkCard.HasMod())
      {
        continue;
      }
      CardTypes foundMod = checkCard.GetMod(modToFind);
      if (foundMod == CardTypes.NoType)
      {
        continue;
      }

      if (CardColor == CardTypes.NoType)
      {
        // anything but black
        if (foundMod != CardTypes.Black)
        {
          return true;
        }

        continue;
      }

      if (foundMod == CardColor)
      {
        return true;
      }

    }

    return false;
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool HandHasCards()
  {
    for (int i = 0; i < this.myHolders.Length; i++)
    {
      if (this.myHolders[i].HolderAndDeckHasCard())
      {
        return true;
      }
    }

    return false;
  }

  //---------------------------------------------------------------------------------------------------------------

  public bool HasCardType(CardTypes checkType, bool CheckDecs = false)
  {
    for (int i = 0; i < this.myHolders.Length; i++)
    {
      if (!this.myHolders[i].HasCard)
      {
        continue;
      }

      if (this.myHolders[i].GetCard.GetCardType() == checkType)
      {
        return true;
      }
      else
      {
        if (CheckDecs)
        {
          if (this.myHolders[i].HasDeck())
          {
            if (this.myHolders[i].GetDeck.HasCardType(checkType))
            {
              return true;
            }
          }
        }
      }
    }

    return false;
  }
  //---------------------------------------------------------------------------------------------------------------
  private void CLoseHolders()
  {
    for (int i = 0; i < (int) CardTypes.LastType + 1; i++)
    {
      this.myHolders[(int) i].SetRefreshState = RefreshState.RefreshStopped;
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  private void OpenHolders()
  {
    float step = 0.2f;

    this.myHolders[(int) CardTypes.Green].SetRefreshState = RefreshState.Auto;


    Game.TimerManager.Start(1 * step, () => { this.myHolders[(int) CardTypes.Blue].SetRefreshState = RefreshState.Auto; });
    Game.TimerManager.Start(2 * step, () => { this.myHolders[(int) CardTypes.Purple].SetRefreshState = RefreshState.Auto; });
    Game.TimerManager.Start(3 * step, () => { this.myHolders[(int) CardTypes.Yellow].SetRefreshState = RefreshState.Auto; });
  }


  //---------------------------------------------------------------------------------------------------------------
  public void ResetHand()
  {
    this.cardsCounter = Game.ActionRoot.GameSessionManager.SessionLenght+1;
    this.CLoseHolders();

    this.PushCards();
    Game.ActionRoot.GameSessionManager.ShowTask(1);

  }
  //---------------------------------------------------------------------------------------------------------------
  private void PushCards()
  {
    this.cardsCounter--;
    if (this.cardsCounter < 1)
    {
      this.OpenHolders();
      return;
    }

    for (int i = 0; i < (int) CardTypes.LastType+1; i++)
    {
      Game.ActionRoot.GameSessionManager.GetACard((CardTypes) i, cardsCounter, this.myHolders[i].GetDeck);
    }

    // once we created a row we showd check for a black card as a task. However, honestly. It never sits well with me. Why do we want MAIN feature of a cat be a black card.

    //if (this.task1ar[Stage - 1] == CardTypes.Black)
    //{ // if black card was requested, always create smallest one possible.
    //  // pretty shitty design, because 100% leftmost deck.
    //  if (this.hasBlackCard[Stage - 1])
    //  { }
    //  CreatedCard.Spoil();
    //  CreatedCard.SetPoints(-1);
    //  this.hasBlackCard[Stage - 1] = true;
    //  return CreatedCard;
    //}

    Game.TimerManager.Start(0.1f, () => { this.PushCards(); });
  }
}
