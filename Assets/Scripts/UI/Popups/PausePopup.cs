//using EasyMobile;
using UnityEngine;
using UnityEngine.UI;
public class PausePopup : GenericPopup
{
  [SerializeField]
  private MyToggleController musicToggle;
  [SerializeField]
  private MyToggleController soundToggle;
  [SerializeField]
  private RectTransform ExitBtn;
  [SerializeField]
  private RectTransform RestartBtn;
  [SerializeField]
  private GameObject IntroBtn;
  [SerializeField]
  private GameObject TutorialRestartBtn;

    [SerializeField]
    private GameObject RestoreBtn;

    [SerializeField]
  private Text VersionText;

  private PausePopupData data = new PausePopupData();

  public class PausePopupData
  {
    public bool isOpenedFromActionPhase = false;
  }


  public override void Activate(object data)
  {

#if !UNITY_IOS
        this.RestoreBtn.SetActive(false);
#endif
        this.VersionText.text = "v. " + Application.version;
    if (data != null && data is PausePopupData)
    {
      this.data = (PausePopupData) data;
    }
        
    if (musicToggle != null)
    {
      musicToggle.SetState(Game.Settings.MusicEnabled);
    }
    if (soundToggle != null)
    {
      soundToggle.SetState(Game.Settings.SoundEnabled);
    }

    if (Game.StateManager.CurrentState == GameState.Menu)
    {
      this.ExitBtn.gameObject.SetActive (false);
      this.RestartBtn.gameObject.SetActive(false);
     
    }

    if (Game.StateManager.CurrentState == GameState.Action)
    {
     
      this.IntroBtn.SetActive(false);
      this.TutorialRestartBtn.SetActive(false);
            this.RestoreBtn.SetActive(false);
    }
  }

  public override void CloseSelf()
  {
    base.CloseSelf();
  }

  public void SeeIntro()
  {
    Game.MenuRoot.SeeIntro();
    this.CloseSelf();
  }

  public void OnSoundChanged(bool value)
  {
    Game.Settings.SoundEnabled = value;
  }

  public void OnMusicChanged(bool value)
  {
    Game.Settings.MusicEnabled = value;
  }


  public void OnRestartTutorialClicked()
  {
    Game.MenuRoot.RestartTutorial();
  }

  public void OnExitClick()
  {
    //GameServices.ReportScore(Game.ActionRoot.actionPhaseSingleSessionDataStorage.DefeatedBlobs, EM_GameServicesConstants.Leaderboard_BlobsInOneRun);
    //GameServices.ReportScore(Game.ActionRoot.Score, EM_GameServicesConstants.Leaderboard_BestScore);

    Game.StateManager.SetState(GameState.Menu);
  }

  public void OnResumeClick()
  {

    CloseSelf();
  }
    public void OnRestoreIAPClicked()
    {
        Game.IAPManager.OniOSRestore();
    }
    public void OnRestartClick()
  {
    //GameServices.ReportScore(Game.ActionRoot.actionPhaseSingleSessionDataStorage.DefeatedBlobs, EM_GameServicesConstants.Leaderboard_BlobsInOneRun);
    //GameServices.ReportScore(Game.ActionRoot.Score, EM_GameServicesConstants.Leaderboard_BestScore);



    Game.StateManager.SetState(GameState.Action, true, true);
  }



}
