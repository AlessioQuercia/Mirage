using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderZone : MonoBehaviour
{

	private PlayerControl player;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerControl>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			player.onLadder = true;
		}
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			player.onLadder = false;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
