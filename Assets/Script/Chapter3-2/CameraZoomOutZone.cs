﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomOutZone : MonoBehaviour
{

	public bool zoomOut;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
			zoomOut = true;
	}
}
