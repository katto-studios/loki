using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using TMPro;

public class MultiplayerKeyboard : MonoBehaviour
{
    public NetworkKeyboard keyboard;
    private UserAccountInfo m_accountInfo;
    public TextMeshPro playerNameUI;
    private int currentScore;
    private int targetScore;
    private float speed;
    public string Username { get { return m_accountInfo.Username; } }

    // Start is called before the first frame update
    void Start()
    {
        keyboard = GetComponentInChildren<NetworkKeyboard>();
        playerNameUI = GetComponentInChildren<TextMeshPro>();
    }

    public void Init(List<ItemInstance> items, UserAccountInfo u)
    {
        keyboard = GetComponentInChildren<NetworkKeyboard>();
        m_accountInfo = u;
        playerNameUI.text = u.Username + " 0";
        keyboard.Init(items);
    }

    public void UpdateScore(int score)
    {
        targetScore = score;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentScore < targetScore)
        {
            speed += 10 * Time.deltaTime;
            int newScore = (int)Mathf.Lerp(currentScore, targetScore, Time.deltaTime * speed);
            playerNameUI.text = m_accountInfo.Username + " " + newScore;
            currentScore = newScore;
        } else
        {
            speed = 0;
        }
    }
}
