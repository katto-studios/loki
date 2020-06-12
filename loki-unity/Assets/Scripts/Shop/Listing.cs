using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

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
        PersistantCanvas.Instance.ConfirmationPanel("Confirm spend " + price + " on " + ListingName + "?", confirm, null);
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
                if(_result.FunctionResult != null)
                {
                    PlayfabMessage msg = PlayFabSimpleJson.DeserializeObject<PlayfabMessage>(_result.FunctionResult.ToString());
                    DecodeMessage(msg);
                    Debug.Log(msg.FunctionMessage);
                } else
                {
                    PersistantCanvas.Instance.ConfirmationPanel("oof, an error has occured", null);
                }
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    void DecodeMessage(PlayfabMessage msg)
    {
        if(msg.FunctionMessage == "Dupe!")
        {
            PersistantCanvas.Instance.ConfirmationPanel("You rolled a duplicate", null);
        } else if (msg.FunctionMessage == "Not enough scrap!")
        {
            PersistantCanvas.Instance.ConfirmationPanel("You have not enough scrap", null);
        } else {
            ArtisanKeycap kc = KeycapDatabase.Instance.getKeyCapFromID(msg.FunctionMessage);
            ColourPack cp = ColourPackDatabase.Instance.GetColourPackFromID(msg.FunctionMessage);
            if(kc != null)
            {
                PersistantCanvas.Instance.GachaScene(kc);
            }
            else if(cp != null)
            {
                PersistantCanvas.Instance.GachaScene(cp);
            }
            else
            {
                PersistantCanvas.Instance.ConfirmationPanel("Something Went Wrong", null);
            }
        }
    }
}
