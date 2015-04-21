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

    // public variables
    public float DBRetryInterval;
 
 
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
        return "\t" + (i + 1) + "\t\t" + score.Name + "\t\t\t" + score.TimePassed + "\n\n";
    }
}