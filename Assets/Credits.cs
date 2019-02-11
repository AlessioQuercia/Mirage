using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{

	public Transform moon;

	public Transform cloud;

	public RectTransform credits;

	public float environmentSpeed = 0.05f;

	public float creditsSpeedIn = 1;

	public float creditsSpeedOut = 10;

	private float moonY = -0.32f;

	private float cloudY = 0.16f;

	private float creditsYIn = -70;

	private float creditsYOut = 900;
	
	private bool startCreditScene;

	private bool endCreditScene;

	private bool ending;

	private bool moonReady, cloudReady;
	
	
	// Use this for initialization
	void Start () 
	{
		Invoke("StartCreditScene", 1);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (startCreditScene)
		{
			if (moon.position.y > moonY)
				moon.position = new Vector3(moon.position.x, moon.position.y - environmentSpeed, moon.position.z);
			else
				moonReady = true;

			if (cloud.position.y < cloudY)
				cloud.position = new Vector3(cloud.position.x, cloud.position.y + environmentSpeed, cloud.position.z);
			else
				cloudReady = true;

			if (moonReady && cloudReady)
			{
				if (credits.anchoredPosition.y < creditsYIn)
					credits.anchoredPosition = new Vector2(credits.anchoredPosition.x, credits.anchoredPosition.y + creditsSpeedIn);
				else if (credits.anchoredPosition.y >= creditsYIn && !endCreditScene)
					Invoke("EndCreditScene", 8);
			}
		}
		if (endCreditScene)
		{
			if (credits.anchoredPosition.y < creditsYOut)
				credits.anchoredPosition = new Vector2(credits.anchoredPosition.x, credits.anchoredPosition.y + creditsSpeedOut);
			else if (credits.anchoredPosition.y >= creditsYOut && !ending)
			{
				ending = true;
				GameController.instance.LoadNextScene();
			}
		}
	}

	void StartCreditScene()
	{
		startCreditScene = true;
	}

	void EndCreditScene()
	{
		endCreditScene = true;
		startCreditScene = false;
	}
}
