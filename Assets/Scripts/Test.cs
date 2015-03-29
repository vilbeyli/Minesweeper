using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{

    public int AAA = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}


[System.Serializable]
public class TestField
{
    [SerializeField]
    public int a = 0;

    [SerializeField]
    public float b = 0.5f;

    [SerializeField]
    public bool c = false;

}
