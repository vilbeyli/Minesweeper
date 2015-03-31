using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{

    public Camera Camera;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void OnToggle(bool val)
    {
        Camera.GetComponent<AudioListener>().enabled = val;
    }
}
