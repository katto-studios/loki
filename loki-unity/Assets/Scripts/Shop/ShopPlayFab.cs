using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class ShopPlayFab
{
    public static event Action<string> PurchaseStarted;
    public static event Action<string> PurchasePaying;
    public static event Action<string> Error;

    public static void StartPurchase(string itemId)
    {
        PlayFabClientAPI.StartPurchase(new StartPurchaseRequest()
        {
            Items = new List<ItemPurchaseRequest>() {
            new ItemPurchaseRequest() {
                ItemId = itemId,
                Quantity = 1,
                Annotation = "Purchased via in-game store"
            }
        }
        }, result => {
            // Handle success
            PurchaseStarted?.Invoke(result.OrderId);
        }, error => {
            // Handle error
            Error?.Invoke(error.GenerateErrorReport());
        });
    }

    public static void PayForPurchase(string orderId)
    {
        PlayFabClientAPI.PayForPurchase(new PayForPurchaseRequest()
        {
            OrderId = orderId,
            ProviderName = "PayPal",
            Currency = "RM"
        }, result => {
            // Handle success
            ConfirmPurchase(result.OrderId);
            PurchasePaying?.Invoke(result.PurchaseConfirmationPageURL);
        }, error => {
            // Handle error
            Error?.Invoke(error.GenerateErrorReport());
        });
    }

    public static void ConfirmPurchase(string orderId)
    {
        PlayFabClientAPI.ConfirmPurchase(new ConfirmPurchaseRequest()
        {
            OrderId = orderId
        }, result => {
            // Handle success
        }, error => {
            // Handle error
        });
    }
}
