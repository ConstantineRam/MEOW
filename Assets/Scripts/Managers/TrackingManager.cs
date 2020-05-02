public class TrackingManager
{

  public readonly TrackingSignal<int> Score = new TrackingSignal<int>("Score");

  public TrackingManager()
  {
#if UNITY_ANDROID

#elif UNITY_STANDALONE_WIN

#elif UNITY_IOS

#elif UNITY_WEBGL

#endif
  }

  private void OnGameClosed()
  {
        
  }
}


