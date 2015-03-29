using System.CodeDom;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameManager GM;

	// Use this for initialization
	void Start ()
	{

        //Debug.Log("UIMANAGER: ON START()");
	}
	
	// Update is called once per frame
	void Update () 
    {
	    ScanForInput();
	}

    void ScanForInput()
    {
        if (Input.GetKeyDown("escape"))     GM.TogglePauseMenu();
    }

    public void ReadSettings()
    {
        int height, width, mines;
        height = width = mines = 0;

        Debug.Log("UIMANAGER:: H: " + height + " W: " + width + " M: " + mines);

        //GameSettings settings = new GameSettings(9, 9, 10);
        //return settings;
    }
}
