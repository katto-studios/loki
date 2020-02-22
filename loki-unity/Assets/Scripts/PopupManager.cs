using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PopupManager : Singleton<PopupManager>
{
    TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowPopUp(string text, float seconds)
    {
        textMesh.text = text;
        transform.DOMoveX(150, 0.5f);
        Sequence sequence = DOTween.Sequence();
        sequence.PrependInterval(0.5f + seconds);
        sequence.Append(transform.DOMoveX(-150, 0.5f));
    }
}
