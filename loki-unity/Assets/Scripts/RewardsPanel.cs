using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardsPanel : MonoBehaviour
{
    public Text exp;
    public TextMeshProUGUI scrap;
    public void Init(int score)
    {
        Debug.Log(score);
        exp.text = "EXP " + (int)((float)score / 150000f * 200f);
        scrap.text = "" + (int)((float)score * (40f / 150000f));
    }
}
