using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class AnalyticsManager : MonoBehaviour
{
   

    public int TotalAdCount { get; private set; }
    public int TotalPurchaseCount { get; private set; }

  //public static AnalyticsManager Instance; added to Game. Singlenton removed
  public List<IAnalyticsInstance> Instances = new List<IAnalyticsInstance>();

  private const string TotalAdCountSaveKey = "total_ad_count";
    private const string TotalPurchaseCountSaveKey = "total_purchase_count";


  //---------------------------------------------------------------------------------------------------------------
  public void Awake()
    {
        //DontDestroyOnLoad(this); added to Game.
        LoadSave();
    }

  //---------------------------------------------------------------------------------------------------------------
  public void OnWatchAd(string contentType)
    {
        TotalAdCount++;
        if (TotalAdCount == 1)
        {
            OnFirstWatchAd(contentType);
        }

        var dic = new Dictionary<string, object>();
        dic.Add("ContentType", contentType);
        dic.Add("Total", TotalAdCount.ToString());

        foreach (var instance in Instances)
        {
            if (instance.IsInitialized())
            {
                instance.OnWatchAd(dic);
            }
        }
    }

  //---------------------------------------------------------------------------------------------------------------
  public void OnPurchase(Product product)
    {
        TotalPurchaseCount++;

        if (TotalPurchaseCount <= 1)
        {
            foreach (var instance in Instances)
            {
                if (instance.IsInitialized())
                {
                    instance.OnFirstPurchase(product);
                }
            }
        }

        foreach (var instance in Instances)
        {
            if (instance.IsInitialized())
            {
                instance.OnPurchase(product, TotalPurchaseCount);
            }
        }
    }

  //---------------------------------------------------------------------------------------------------------------
  private void OnFirstWatchAd(string contentType)
    {
        var dic = new Dictionary<string, object>();
        dic.Add("ContentType", contentType);

        foreach (var instance in Instances)
        {
            if (instance.IsInitialized())
            {
                instance.OnFirstWatchAd(dic);
            }
        }
    }

  //---------------------------------------------------------------------------------------------------------------
  public void OnTutorialStep(string id)
    {
        var dic = new Dictionary<string, object>();
        dic.Add("Step", id);

        foreach (var instance in Instances)
        {
            if (instance.IsInitialized())
            {
                instance.OnCustomEvent("Tutorial Step", dic);
            }
        }
    }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Event reports finished Tuturial section.
  /// </summary>
  /// <param name="id"></param>
  public void OnTutorialComplete(string id)
    {
        var dic = new Dictionary<string, object>();
        foreach (var instance in Instances)
        {
            if (instance.IsInitialized())
            {
                instance.OnCustomEvent("Tutorial Finish", dic);
            }
        }
    }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Event reports finished the finish of single card game session.
  /// </summary>
  /// <param name="id"></param>
  public void OnLevelComplete(string id)
    {
        var dic = new Dictionary<string, object>();
        dic.Add("Step", id);

        foreach (var instance in Instances)
        {
            if (instance.IsInitialized())
            {
                instance.OnCustomEvent("TutorialStep", dic);
            }
        }
    }

    #region Load

    public void LoadSave()
    {
        TotalAdCount = PlayerPrefs.GetInt(TotalAdCountSaveKey, 0);
        TotalPurchaseCount = PlayerPrefs.GetInt(TotalPurchaseCountSaveKey, 0);
    }

    #endregion
}

public interface IAnalyticsInstance
{
    void OnWatchAd(Dictionary<string, object> parameters);
    void OnFirstWatchAd(Dictionary<string, object> parameters);
    void OnPurchase(Product product, int count);
    void OnFirstPurchase(Product product);
    void OnCustomEvent(string eventName, Dictionary<string, object> parameters);
    bool IsInitialized();
}