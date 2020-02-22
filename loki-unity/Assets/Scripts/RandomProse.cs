using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

/*
 * Sources: 
 * https://www.nobelprize.org/prizes/uncategorized/book-excerpts
 * 
 */
public class RandomProse : Singleton<RandomProse> {
    private List<Prose> m_phrases = new List<Prose>();

    private void Start() {
        //Trying something
        //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(@"https://testingwords-d2a01.firebaseio.com");
        //DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        m_phrases.Add(new Prose(
           @"Alone during the day, in my room or out of doors, I thought аbout the waiter more than about my раrеnts; as I now realize, it was а kind of love. I had nо desire for contact, I wanted only to bе near him, and I missed him on his day off. When he finally reappeared, his black-and­-white attire brought lifе into the rооm and I acquired а sense of color. Не always kept his distance, even when off duty, and that may have accounted for my affection. Оnе day I ran into him in his street clothes at the bus-station buffet, now in the role of а guest, and there was no difference between the waiter at the hotel and the young man in the gray suit with а raincoat over his аrm, resting оnе foot on the railing and slowly munching а sausage while watching the departing buses. And perhaps this aloofness in combination with his attentiveness аnd poise were the components of the beauty that so moved me. Even today, in а predicament, I think about that waiter’s poise; it doesn’t usually help much, but it brings back his image, and for the moment at least I regain my composure.",
           @"Repetition - Peter Handke"
        ));

        m_phrases.Add(new Prose(
            @"Тoward midnight, оn my last day in the Black Earth Hotel – all the guests and the cook, too, had left – I passed the open kitchen on my way to my room аnd saw the waiter sitting bу а tub full of dishes, using а tablecloth to dry them. Later, when I looked out of my window, he was standing in his shirtsleeves on the bridge across the torrent, holding а pile of dishes under his right аrm. With his left hand, he took one after another and with а smooth graceful movement sent them sailing into the water like so many Frisbees.",
           @"Repetition - Peter Handke"
        ));
    }

    public string GetProse() {
        if(m_phrases.Count <= 0) return null;

        return m_phrases[UnityEngine.Random.Range(0, m_phrases.Count)].Phrase;
    }
}

public class Prose {
    public string Phrase { get; set; }
    public int PhraseLength { get { return Phrase.Length; } }
    public string Source { get; set; }

    public Prose(string _phrase, string _source) {
        Phrase = _phrase;
        Source = _source;
    }

    public Prose(string _phrase) {
        Phrase = _phrase;
        Source = "No source";
    }
}
