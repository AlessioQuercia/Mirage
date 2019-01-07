using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelZone : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			// If you want to test it, reload the same scene with the following line
//			GameController.instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
			
			GameController.instance.LoadNextScene();
		}
	}
}
