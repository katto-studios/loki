using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<Listing> listings;
    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        //ShopPlayFab.PurchaseStarted += PurchaseStarted;
        //ShopPlayFab.PurchasePaying += PurchasePaying;
        //ShopPlayFab.Error += Error;
        //ShopPlayFab.StartPurchase("BLACK_MATTER");

        foreach(Listing listing in listings)
        {
            listing.gameObject.SetActive(false);
        }

        listings[0].gameObject.SetActive(true);
    }

    public void ChangeItem(int index)
    {
        ButtonChime.Instance.PlayChime();
        listings[currentIndex].gameObject.SetActive(false);
        currentIndex = index;
        listings[currentIndex].gameObject.SetActive(true);
    }

    public void ChangeUp()
    {
        int next = currentIndex + 1;
        if (next >= listings.Count)
        {
            next = 0;
        }
        ChangeItem(next);
    }

    public void ChangeDown()
    {
        int next = currentIndex - 1;
        if (next < 0)
        {
            next = listings.Count - 1;
        }
        ChangeItem(next);
    }

    public void Select()
    {
        Select(listings[currentIndex]);
    }

    public void Select(Listing listing)
    {
        listing.Select();
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeDown();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeUp();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            Select();
        }
    }
}
