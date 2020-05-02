using System;
using Assets.Scripts.Utils.ExtensionMethods;
using UnityEngine;
using UnityEngine.Advertisements;

public enum AdsType
{
  [Description("video")]
  VIDEO,
  [Description("rewardedVideo")]
  REWARDED_VIDEO,
}

public class AdsManager
{
  private const AdsType DefaultPlacementID = AdsType.REWARDED_VIDEO;

  private Action onAdsWatchedCallback;
  private AdsType _watchedType = AdsType.REWARDED_VIDEO;
  
  public bool IsAvailable(AdsType type = DefaultPlacementID)
  {
#if UNITY_STANDALONE_WIN
    return true;
#else
    return Advertisement.IsReady(type.GetDesc());
#endif
  }

  public void Show(Action callback, AdsType type = DefaultPlacementID)
  {
    onAdsWatchedCallback = callback;

    if (!IsAvailable(type))
    {
      return;
    }

#if UNITY_STANDALONE_WIN
    onAdsWatchedCallback.Invoke();
#else
    _watchedType = type;
    Advertisement.Show(type.GetDesc(), new ShowOptions()
    {
      resultCallback = OnAdsWatched
    });
#endif
  }

#if !UNITY_STANDALONE_WIN
  private void OnAdsWatched(ShowResult result)
  {
    if (result == ShowResult.Finished)
    {
      onAdsWatchedCallback.Invoke();
      AnalyticsManager.Instance.OnWatchAd(_watchedType.ToString());
      return;
    }

    if (result == ShowResult.Failed)
    {
      Debug.Log("Ad failed");
    }

    if (result == ShowResult.Skipped)
    {
      Debug.Log("Ad skipped");
    }
  }
#endif
}
