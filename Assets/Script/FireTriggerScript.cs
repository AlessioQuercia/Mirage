using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTriggerScript : MonoBehaviour {
    private GameObject player;
    private GameObject fadeToBlack;

    private IEnumerator coroutine;

    public float fadeToBlackSpeed = 0.1f;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        fadeToBlack = GameObject.Find("FadeToBlack");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            coroutine = FadeToBlack();
            StartCoroutine(coroutine);
//            SoundManager.instance.musicSource.clip = Resources.Load<AudioClip>("Sounds/DangerousMusic");
        }
    }

    public IEnumerator FadeToBlack()
    {
        player.GetComponent<PlayerController>().isInControl = false;
        Color color = fadeToBlack.GetComponent<SpriteRenderer>().color;

        while (color.a < 1)
        {
            fadeToBlack.GetComponent<SpriteRenderer>().color = new Color(color.r, color.b, color.b, color.a += fadeToBlackSpeed * Time.deltaTime);
            yield return null;
        }
        player.transform.position = new Vector2(145, player.transform.position.y);
        while (color.a > 0)
        {
            fadeToBlack.GetComponent<SpriteRenderer>().color = new Color(color.r, color.b, color.b, color.a -= fadeToBlackSpeed * Time.deltaTime);
            yield return null;
        }
        player.GetComponent<PlayerController>().isInControl = true;
    }
}
