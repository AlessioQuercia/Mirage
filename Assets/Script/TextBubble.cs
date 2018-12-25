using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBubble : MonoBehaviour {

    [TextArea]
    public string puzzleActive;
    [TextArea]
    public string puzzleInactive;

    private bool isActive = false;

    public bool IsActive
    {
        get
        {
            return isActive;
        }
    }

    // Use this for initialization
    void Start () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isActive = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
