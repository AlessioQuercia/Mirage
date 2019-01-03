using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverZone : MonoBehaviour
{

	private PlayerControl32 player;
	
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerControl32>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			player.onSwitch = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			player.onSwitch = false;
		}
	}
}
