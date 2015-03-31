using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    private Vector2 _gridPosition = Vector2.zero;
    private bool _revealed;

    public Vector2 GridPosition
    {
        get { return _gridPosition; }
        set { _gridPosition = value; }
    }


    // Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnMouseEnter()
    {
        transform.renderer.material.color = Color.gray;
        //Debug.Log("TILE:: Position: " + gridPosition.x + ", " + gridPosition.y);
    }

    void OnMouseExit()
    {
        transform.renderer.material.color = Color.white;
    }
}
