//using I2.Loc;
using System;
using System.Linq;


public class Settings
{
  [PlayerPrefsValue("isPremium", false)]
  private bool isPremium;
  public bool IsPremium { get { return isPremium; } set { isPremium = value; SaveSettings(); } }

  [PlayerPrefsValue("soundEnabled", true)]
  private bool soundEnabled;
  public bool SoundEnabled { get { return soundEnabled; } set { soundEnabled = value; Game.Events.SoundEnabled.Invoke(soundEnabled); SaveSettings(); } }

  [PlayerPrefsValue("musicEnabled", true)]
  private bool musicEnabled;
  public bool MusicEnabled { get { return musicEnabled; } set { musicEnabled = value; Game.Events.MusicEnabled.Invoke(musicEnabled); SaveSettings(); } }

  [PlayerPrefsValue("BestScore", 0)]
  private int bestScore;
  public int BestScore { get { return bestScore; } set { bestScore = value; Game.Events.BestScoreChanged.Invoke(bestScore); SaveSettings(); } }

  [PlayerPrefsValue("FirstLaunch", true)]
  private bool isFirstLaunch;
  public bool IsFirstLaunch { get { return isFirstLaunch; } set { isFirstLaunch = value; SaveSettings(); } }

  // Tutorial Manager
  [PlayerPrefsValue("TutorialManagerActive", true)]
  private bool isTutorialActive;
  public bool IsTutorialActive { get { return isTutorialActive; } set { isTutorialActive = value; SaveSettings(); } }

  // Tutorial Manager
  [PlayerPrefsValue("ActionTutorialFinished", false)]
  private bool isActionTutorialFinished;
  public bool IsActionTutorialFinished { get { return isActionTutorialFinished; } set { isActionTutorialFinished = value; SaveSettings(); } }

  // Tutorial Manager
  [PlayerPrefsValue("TutorialManagerStage", 0)]
  private int tutorialStage;
  public int TutorialStage { get { return tutorialStage; } set { tutorialStage = value; SaveSettings(); } }

  // Difficulty Manager
  [PlayerPrefsValue("WinCount", 0)]
  private int winCount;
  public int WinCount { get { return winCount; } set { winCount = value; SaveSettings(); } }

  // Freemium Cats
  [PlayerPrefsValue("BonusValue", 1)]
  private int bonusValue;
  public int BonusValue { get { return bonusValue; } set { bonusValue = value; SaveSettings(); } }

  // Freemium Cats
  [PlayerPrefsValue("FreeCats", 5)]
  private int freeCats;
  public int FreeCats { get { return freeCats; } set { freeCats = value; SaveSettings(); } }

  // Cat book
  [PlayerPrefsValue("CatBookMagicCatsEnabled", true)]
  private bool catBookMagicCatsEnabled;
  public bool CatBookMagicCatsEnabled { get { return catBookMagicCatsEnabled; } set { catBookMagicCatsEnabled = value; SaveSettings(); } }

  // Cat book
  [PlayerPrefsValue("CatBookHelpShown", false)]
  private bool catBookHelpShown;
  public bool CatBookHelpShown { get { return catBookHelpShown; } set { catBookHelpShown = value; SaveSettings(); } }


  // GameWon
  [PlayerPrefsValue("FirstKittyHintShown", false)]
  private bool firstKittyHintShown;
  public bool FirstKittyHintShown { get { return firstKittyHintShown; } set { firstKittyHintShown = value; SaveSettings(); } }

  // Letter From Meow Shown
  [PlayerPrefsValue("ActionTutorialFinished", false)]
  private bool letterFromMeowShown;
  public bool LetterFromMeowShown { get { return letterFromMeowShown; } set { letterFromMeowShown = value; SaveSettings(); } }

  //public LanguageKind Language
  //{
   // get
   /// {
 //     LanguageKind kind;
    //  string lng = LocalizationManager.CurrentLanguage;
  //    if (Enum.GetNames(typeof(LanguageKind)).ToList().Contains(lng))
  //    {
 //       kind = (LanguageKind) Enum.Parse(typeof(LanguageKind), lng, true);
  //    }
 //     else
  //    {
  //      kind = LanguageKind.English;
 //     }


 //     return kind;
   // }
 //   set
 //   {
   //   LocalizationManager.CurrentLanguage = value.ToString();
//      Game.Events.LanguageChanged.Invoke(Language);
//    }
 // }

  public Settings()
  {
    LoadSettings();
  }

  public void LoadSettings()
  {
    PlayerPrefsUtils.Fill(this);
  }

  public void SaveSettings()
  {
    PlayerPrefsUtils.Save(this);
  }
}
