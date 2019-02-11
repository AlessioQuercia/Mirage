using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{

	public Transform moon;

	public Transform cloud;

	public RectTransform credits;

	public float environmentSpeed = 0.05f;

	public float creditsSpeedIn = 0.01f;

	public float creditsSpeedOut = 10;

	private float moonY = -0.32f;

	private float cloudY = 0.16f;

	private float creditsY = 2000;

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

//			if (moonReady && cloudReady)
//			{
//				if (credits.anchoredPosition.y < creditsYIn)
//					credits.anchoredPosition = new Vector2(credits.anchoredPosition.x, credits.anchoredPosition.y + creditsSpeedIn);
//				else if (credits.anchoredPosition.y >= creditsYIn && !endCreditScene)
//					Invoke("EndCreditScene", 8);
//			}
			if (moonReady && cloudReady)
			{
				if (credits.position.y < creditsY)
					credits.position = new Vector3(credits.position.x, credits.position.y + creditsSpeedIn,
						credits.position.z);
				else if (credits.position.y >= creditsY && !ending)
				{
					ending = true;
					GameController.instance.LoadNextScene();
				}
			}
		}
	}

	void StartCreditScene()
	{
		startCreditScene = true;
	}
}
