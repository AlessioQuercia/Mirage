using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePlatformController : PuzzleController {

    public float platformTravelTime = 3f;

    private PlatformController[] platformControllers;

    // Use this for initialization
    void Start () {
        platformControllers = GetComponentsInChildren<PlatformController>();
	}
	
	// Update is called once per frame
	void Update () {
        int numberOfPlatforms = platformControllers.Length;
        int platformCounter = 0;
		foreach (PlatformController platformController in platformControllers)
        {
            if (platformController.isDown())
            {
                platformCounter++;
            }
        }
        if(platformCounter == numberOfPlatforms)
        {
            IsActive = false;
        }
	}
}
