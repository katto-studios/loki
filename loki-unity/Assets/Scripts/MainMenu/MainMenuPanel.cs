using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    public float offset;
    float originalX;
    RectTransform rectTransform;
    public bool isVisible = false;
    public GameObject toggleButton;

    // Start is called before the first frame update
    void Start()
    {
        originalX = transform.position.x;
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClick()
    {
        ButtonChime.Instance.PlayChime();
        if (isVisible)
        {
            isVisible = false;
            transform.DOMoveX(originalX, 0.5f);
            toggleButton.transform.DORotate(new Vector3(0,0,-90), 1f);
        } else
        {
            isVisible = true;
            transform.DOMoveX(originalX + offset, 0.5f);
            toggleButton.transform.DORotate(new Vector3(0, 0, 90), 1f);
        }
    }
}
