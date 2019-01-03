using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialControl : MonoBehaviour {
    public GameObject scene1, scene2;
    int pos;
    bool changePos;

	// Use this for initialization
	void Start () {
        scene2.SetActive(false);
        pos = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (changePos)
        {
            if(pos == 0)
            {
                scene1.SetActive(true);
                scene2.SetActive(false);
            }
            else if(pos == 1)
            {
                scene1.SetActive(false);
                scene2.SetActive(true);
            }
            else if (pos == 2)
            {
                SceneManager.LoadScene("Chapter3");
            }
        }

		if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(pos != 0)
            {
                pos--;
                changePos = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            pos++;
            changePos = true;
        }
    }
}
