using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour 
{
 
    //======================================
    // Variable Declarations
    
    // static variables
    public static bool DBReadSuccessful;

    // handles
    public Text BeginnerScores;
    public Text IntermediateScores;
    public Text ExpertScores;


    // private variables
    private List<List<Score>> _highScores;
    private float _nextDbAttempt;
    private Score _playerScore;

    // public variables
    public float DBRetryInterval;

    public Score PlayerScore
    {
        get { return _playerScore; }
        set { _playerScore = value; }
    }


    //======================================
    // Function Definitions
	
	void Update () 
    {
	    if (!DBReadSuccessful && Time.time > _nextDbAttempt)
	    {
	        GetHighScores();
	        _nextDbAttempt = Time.time + DBRetryInterval;
	    }
	}
 
    // member functions
    void GetHighScores()
    {
        // create highscore objects
        _highScores = new List<List<Score>>();

        // 3 tables: 1 for each difficulty
        _highScores.Add(new List<Score>()); // 0: Beginner
        _highScores.Add(new List<Score>()); // 1: Intermediate
        _highScores.Add(new List<Score>()); // 2: Expert

        //CreateDummyScores();
        GetComponent<Database>().GetScores(_highScores);

    }

    void CreateDummyScores()
    {
        float baseScore;

        for (int i = 0; i < 3; i++)
        {
            baseScore = (i + 1) * 5;  // 5-6-7... beginner, 10-11-12... intermediate...
            for (int j = 0; j < 10; j++)
            {
                _highScores[i].Add(new Score(baseScore + j));
            }
        }
    }

    public void LoadScoresToUI()
    {
        // construct text to be displayed in UI elements
        String beginnerScoresText, intermediateScoresText, expertScoresText;
        beginnerScoresText = intermediateScoresText = expertScoresText = "";

        for (int j = 0; j < _highScores[0].Count; j++)
            beginnerScoresText += HighScoreFormat(j, _highScores[0][j]);
        for (int j = 0; j < _highScores[1].Count; j++)
            intermediateScoresText += HighScoreFormat(j, _highScores[1][j]);
        for (int j = 0; j < _highScores[2].Count; j++)
            expertScoresText += HighScoreFormat(j, _highScores[2][j]);


        // update UI elements' text fields
        BeginnerScores.text = beginnerScoresText;
        IntermediateScores.text = intermediateScoresText;
        ExpertScores.text = expertScoresText;
    }

    string HighScoreFormat(int i, Score score)
    {

        string s = "\t" + (i+1);      
        s += i == 9 ? "\t" : "\t\t";   // use 1 tab on 2 digits (i==9)
        s += score.Name;

        switch (score.Name.Length)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                s += "\t\t\t\t";
                break;
            case 5:
            case 6:
                s += "\t\t\t";
                break;
            case 7:
            case 8:
            case 9:
                s += "\t\t";
                break;
            default:
                s += "\t";
                break;
        }

        s += score.TimePassed.ToString("0.00") + "\n\n";

        return s;
    }

    // score submission
    public void PostScore(string name)
    {
        _playerScore.Name = name;
        StartCoroutine(GetComponent<Database>().SubmitScore(_playerScore));
    }

}

[Serializable]
public class Score
{
    private float _timePassed;
    private string _name;
    private string _difficulty;

    public string Difficulty
    {
        get { return _difficulty; }
    }

    public float TimePassed
    {
        get { return _timePassed; }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public Score(float timePassed)
    {
        _timePassed = timePassed;
        _name = "anon" + (int)_timePassed;
    }

    public Score(string name, float timePassed)
    {
        _timePassed = timePassed;
        _name = name;
    }

    public Score(float timePassed, string difficulty)
    {
        _timePassed = timePassed;
        _difficulty = difficulty;
    }

    public string print()
    {
        string s = "";

        s += "Name: " + _name + "\n"
             + "Score: " + _timePassed + "\n"
             + "Difficulty: " + _difficulty;

        return s;
    }
}