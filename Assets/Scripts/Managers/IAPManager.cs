using System.Collections;
using EasyMobile;
using UnityEngine;

public class IAPManager : MonoBehaviour
{
    private bool active = false;

    public bool Active
    {
        get { return active; }
    }

    private bool initiated = false;

    public bool Initiated
    {
        get { return initiated; }
    }

    //---------------------------------------------------------------------------------------------------------------
    private void Start()
    {
        this.Init();
    }

    //---------------------------------------------------------------------------------------------------------------
    public void Init()
    {
        if (this.Active)
        {
            return;
        }


        this.active = true;
        StartCoroutine(this.VerifyIAPS());
        InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    //---------------------------------------------------------------------------------------------------------------
    void PurchaseCompletedHandler(IAPProduct product)
    {
        switch (product.Name)
        {
            case EM_IAPConstants.Product_Buy_Game:
                Debug.Log("Product_Buy_Game was purchased.");
                Game.Settings.IsPremium = true;
                Game.UiManager.Open(PopupId.ThankYou);
                
                if (InAppPurchasing.StoreController != null)
                {
                    var iapProduct = InAppPurchasing.StoreController.products.WithID(product.Id);
                    if (iapProduct != null)
                    {
                        Game.AnalyticsManager.OnPurchase(iapProduct);
                    } 
                }
                break;
            default:
                Debug.Log("Purchase complete was returned, but no product was found!");
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------------------
    void PurchaseFailedHandler(IAPProduct product)
    {
        Debug.Log("The purchase of product " + product.Name + " has failed.");
    }

    //---------------------------------------------------------------------------------------------------------------
    public void OniOSRestore()
    {
#if !UNITY_IOS
        Debug.Log("OniOSRestore was called, but build is not for iOS. Action Canceled.");
        return;
#endif
        InAppPurchasing.RestorePurchases();
    }

    //---------------------------------------------------------------------------------------------------------------
    public void OnDestroy()
    {
        // Not actually needed, bc IAP manager works all the time App works, but let's make clear code.  
        InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
        InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;
    }

    //---------------------------------------------------------------------------------------------------------------
    IEnumerator VerifyIAPS()
    {
        //   int AttemptsCounter = 0;
        while (!InAppPurchasing.IsInitialized())
        {
            yield return new WaitForSeconds(.03f);
            //  AttemptsCounter++;
        }

        this.initiated = true;

        Debug.Log("---- Products ----");
        IAPProduct[] products = InAppPurchasing.GetAllIAPProducts();
        // Print all product names
        foreach (IAPProduct prod in products)
        {
            Debug.Log("Product name: " + prod.Name);
        }

        Debug.Log("--------------");


        if (InAppPurchasing.IsProductOwned(EM_IAPConstants.Product_Buy_Game))
        {
            Game.Settings.IsPremium = true;
            Debug.Log("Game is premium.");
        }
        else
        {
            Game.Settings.IsPremium = false;
            Debug.Log("Game is fremium.");
        }
    }
}