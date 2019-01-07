using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialControl : MonoBehaviour {
    public GameObject scene1, scene2;
    public GameObject jumping;
    Animator anim;
    int pos;
    bool changePos;

	// Use this for initialization
	void Start () {
        scene2.SetActive(false);
        pos = 0;

        anim = jumping.GetComponent<Animator>();
        anim.SetBool("jumping", true);
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (anim.GetBool("jumping"))
        {
            StartCoroutine(Wait(true));
        }
        else
        {
            StartCoroutine(Wait(false));
        }

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
                SceneManager.LoadScene("SampleScene");
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

    IEnumerator Wait(bool on)
    {
        yield return new WaitForSeconds(0.1f);

        if (on)
        {
            anim.SetBool("jumping", false);
        }
        else
        {
            anim.SetBool("jumping", true);
        }
    }
}
