using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

    private bool downStatus;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void toggleStatus()
    {
        downStatus = !downStatus;
    }

    public bool isDown()
    {
        return downStatus;
    }
}
