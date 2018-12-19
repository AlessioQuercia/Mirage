using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour {

    private bool platformsTravelling = false;
    public float platformTravelTime = 3f;

    private PlatformController[] platformControllers;

    private bool isActive = true;

    public bool IsActive
    {
        get
        {
            return isActive;
        }
    }

    public bool getPlatformsTravelling()
    {
        return platformsTravelling;
    }

    public void setPlatformsTravelling(bool value)
    {
        platformsTravelling = value;
    }

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
            isActive = false;
        }
	}
}
