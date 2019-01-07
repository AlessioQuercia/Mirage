using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechController : MonoBehaviour {

    private LevelController level;
    private GameObject player;

    public PuzzleController questController;

    public float showLength;

    // Use this for initialization
    void Start () {
        level = GameObject.FindObjectOfType<LevelController>();
        
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.E) && !level.IsBusy)
        {
            GameObject nearestCharacter = getNearestCharacter();
            if(nearestCharacter != null)
            {
                IEnumerator coroutine = showText(nearestCharacter);
                StartCoroutine(coroutine);
            }
        }
	}

    private IEnumerator showText(GameObject character)
    {
        questController.IsBusy = true;

        GameObject textBubble = null;
        foreach (Transform child in character.transform)
        {
            if (child.tag == "TextBubble")
                textBubble = child.gameObject;
        }

        if (textBubble != null)
        {
            string message;
            TMPro.TextMeshPro text = textBubble.GetComponentInChildren<TMPro.TextMeshPro>();
            Renderer textBubbleRenderer = textBubble.GetComponent<Renderer>();

            if (questController.IsActive)
            {
                message = textBubble.GetComponentInParent<TextBubble>().puzzleActive;
            }
            else
            {
                message = textBubble.GetComponentInParent<TextBubble>().puzzleInactive;
            }


            char[] separators = { '\n' };
            string[] strValues = message.Split(separators);

            textBubbleRenderer.enabled = true;
            text.enabled = true;

            foreach (string sentence in strValues)
            {
                yield return StartCoroutine(setText(text, sentence));                
            }

            textBubbleRenderer.enabled = false;
            text.enabled = false;
            questController.IsBusy = false;
        }
    }

    private IEnumerator setText(TMPro.TextMeshPro text, string message)
    {
        text.SetText(message);
        yield return new WaitForSeconds(showLength);
    }

    private GameObject getNearestCharacter()
    {
        TextBubble[] textBubbles = GetComponentsInChildren<TextBubble>();
        float distance = 1000;
        GameObject nearestCharacter = null;
        Transform playerTransform, characterTransform;

        foreach (TextBubble textBubble in textBubbles)
        {
            if (textBubble.IsActive)
            {
                playerTransform = player.transform;
                characterTransform = textBubble.gameObject.transform;
                if(Math.Abs(characterTransform.position.x - playerTransform.position.x) < distance)
                {
                    nearestCharacter = textBubble.gameObject;
                }
            }
        }
        return nearestCharacter;
    }
}
