using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		Invoke("BackToTitle", 10);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void BackToTitle()
	{
		SceneManager.LoadScene("Title");
	}
}
