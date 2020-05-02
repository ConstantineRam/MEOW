using UnityEngine;

public class ActionRoot : RootBase
{
  [SerializeField]
  private Canvas canvas;
  public Canvas Canvas { get { return canvas; } }
  public float GetScaleFactor { get { return Canvas.scaleFactor; } }

  private int CurrentScore;
  private CardManager cardManger;
  public CardManager CardManager { get { return cardManger; } }

  [SerializeField]
  private KotansStorage kotansStorage;
  public KotansStorage KotansStorage { get { return kotansStorage; } }

  
  [SerializeField]
  private ScorePanelController scorePanelController;
  public ScorePanelController ScorePanel { get { return scorePanelController; } }

  [SerializeField]
  private HolderBlock summonBlock;
  public HolderBlock SummonBlock { get { return summonBlock; } }

  [SerializeField]
  private SessionResult resultStorage;
  public SessionResult SessionResult { get { return resultStorage; } }

  [SerializeField]
  private TaskController taskController;
  public TaskController TaskController { get { return taskController; } }

  [SerializeField]
  private MrCat mrCat;
  public MrCat MrCat { get { return mrCat; } }

  [SerializeField]
  private Kotan kotan;
  public Kotan Kotan { get { return kotan; } }

  [SerializeField]
  private CatCardHolderBlock catCardHolderBlock;
  public CatCardHolderBlock CatCardHolderBlock { get { return catCardHolderBlock; } }

  [SerializeField]
  private PlayerHandController playerHandController;
  public PlayerHandController PlayerHandController { get { return playerHandController; } }

  private GameSessionManager gameSessionManager;
  public GameSessionManager GameSessionManager { get { return gameSessionManager; } }

  private UniqueKotanStorage uniqueKotanStorage;
  public UniqueKotanStorage UniqueKotanStorage { get { return uniqueKotanStorage; } }

  private CustomKotanStorage customKotanStorage;
  public CustomKotanStorage CustomKotanStorage { get { return customKotanStorage; } }
  

  
  [SerializeField]
  private Camera camera;
  public Camera Camera { get { return camera; } }


  private DifficultyManager difficultyManager;
  public DifficultyManager DifficultyManager { get { return difficultyManager; } }

  private int TutorialStep =0;

  //---------------------------------------------------------------------------------------------------------------
  private void Awake()
  {
    base.Awake();
    Game.ActionRoot = this;

    this.gameSessionManager = this.GetComponent<GameSessionManager>();
    this.uniqueKotanStorage = this.GetComponent<UniqueKotanStorage>();
    this.difficultyManager = this.GetComponent<DifficultyManager>();

    this.customKotanStorage = this.GetComponent<CustomKotanStorage>();



    this.CurrentScore = 0;

    Game.Events.GameLost.Listen(GameOver);
    Game.Events.GameWon.Listen(GameWon);

    this.cardManger = this.GetComponent<CardManager>();
    if (this.cardManger == null)
    {
      Debug.LogError("Card manager wasn't found in ActionRoot");
    }


  }

  //---------------------------------------------------------------------------------------------------------------
  void Start()
  {
    
    Game.TimerManager.Start(1.2f, () => {
      Game.AudioManager.PlayMusic(AudioId.Music, true, 0.6f, true, 1.2f);
    });
    

    if (Game.Settings.IsFirstLaunch)
    {
      Debug.Log("First Launch detected (action phase).");
      this.EndTheFirstLaunch();
    //  this.ShowTutorial();
    }


  }

  //---------------------------------------------------------------------------------------------------------------
  public void ContinueTutorial()
  {
    if (!Game.Settings.IsTutorialActive)
    {
      Debug.LogError("Continue Tutorial was called, but tutorial is reported as finished.");
      return;
    }
    this.TutorialStep++;
    this.PlayerHandController.TutorialGame(TutorialStep);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void EndTheFirstLaunch()
  {
    Game.Settings.IsFirstLaunch = false;
  }

  //---------------------------------------------------------------------------------------------------------------
  public int GetCurrenScore()
  {
    return this.CurrentScore;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void SetCurrenScore(int newScore)
  {
    this.CurrentScore = newScore;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void GameWon()
  {

    //{ 
    //if (Game.Settings.BestScore == 0)
    //{
    //  Game.Settings.BestScore = this.CurrentScore + 1;
    //}
    //else
    //{
    //  if (Game.Settings.BestScore > this.CurrentScore)
    //  {
    //    Game.Settings.BestScore = this.CurrentScore + 1;
    //  }
    //}

    //
    Game.ActionRoot.DifficultyManager.RegisterWin();
    Game.AudioManager.PlaySound(AudioId.CatSound);
    GameWon.WinPopupData winPopupData = new GameWon.WinPopupData();
    winPopupData.score = this.ScorePanel.TotalPoints;
    Game.UiManager.Open(PopupId.GameWon, winPopupData);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void GameOver()
  {
    
    Game.AudioManager.PlaySound(AudioId.ArcadeJingle);
    Game.UiManager.Open(PopupId.GameOver);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void CustomKotanBook()
  {

    Game.AudioManager.PlaySound(AudioId.BookOpen);
    Game.UiManager.Open(PopupId.CustomKotanCollectionPopUp);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void NextTurnHintPopUp()
  {

    //  Game.AudioManager.PlaySound(AudioId.BookOpen);
    Game.UiManager.Open(PopupId.NextTurnHintPopUp);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void HintHelpPopUp()
  {

  //  Game.AudioManager.PlaySound(AudioId.BookOpen);
    Game.UiManager.Open(PopupId.HintHelpPopUp);
  }

  
  //---------------------------------------------------------------------------------------------------------------
  public void UniqueKotanBook()
  {

    Game.AudioManager.PlaySound(AudioId.ArcadeJingle);
    UniqueKotanPopUp.UniqueKotanPopupData popUpData = new UniqueKotanPopUp.UniqueKotanPopupData(UniqueKotanPopUp.UniqueKotanPopupData.NO_DATA_SET);

    Game.UiManager.Open(PopupId.UniqueKotans, popUpData);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ShowTutorial()
  {

    //Game.UiManager.Open(PopupId.Tutorial);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OpenMenu()
  {
    Game.UiManager.Open(PopupId.GameMenu);
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void Unload()
  {
    Game.ActionRoot = null;
    base.Unload();
  }
}
