using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class MenuRoot : RootBase
{
  [SerializeField]
  private bool DisableIntroDebugOnly = false;

  [SerializeField]
  private Canvas canvas;
  public Canvas Canvas { get { return canvas; } }
  public float GetScaleFactor { get { return Canvas.scaleFactor; } }

  private UniqueKotanStorage uniqueKotanStorage;
  public UniqueKotanStorage UniqueKotanStorage { get { return uniqueKotanStorage; } }

  private CustomKotanStorage customKotanStorage;
  public CustomKotanStorage CustomKotanStorage { get { return customKotanStorage; } }

  [SerializeField]
  private MrCat mrCat;
  public MrCat MrCat { get { return mrCat; } }

  [SerializeField]
  private GameObject startBtn;
  public GameObject StartBtn { get { return startBtn; } }


  [SerializeField]
  private GameObject catBook;
  public GameObject CatBook { get { return catBook; } }


  [SerializeField]
  private KotansStorage kotansStorage;
  public KotansStorage KotansStorage { get { return kotansStorage; } }

  [SerializeField]
  private Button buyGameBtn;
  public Button BuyGameBtn { get { return buyGameBtn; } }

  [SerializeField]
  private GameObject LetterBtn;

  [SerializeField]
  private GameObject SkipBtn;

  [SerializeField]
  private BuyBtnText buyBtnText;
  public BuyBtnText BuyBtnText { get { return buyBtnText; } }

  private MainMenuHelloweenManager MainMenuHelloweenManager;
  private IntroAnimation IntroAnimation;

  private void Awake()
  {
    base.Awake();
    Game.MenuRoot = this;
    BuyGameBtn.interactable = false;

    this.uniqueKotanStorage = this.GetComponent<UniqueKotanStorage>();
    this.customKotanStorage = this.GetComponent<CustomKotanStorage>();
    this.MainMenuHelloweenManager = this.GetComponent<MainMenuHelloweenManager>();

    this.SkipBtn.SetActive(false);

    this.IntroAnimation = this.GetComponent<IntroAnimation>();

    if (!Game.Settings.LetterFromMeowShown)
    {
      this.LetterBtn.SetActive(false);
    }

  }

  private void Start()
  {
                 
  
    if (Game.TutorialManager.GetLastStage < (int) TutorialId.Stage2_PressStart)
    {
      //this.MrCat.SetAlpha(0.0f);
      this.StartBtn.SetActive(false);
      this.CatBook.SetActive(false);
    }

    float TimeForTutorial = 0.01f;
    if (Game.Settings.IsFirstLaunch)
    {
      if (Game.IsDebug && this.DisableIntroDebugOnly)
      {
        Debug.Log("<color=red>***INTRO DISABLED***</color>");
        this.IntroAnimation.DisableAll();
      }
      else
      {
        this.SkipBtn.SetActive(true);
        this.IntroAnimation.StartAnimation(TutorialChecks);
        //TimeForTutorial = this.IntroAnimation.GetTotalTime();
      }
      
    }
    else
    {

      TutorialChecks();
    }



    StartCoroutine(ActivateIAPBtn());
  }

  //---------------------------------------------------------------------------------------------------------------
  public void SeeIntro()
  {
    this.IntroAnimation.StartAnimation(this.RestartMenuScene);
  }



    //---------------------------------------------------------------------------------------------------------------
    public void ShowLetter()
  {
    Game.UiManager.Open(PopupId.LetterFromMeow);
  }


  //---------------------------------------------------------------------------------------------------------------
  private void TutorialChecks()
  {
    if (Game.Settings.IsFirstLaunch)
    {
      OpenActionScene();
      return;
    }

    AudioId UsedAudioID = AudioId.MusicMM;
    if (this.UniqueKotanStorage.NextToUnlock() == UniqueKotanStorage.AllUnlocked)
    {
      UsedAudioID = AudioId.MusicEndGame;

      if (!Game.Settings.LetterFromMeowShown)
      {
        Game.Settings.LetterFromMeowShown = true;
        Game.UiManager.Open(PopupId.LetterFromMeow);
      }
      
    }

    DateTime dt = DateTime.Now;
    if (dt.DayOfWeek == DayOfWeek.Friday)
    {
      if (dt.Day == 13)
      {
        UsedAudioID = AudioId.MusicMMH;
        this.MrCat.SetAlpha(0.6f);
        this.MainMenuHelloweenManager.StartHelloween();
      }
    }

    if (dt.Month == 11)
    {
      if (dt.Day > 29)
      {
        UsedAudioID = AudioId.MusicMMH;
        this.MrCat.SetAlpha(0.6f);
        this.MainMenuHelloweenManager.StartHelloween();
      }
    }

    Game.AudioManager.PlayMusic(UsedAudioID, true, 0.6f, true, 1.2f);



    //Game.TutorialManager.ShowStage(TutorialId.Stage2_PressStart, TutorialStage.standard); Depricated stage. We go directly to Actio Phase now.


    // this stage is in conflict with Unlock new cards feature.
    //if (!Game.TutorialManager.WasStageShown(TutorialId.Stage9_FirstUniqueCatUnlocked) )
    //{
    //  if (Game.MenuRoot.UniqueKotanStorage.AtLeastOneUnlocked())
    //  {
    //    Game.TutorialManager.ShowStage(TutorialId.Stage9_FirstUniqueCatUnlocked, TutorialStage.standard);
    //  }
    //}


    this.CheckForCardsUnlock();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void RestartTutorial()
  {
    Game.Settings.IsTutorialActive = true;
    this.OpenActionScene();
  }
  //---------------------------------------------------------------------------------------------------------------
  private void CheckForBuyPopup()
  {
    
    if (!Game.TutorialManager.WasStageShown(TutorialId.Tutorial105_BuyGame))
    {

      if ( this.UniqueKotanStorage.WasCardsUnlockShown() )
      {

        if (Game.MenuRoot.UniqueKotanStorage.IsItTimeToBuyGame())
        {
          Game.TutorialManager.MarkStageDone(TutorialId.Tutorial105_BuyGame);
          Game.UiManager.Open(PopupId.Tutorial105_BuyGame);
        }
      }
    }
    else
    {

    }
  }

  //---------------------------------------------------------------------------------------------------------------
  private void CheckForCardsUnlock()
  {

    if (this.UniqueKotanStorage.WasCardsUnlockShown())
    {
      CheckForBuyPopup();
      return;
    }

    if (!this.UniqueKotanStorage.HasAnythingToUnlock())
    {
      this.UniqueKotanStorage.SetCardsUnlockShown();
      CheckForBuyPopup();
      return;
    }

    Game.UiManager.Open(PopupId.UnlockCardsPopUp);

  }


  //---------------------------------------------------------------------------------------------------------------
  IEnumerator ActivateIAPBtn()
  {
    while (!Game.IAPManager.Initiated)
    {
      yield return new WaitForSeconds(.03f);
    }

    if (Game.Settings.IsPremium)
    {
      this.BuyGameBtn.gameObject.SetActive(false);
    }
    else
    {
      this.BuyGameBtn.interactable = true;
    }
    

  }

    //---------------------------------------------------------------------------------------------------------------
    public void OnTutorialPutCatBackToChair()
  {
    this.MrCat.SetAlpha(1.0f);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OnTutorialEnableGameStart()
  {
    this.StartBtn.SetActive(true);
   
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OpenBuyMe()
  {
    Debug.Log("Buy me opened");
    if (Game.Settings.IsPremium)
    {
      Game.UiManager.Open(PopupId.ThankYou);
    }
    else
    {
      Game.UiManager.Open(PopupId.BuyGame);
    }
    
  }
  //---------------------------------------------------------------------------------------------------------------
  public void OnScoreClick()
  {
    Game.UiManager.Open(PopupId.HintScore);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OpenMenu()
  {
    Game.UiManager.Open(PopupId.GameMenu);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void RestartMenuScene()
  {
   
    Game.StateManager.SetState(GameState.Menu, true, true);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OpenActionScene()
  {
 //   Game.AudioManager.StopAllMusic();
    Game.StateManager.SetState(GameState.Action);
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void Unload()
  {
    Game.MenuRoot = null;
    base.Unload();
  }


  //---------------------------------------------------------------------------------------------------------------
  public void CustomKotanBook()
  {

    Game.AudioManager.PlaySound(AudioId.BookOpen);
    Game.UiManager.Open(PopupId.CatBook);
  }


  //---------------------------------------------------------------------------------------------------------------
  public void MenuDeckBook()
  {

    Game.AudioManager.PlaySound(AudioId.BookOpen);
    Game.UiManager.Open(PopupId.MenuDeck);
  }
}
