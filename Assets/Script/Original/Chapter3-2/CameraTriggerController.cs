using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTriggerController : MonoBehaviour {

    public bool isActive;

	// Use this for initialization
	void Start () {
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            isActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = false;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
