using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour {
    //Mirage
	public bool isTriggered = false;
    public bool triggeredByPlayerOnly;

    void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log("Object entered the trigger zone");
        if (triggeredByPlayerOnly && other.gameObject.tag == "Player" || !triggeredByPlayerOnly)
        {
            isTriggered = true;
        }
	}
	
	void OnTriggerStay2D (Collider2D other)
	{
		Debug.Log("Object is within the trigger zone");
        if (triggeredByPlayerOnly && other.gameObject.tag == "Player" || !triggeredByPlayerOnly)
        {
            isTriggered = true;
        }
    }
	
	void OnTriggerExit2D (Collider2D other)
	{
		Debug.Log("Object exited the trigger zone");
        if (triggeredByPlayerOnly && other.gameObject.tag == "Player" || !triggeredByPlayerOnly)
        {
            isTriggered = false;
        }
    }
}
