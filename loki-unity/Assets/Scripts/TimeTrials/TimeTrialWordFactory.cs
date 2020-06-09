using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

//use this to get a random word
public class TimeTrialWordFactory : MonoBehaviour{
    private int m_index = 0;
    private void Start(){
        GetWords.Initalise(Random.Range(0, int.MaxValue));
    }

    public string GetWord(){
        return GetWords.GetWord().TrimStart().TrimEnd();
        //return "XD";
    }

    public WordLine GetLine(){
        WordLine returnMe = new WordLine();
        for (int count = 0; count < 5; count++){
            returnMe.Line.Add(GetWord());
        }

        return returnMe;
    }
}

public class WordLine{
    public List<string> Line{ get; } = new List<string>();
    private int m_current = 0;
    public int CurrentlyOn => m_current;
    public string CurrentWord => Line[m_current];
    public bool AmOut => m_current + 1 >= Line.Count;

    public void BumpCurrent() => m_current++;

    public override string ToString(){
        StringBuilder sb = new StringBuilder();
        if (m_current != 0){
            sb.Append("<color=green>");
        }

        for (int count = 0; count <= Line.Count - 1; count++){
            sb.AppendFormat("{0} ", Line[count]);
            if (m_current - 1 == count){
                sb.Append("</color>");
            }
        }

        return sb.ToString().TrimEnd();
    }

    public string GetFancy(string _whatIHave){
        StringBuilder sb = new StringBuilder();
        sb.Append("<color=green>");

        for (int count = 0; count <= Line.Count - 1; count++){
            if (m_current == count){
                try{
                    int stopAt = _whatIHave.Length;
                    //append each char as green
                    for (int charCount = 0; charCount < _whatIHave.Length; charCount++){
                        if (_whatIHave[charCount].Equals(CurrentWord[charCount])){
                            sb.Append(_whatIHave[charCount]);
                        }
                        else{
                            stopAt = charCount;
                            break;
                        }
                    }

                    if (stopAt < CurrentWord.Length){
                        sb.Append($"</color><u>{CurrentWord[stopAt]}</u>");
                    }
                    else{
                        sb.Append("</color>");
                    }

                    for (int charCount = stopAt + 1; charCount < CurrentWord.Length; charCount++){
                        sb.Append(CurrentWord[charCount]);
                    }
                }
                catch (Exception){
                    // ignored
                }

                sb.Append(" ");
            }
            else{
                sb.AppendFormat("{0} ", Line[count]);
            }
        }

        return sb.ToString();
    }
}

