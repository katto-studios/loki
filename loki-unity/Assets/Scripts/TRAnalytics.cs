﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TRAnalytics : MonoBehaviour
{

    public float timeSinceStart;
    public bool hasStarted;
    TypeGameManager typeGameManager;

    public GameObject analyticsPanel;
    public TextMeshProUGUI WPMText;
    public TextMeshProUGUI ACCText;
    public TextMeshProUGUI MISSText;

    // Start is called before the first frame update
    void Start()
    {
        typeGameManager = TypeGameManager.Instance;
    }

    public void UpdateInput()
    {
        if (!hasStarted)
        {
            hasStarted = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted)
        {
            timeSinceStart += Time.deltaTime;
        }
    }

    public void GameComplete()
    {
        hasStarted = false;
        analyticsPanel.SetActive(true);

        float WPM = typeGameManager.words.Count / (timeSinceStart / 60);
        string WPMString = string.Format("{0:00.0}", WPM);
        WPMText.text = WPMString;

        float ACC = (1 - ((float)typeGameManager.mistakeWords.Count / (float)typeGameManager.words.Count)) * 100;
        string ACCString = string.Format("{0:00.0}", ACC);
        ACCText.text = ACCString;

        string MISSString = typeGameManager.mistakeWords.Count.ToString();
        MISSText.text = MISSString;
    }
}