using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class Listing : MonoBehaviour
{
    public string ListingName;
    public string CatalogName;
    public string price;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        ConfirmationPanel.CPCallback confirm = ConfirmPurchase;
        ConfirmationPanel.CPCallback cancel = CancelPurchase;
        PersistantCanvas.Instance.ConfirmationPanel("Confirm spend " + price + " on " + ListingName + "?", confirm, cancel);
    }

    public void ConfirmPurchase()
    {
        PlayFabClientAPI.ExecuteCloudScript(
            new ExecuteCloudScriptRequest()
            {
                FunctionName = "GachaScrap",
                FunctionParameter = new { CatalogVer = CatalogName }
            },
            (_result) => {
                Debug.Log(_result.FunctionResult.ToString());
                ConfirmationPanel.CPCallback confirm = ConfirmGetItem;
                string itemName = _result.FunctionResult.ToString();
                PersistantCanvas.Instance.ConfirmationPanel("Congradulations, you won " + itemName + "." , confirm);

                
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    public void ConfirmGetItem()
    {

    }

    public void CancelPurchase()
    {

    }
}
