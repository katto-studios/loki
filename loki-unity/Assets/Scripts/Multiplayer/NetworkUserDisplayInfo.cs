using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NetworkUserDisplayInfo : MonoBehaviour {
    [Header("Displays")]
    public TextMeshProUGUI displayNameText;
    public TextMeshProUGUI displayScoreText;
    public Slider displayProgress;
    [Header("Unique values")]
    public float spacing = 50;

    // Start is called before the first frame update
    void Start() {
        //transform.position = Vector3.up * (spacing * (transform.GetSiblingIndex() + 1));
        displayProgress.value = 0;
    }

    // Update is called once per frame
    void Update() {

    }

    public void SetName(string _name) {
        displayNameText.SetText(_name.ToUpper());
    }

    public void SetScore(float _score) {
        displayScoreText.SetText(string.Format("Score: {0}", _score.ToString()));
    }

    public void UpdateProgress(float _progress) {
        displayProgress.value = _progress;
    }

    public void Swap(NetworkUserDisplayInfo _other) {
        Vector3 temp = _other.transform.position;
        _other.transform.position = transform.position;
        transform.position = temp;
    }
}
