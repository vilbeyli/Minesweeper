using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using UnityEngine.UI;

public class Database : MonoBehaviour {
 
    //======================================
    // Variable Declarations
    
    // static variables
	
    // handles
	
    // public variables
    public float TimeoutLength;

    // private variables
    private string TopScoresURL = "http://ilbeyli.byethost18.com/leaderboard/topscores.php";
 
    //======================================
    // Function Definitions
 
    // getters & setters
 
    // unity functions
	void Awake ()
	{
	
	}
	
	void Start ()
	{

	}
	
	void Update () 
    {
	
	}
 
    // member functions
    public void GetScores(List<List<Score>> highScores)
    {
        StartCoroutine(GetScoresRoutine(highScores));
    }

    IEnumerator GetScoresRoutine(List<List<Score>> highScores)
    {
        float timeout = Time.time + TimeoutLength;

        StartCoroutine(ReadScoresFromDB(highScores));

        // wait until DB request is successful or timeout
        while (!ScoreManager.DBReadSuccessful)
        {   
            yield return new WaitForSeconds(0.01f);
            if (Time.time >= timeout)
            {
                Debug.Log("DATABASE:: Time Out!");
                // TODO: Timeout Indicator in highScores
                break;
            }
        }

        Debug.Log("DBRead: " + ScoreManager.DBReadSuccessful);

        GetComponent<ScoreManager>().LoadScoresToUI();
    }

    IEnumerator ReadScoresFromDB(List<List<Score>> highScores)
    {
        WWW GetScoresAttempt = new WWW(TopScoresURL);
        yield return GetScoresAttempt;

        if (GetScoresAttempt.error != null)
        {
            Debug.Log(string.Format("ERROR GETTING SCORES: {0}", GetScoresAttempt.error));
            ScoreManager.DBReadSuccessful = false;

            highScores[1].Add(new Score("ERROR GETTING SCORES", -1));
        }
        else
        {
            Debug.Log("Good: ");
            ScoreManager.DBReadSuccessful = true;

            // textlist: (Name/Score/Difficulty) * ScoreCount
            string[] textlist = GetScoresAttempt.text.Split(new string[] { "\n", "\t" },
                StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in textlist)
                ;//Debug.Log(s);


            // iteration count = textlist length/3 (Name/Score/Difficulty)
            for (int i = 0; i < textlist.Length; i += 3)
            {
                // according to the difficulty, populate the highscores
                switch (textlist[i + 2])
                {
                    case "beginner":            //      NAME                        SCORE
                        highScores[0].Add(new Score(textlist[i], float.Parse(textlist[i + 1])));
                        break;
                    case "intermediate":
                        highScores[1].Add(new Score(textlist[i], float.Parse(textlist[i + 1])));
                        break;
                    case "expert":
                        highScores[2].Add(new Score(textlist[i], float.Parse(textlist[i + 1])));
                        break;
                }
            }
        }
        
    }

   
}
