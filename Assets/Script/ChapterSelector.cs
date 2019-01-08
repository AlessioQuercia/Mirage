using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterSelector : MonoBehaviour {
    public GameObject image;
    public GameObject title;
    public Sprite[] imageChapters;
    public Sprite[] textChapters;
    int indice;
    bool loadScene;
    public GameObject continueSelector;


    void Start ()
    {
        indice = 2;
        image.GetComponent<SpriteRenderer>().sprite = imageChapters[indice];
        title.GetComponent<SpriteRenderer>().sprite = textChapters[indice];

        loadScene = false;
        continueSelector.SetActive(loadScene);
    }



    void Update ()
    {
        if(indice!= 2)
        {
            image.GetComponent<SpriteRenderer>().color = new Color(102.0f / 255.0f, 102.0f / 255.0f, 102.0f / 255.0f);
        }
        else
        {
            image.GetComponent<SpriteRenderer>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if(indice != 0)
            {
                indice--;
                image.GetComponent<SpriteRenderer>().sprite = imageChapters[indice];
                title.GetComponent<SpriteRenderer>().sprite = textChapters[indice];
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
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
            if(indice == 2 && !loadScene)
            {
                SceneManager.LoadScene("Tutorial");
            }
            else if (loadScene)
            {
                Load();
            }
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (loadScene)
            {
                loadScene = false;
            }
            else
            {
                loadScene = true;
            }

            continueSelector.SetActive(loadScene);
        }
    }



    void Load()
    {
        if (File.Exists(Application.dataPath + "/saves/autosave"))
        {
            String toLoad = File.ReadAllText(Application.dataPath + "/saves/autosave");

            int sceneToLoad = Int32.Parse(toLoad.Split('=')[1].Replace(" ", ""));

            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
