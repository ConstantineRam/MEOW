using System;
using UnityEngine;
using EasyMobile;
using UnityEngine.Scripting;

public class Game : MonoBehaviour
{
  public static Boolean InitStarted { get; set; }
  public static GameState PreferredFirstState { get; set; }


    [Preserve] public static StateManager StateManager { get; private set; }
    [Preserve] public static Settings Settings { get; private set; }
    [Preserve] public static AudioManager AudioManager { get; private set; }
    [Preserve] public static PoolManager PoolManager { get; private set; }
    [Preserve] public static UIManager UiManager { get; private set; }
    [Preserve] public static Events Events { get; private set; }
    [Preserve] public static TimeScaleProvider TimeScale { get; private set; }
    [Preserve] public static TimerManager TimerManager { get; private set; }
    [Preserve] public static CoroutineProvider CoroutineProvider { get; private set; }
    [Preserve] public static TrackingManager Tracking { get; private set; }
    [Preserve] public static AdsManager AdsManager { get; private set; }
    [Preserve] public static TutorialManager TutorialManager { get; private set; }
    [Preserve] public static IAPManager IAPManager { get; private set; }

    [Preserve] public static MenuRoot MenuRoot { get; set; }
    [Preserve] public static ActionRoot ActionRoot { get; set; }

    [Preserve] public static AnalyticsManager AnalyticsManager { get; set; }



  public static bool IsDebug { get { return Debug.isDebugBuild; } }

  //---------------------------------------------------------------------------------------------------------------
  void Awake()
  {
    InitStarted = true;
    DontDestroyOnLoad(gameObject);


    Application.runInBackground = true;
    Application.backgroundLoadingPriority = ThreadPriority.Low;
    Screen.sleepTimeout = SleepTimeout.NeverSleep;
#if UNITY_STANDALONE
    Screen.SetResolution((int) (Screen.height * 9 / 16f), Screen.height, false);
#endif
    Application.targetFrameRate = 60;


    // Easy Mobile part. Remove if not used.
    if (!RuntimeManager.IsInitialized())
    {
      Debug.Log("starting EasyMobile");
      RuntimeManager.Init();

      if (!RuntimeManager.IsInitialized())
      {
        Debug.LogError("Easy Mobile not initialized.");
      }
    }


    StateManager = GetComponentInChildren<StateManager>();
    AudioManager = GetComponentInChildren<AudioManager>();
    PoolManager = GetComponentInChildren<PoolManager>();
    UiManager = GetComponentInChildren<UIManager>();
    TutorialManager = GetComponentInChildren<TutorialManager>();
    IAPManager = GetComponentInChildren<IAPManager>();
    AnalyticsManager = GetComponentInChildren<AnalyticsManager>();

    Settings = new Settings();
    Events = new Events();
    CoroutineProvider = new CoroutineProvider(this);
    TimeScale = new TimeScaleProvider();
    TimerManager = new TimerManager(TimeScale);
    Tracking = new TrackingManager();
    AdsManager = new AdsManager();



    Events.GameStarted.Invoke();

    StateManager.SwitchToFirstState();


  }

  //---------------------------------------------------------------------------------------------------------------
  void Update()
  {
    if (TimeScale != null)
      TimeScale.Update();
  }

  //---------------------------------------------------------------------------------------------------------------
  void OnApplicationQuit()
  {
    Events.GameClosed.Invoke();
  }


}
