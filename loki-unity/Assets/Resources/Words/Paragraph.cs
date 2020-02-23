using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProseInput", menuName = "Prose")]
public class Paragraph : ScriptableObject {
    public string Prose;
    public string Author;
    public string Source;

    public Paragraph() { }
    public Paragraph(string _prose, string _author, string _soruce) {
        Prose = _prose;
        Author = _author;
        Source = _author;
    }
}
