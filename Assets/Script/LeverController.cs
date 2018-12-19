using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting;
using UnityEngine;

public class LeverController : MonoBehaviour
{

    private PlayerControl32 player;

    public float rotationSpeed;

    public bool leverOn = false;

    private Rigidbody2D platformRB;

    private bool isTravelling;

    public GameObject[] affectedPlatforms;
    public float translateValue;

    public bool switchActive;

    private IEnumerator coroutine, switchFlash;

    private PuzzleController puzzleController;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerControl32>();
        puzzleController = GetComponentInParent<PuzzleController>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            switchActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            switchActive = false;
        }
    }

    private IEnumerator flashRed(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.color = new Color(255, 0, 0);
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = new Color(255, 255, 255);
    }

    // Update is called once per frame
    void Update()
    {
        if (switchActive && Input.GetKeyDown(KeyCode.E) && (puzzleController.getPlatformsTravelling() || !puzzleController.IsActive))
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            switchFlash = flashRed(spriteRenderer);
            StartCoroutine(switchFlash);
        }
        else if (switchActive && Input.GetKeyDown(KeyCode.E) && !puzzleController.getPlatformsTravelling() && puzzleController.IsActive)
        {
            puzzleController.setPlatformsTravelling(true);
            activateLever();
        }
        
    }

    private void activateLever()
    {
        leverOn = !leverOn;
        if (leverOn)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
        }

        PlatformController platformController;
        Vector2 translateVector;

        int i = 0;
        
        foreach(GameObject platform in affectedPlatforms) {
            platformController = platform.GetComponent<PlatformController>();
            if (!platformController.isDown())
            {
                translateVector = 
                    new Vector2(platform.transform.position.x,
                                platform.transform.position.y - translateValue);
            }
            else
            {
                translateVector =
                    new Vector2(platform.transform.position.x,
                                platform.transform.position.y + translateValue);
            }

            coroutine = MoveToPosition(platform.transform, translateVector, puzzleController.platformTravelTime);
            StartCoroutine(coroutine);

            platformController.toggleStatus();
            i++;
        }   
    }

    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        puzzleController.setPlatformsTravelling(false);
    }
}
