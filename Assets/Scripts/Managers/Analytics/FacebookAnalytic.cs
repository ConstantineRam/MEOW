using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine.Purchasing;

public class FacebookAnalytic : IAnalyticsInstance
{
    public void OnWatchAd(Dictionary<string, object> parameters)
    {
        parameters[AppEventParameterName.ContentType] = parameters["ContentType"];
        parameters[AppEventParameterName.ContentID] = parameters["ContentType"];
        parameters[AppEventParameterName.Currency] = "USD";
        FB.LogAppEvent(AppEventName.ViewedContent, 0, parameters);
    }

    public void OnFirstWatchAd(Dictionary<string, object> parameters)
    {
        parameters[AppEventParameterName.ContentType] = parameters["ContentType"];
        FB.LogAppEvent("firstAdViewed", 0, parameters);
    }

    public void OnPurchase(Product product, int count)
    {
        var iapParameters = new Dictionary<string, object>();
        iapParameters["productId"] = product.definition.id;
        FB.LogPurchase(
            (float) product.metadata.localizedPrice,
            product.metadata.isoCurrencyCode,
            iapParameters
        );
    }

    public void OnFirstPurchase(Product product)
    {
        var parameters = new Dictionary<string, object>();
        parameters["item_id"] = product.definition.id;
        FB.LogAppEvent("firstPurchased", 0, parameters);
    }

    public void OnCustomEvent(string eventName, Dictionary<string, object> parameters)
    {
        FB.LogAppEvent(eventName, 0, parameters);
    }

    public bool IsInitialized()
    {
        return FB.IsInitialized;
    }
}