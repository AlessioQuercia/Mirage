using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterSelector : MonoBehaviour {
    public GameObject image;
    public GameObject title;
    public Sprite[] imageChapters;
    public Sprite[] textChapters;
    int indice;
    // Use this for initialization
    void Start ()
    {
        indice = 0;
        image.GetComponent<SpriteRenderer>().sprite = imageChapters[indice];
        title.GetComponent<SpriteRenderer>().sprite = textChapters[indice];
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(indice != 0)
            {
                indice--;
                image.GetComponent<SpriteRenderer>().sprite = imageChapters[indice];
                title.GetComponent<SpriteRenderer>().sprite = textChapters[indice];
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (indice != 2)
            {
                indice++;
                image.GetComponent<SpriteRenderer>().sprite = imageChapters[indice];
                title.GetComponent<SpriteRenderer>().sprite = textChapters[indice];
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if(indice == 2)
            {
                SceneManager.LoadScene("Chapter3");
            }
        }
    }
}
