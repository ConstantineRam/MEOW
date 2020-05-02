using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

public class SummonedKotan : Kotan
{
  [SerializeField]
  private Image puff;
  [SerializeField]
  private SessionResult resultStorage;

  [SerializeField]
  private CatCardHolderBlock cardBlock;

  [SerializeField]
  private Image NonSummonFlightImage;

  [SerializeField]
  private GameObject OptionsBtn;

  private const float PuffAppear = 0.5f;
  private const float SummonTime = 3f;
  private const float CatAppear = 1.25f;
  private const float CatFlight = 0.6f;
  private Vector3 scaleFrom;
  private Vector3 scaleCat;
  private Vector3 punchCat;
  private Vector3 myPosition;

  private List<Tween> tweens;
  // Use this for initialization

  //---------------------------------------------------------------------------------------------------------------
  protected override void Awake ()
  {
    base.Awake();
    tweens = new List<Tween>();
    this.NonSummonFlightImage.gameObject.SetActive(false);
    Game.Events.OnCardMergeEnded.Listen(MergeDone);
    Game.Events.OnCardMergeStarted.Listen(MergeStart);


    this.puff.gameObject.SetActive(false);
    this.myPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);

  }

  //---------------------------------------------------------------------------------------------------------------
  public void MergeStart()
  {
    if (this.isTasksFulfilled())
    {
      if (UnityEngine.Random.Range(0, 100) > 50)
      {
        Game.ActionRoot.MrCat.ChangeState(2, 4f);
      }
      else
      {
        Game.ActionRoot.MrCat.ChangeState(3, 4f);
      }
      
    }
    else
    {
      if (Game.Settings.IsTutorialActive)
      {
        Game.ActionRoot.MrCat.ChangeState(3, 4f);
      }
      else
      {
        Game.ActionRoot.MrCat.ChangeState(1, 4f);
      }
      
    }


  }

  //---------------------------------------------------------------------------------------------------------------
  public bool isTasksFulfilled()  
  {
    if (this.cardBlock.BonusPoints < 1)
    {
      return true;
    }

    if (this.cardBlock.MainTaskFulfilled)
    {
      return true;
    }

    return false;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void MergeDone()
  {
    Game.ActionRoot.SummonBlock.StopAcceptingCards();
    // this.DisableMask();



    if (this.cardBlock.BonusPoints < 1)
    {
      GameLostPopUp.GameLostPopUpData GameLostPopUpData = new GameLostPopUp.GameLostPopUpData();
      GameLostPopUpData.lostReason = GameLostPopUp.LostReason.LowScore;
      Game.ActionRoot.DifficultyManager.RegisterLose();
      Game.UiManager.Open(PopupId.GameOver, GameLostPopUpData);
      return;
    }

    if (!this.cardBlock.MainTaskFulfilled)
    {
      GameLostPopUp.GameLostPopUpData GameLostPopUpData = new GameLostPopUp.GameLostPopUpData();
      GameLostPopUpData.lostReason = GameLostPopUp.LostReason.WrongCards;
      Game.ActionRoot.DifficultyManager.RegisterLose();
      Game.UiManager.Open(PopupId.GameOver, GameLostPopUpData);
      return;
    }


    


    this.DebugRandomKotan();

    scaleFrom = new Vector3(0, 0, 0);
    scaleCat = new Vector3(1.2f, 1.2f, 1.2f);


    this.NonSummonFlightImage.gameObject.SetActive(true);
    Vector3 FlyCoords = this.resultStorage.Push(null, CatFlight, this.cardBlock.BonusPoints);
    tweens.Add(this.transform.DOMove(FlyCoords, CatFlight));
   // tweens.Add(this.transform.DOScale(scaleFrom, CatFlight + 0.1f));


    Game.Events.SummoningFinalized.Invoke();

    Game.TimerManager.Start(CatFlight, EndOfPostMergeRoutines);



    //SummonAnimationStart(); This is an old flow.





  }
  //---------------------------------------------------------------------------------------------------------------
  private void SummonAnimationStart()
  {

    this.puff.gameObject.SetActive(true);
    tweens.Add(this.puff.rectTransform.DOScale(scaleFrom, PuffAppear).From());
    tweens.Add(this.puff.rectTransform.DOShakePosition (SummonTime, 1f));

    Game.TimerManager.Start(PuffAppear, SummonAnimationCatAppear);

  }

  //---------------------------------------------------------------------------------------------------------------
  private void SummonAnimationCatAppear()
  {

    this.TurnOn();
    tweens.Add(this.transform.DOMoveY(this.transform.position.y - 420, CatAppear).From());
    Game.TimerManager.Start(CatAppear + CatAppear / 6, () => 
    {
      Game.AudioManager.PlaySound(AudioId.CatSound, 0.5f);
      // tweens.Add(this.transform.DOScale(scaleCat, CatFlight + 0.1f)); 
      this.punchCat = new Vector3(1, 1.3f, 1);
     // tweens.Add(this.transform.DOMoveY(this.transform.position.y - 460, 0.2f));
      tweens.Add(this.transform.DOScale(scaleCat, 0.9f).SetEase(Ease.OutBack) );
    });
    

    Game.TimerManager.Start(CatAppear + CatAppear, SummonAnimationCatEnd);

  }

  //---------------------------------------------------------------------------------------------------------------
  private void SummonAnimationCatEnd()
  {
    tweens.Add(this.puff.rectTransform.DOScale(scaleFrom, PuffAppear));

    Vector3 FlyCoords = this.resultStorage.Push(this.GetKotanData(), CatFlight, this.cardBlock.BonusPoints);
    tweens.Add(this.transform.DOMove(FlyCoords, CatFlight));
    tweens.Add(this.transform.DOScale (scaleFrom, CatFlight+0.1f));

    Game.TimerManager.Start(CatFlight, FinalizeSummoning);

  }
  
  //---------------------------------------------------------------------------------------------------------------
  private void FinalizeSummoning()
  {

    foreach (Tween t in tweens)
    {
      t.Kill();
    }
    tweens.Clear();

    Game.Events.SummoningFinalized.Invoke();

    this.EndOfPostMergeRoutines();
  }

  //---------------------------------------------------------------------------------------------------------------
  private void EndOfPostMergeRoutines()
  {
    this.puff.rectTransform.localScale = new Vector3(1, 1, 1);
    this.puff.gameObject.SetActive(false);
    this.TurnOff();
    this.transform.localPosition = this.myPosition;
    this.transform.localScale = new Vector3(1, 1, 1);

    if (!this.resultStorage.HasEmpty())
    {
      //session won.
      this.SessionWon();
      return;
    }
    this.NonSummonFlightImage.gameObject.SetActive(false);

    //Game.ActionRoot.GameSessionManager.ShowTask(this.resultStorage.GetStage());
    Game.ActionRoot.SummonBlock.StartAcceptingCards();


  }


  //---------------------------------------------------------------------------------------------------------------
  private void SessionWon()
  {
    this.cardBlock.TurnOffSlotsImages();
    Game.ActionRoot.MrCat.ChangeState(3, 10f);
    this.resultStorage.FlyBtnsForSummoning();
    Game.Events.GameWon.Invoke();
    this.NonSummonFlightImage.gameObject.SetActive(false);
  }

    //---------------------------------------------------------------------------------------------------------------
    public void DebugRandomKotan()
  {
    this.ActivateKotan(Game.ActionRoot.KotansStorage.GetRandomKotan());

  }
}
