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
    private string AddScoreURL = "http://ilbeyli.byethost18.com/leaderboard/addscore.php?";
 
    //======================================
    // Function Definitions
 
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
            ScoreManager.DBReadSuccessful = true;

            // textlist: (Name/Score/Difficulty) * ScoreCount
            string[] textlist = GetScoresAttempt.text.Split(new string[] { "\n", "\t" },
                StringSplitOptions.RemoveEmptyEntries);


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

    public IEnumerator SubmitScore(Score score)
    {

        string privateKey = "pKey";
        string hash = Md5Sum(score.Name + score.TimePassed + score.Difficulty + privateKey);

        Debug.Log("SUBMITTING: " + score.print());
        Debug.Log("Name: " + score.Name + " Escape: " + WWW.EscapeURL(score.Name));

        WWW ScorePost = new WWW(AddScoreURL + "name=" + WWW.EscapeURL(score.Name) + "&score=" + score.TimePassed + "&difficulty=" + score.Difficulty + "&hash=" + hash);
        yield return ScorePost;

        if (ScorePost.error == null)
        {
            // NO ERROR - CONTINUE POST PROCESSING
            Debug.Log("SCORE POSTED SUCECSSFULLY!");
            GetComponent<GameManager>().UI.DisableScoreCanvas();
            ScoreManager.DBReadSuccessful = false;

        }
        else
        {
            Debug.Log("Error posting results: " + ScorePost.error);
        }

        yield return new WaitForSeconds(2);
    }

    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
}
