using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPanel : MonoBehaviour
{
    public delegate void CPCallback();

    public Text text;
    public Button confirmButton;
    public Button cancelButton;

    private CPCallback confirmAction;
    private CPCallback cancelAction;

    public void Init(string t, CPCallback confirm, CPCallback cancel)
    {
        text.text = t;
        confirmAction = confirm;
        cancelAction = cancel;
        cancelButton.gameObject.SetActive(true);
    }

    public void Init(string t, CPCallback confirm)
    {
        text.text = t;
        confirmAction = confirm;
        cancelButton.gameObject.SetActive(false);
    }

    public void Confirm()
    {
        confirmAction?.Invoke();
        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        cancelAction?.Invoke();
        gameObject.SetActive(false);
    }
}
