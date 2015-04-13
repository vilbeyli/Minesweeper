using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void TurnGreen()
    {
        GetComponentInChildren<Text>().color = Color.green;
    }

    public void TurnCyan()
    {
        GetComponentInChildren<Text>().color = Color.cyan;
    }
}
