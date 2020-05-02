public class Events
{
  public readonly Signal GameStarted = new Signal();
  public readonly Signal GameClosed = new Signal();

  // Ingame state changes
  public readonly Signal GameWon = new Signal();
  public readonly Signal GameLost = new Signal();
  public readonly Signal<int> ReportScore = new Signal<int>();

  // Settings

  public readonly Signal<bool> MusicEnabled = new Signal<bool>();
  public readonly Signal<bool> SoundEnabled = new Signal<bool>();
  public readonly Signal<int> BestScoreChanged = new Signal<int>();
  public readonly Signal<LanguageKind> LanguageChanged = new Signal<LanguageKind>();

  // SHARING
  public readonly Signal ScreenshotTaken = new Signal();
  

  // Cards
  public readonly Signal<CardHolderController, ACard> ReceivedACard = new Signal<CardHolderController, ACard>();
  public readonly Signal<float, float> DragedOver = new Signal<float, float>();
  public readonly Signal DragStarted = new Signal();
  public readonly Signal DragEnded = new Signal();
  public readonly Signal OnCardMergeStarted = new Signal();
  public readonly Signal OnCardMergeEnded = new Signal();

  

  // M.E.O.W.
  public readonly Signal SessionOverAllTasksDone = new Signal();
  public readonly Signal SessionOverFailure = new Signal();

  public readonly Signal SummoningFinalized = new Signal();

  public readonly Signal SwipeLeft = new Signal();
  public readonly Signal SwipeRight = new Signal();
}
