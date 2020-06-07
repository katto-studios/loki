using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ShopPlayFab.PurchaseStarted += PurchaseStarted;
        ShopPlayFab.PurchasePaying += PurchasePaying;
        ShopPlayFab.Error += Error;
        ShopPlayFab.StartPurchase("BLACK_MATTER");
    }

    public void PurchaseStarted(string orderId)
    {
        Debug.Log("PurchaseStarted " + orderId);

        ShopPlayFab.PayForPurchase(orderId);
    }

    public void PurchasePaying(string cfmurl)
    {
        Debug.Log("PurchaseStarted " + cfmurl);

        Application.OpenURL(cfmurl);
    }


    public void Error(string err)
    {
        Debug.LogWarning(err);  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
