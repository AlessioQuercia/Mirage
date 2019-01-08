using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour {
    //Mirage
	public TriggerZone triggerZone;
    public bool fallsAfterTouchingThePlayer = false;
    public bool fallsAfterTriggerZoneActivation = false;
    public float delayAfterTouchingThePlayer = 0f;
    public float delayAfterTriggerZoneActivation = 0f;

    private Rigidbody2D rb2d;

    // Use this for initialization
    void Start () 
	{
		rb2d = GetComponent<Rigidbody2D>();            
		rb2d.bodyType = RigidbodyType2D.Static;
	}

    // Check if the player touched the platform
    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (fallsAfterTouchingThePlayer)
            {
                StartCoroutine(MakeObjectFall(delayAfterTouchingThePlayer));
            }
        }
    }

    // Update is called once per frame
    void Update () 
	{
        if (triggerZone != null && triggerZone.isTriggered)
		{
            if (fallsAfterTriggerZoneActivation)
            {
                StartCoroutine(MakeObjectFall(delayAfterTriggerZoneActivation));
            }
		}
    }
	
	IEnumerator MakeObjectFall(float delay)
    {
        yield return new WaitForSeconds(delay);
		rb2d.bodyType = RigidbodyType2D.Dynamic;
    }
}
