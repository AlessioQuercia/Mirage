using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleQuestController : PuzzleController {

    public GameObject winCondition, blockingWall;
    [TextArea]
    public string winMessage;

    private GameObject player;
    private Collider2D playerCollider, winConditionCollider, blockingWallCollider;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");

        playerCollider = player.GetComponent<Collider2D>();
        winConditionCollider = winCondition.GetComponent<Collider2D>();
        Collider2D[] blockingWallColliders = blockingWall.GetComponents<Collider2D>();

        foreach(Collider2D collider in blockingWallColliders)
        {
            if (!collider.isTrigger)
            {
                blockingWallCollider = collider;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        

        if (Input.GetKey(KeyCode.E) && IsActive && !IsBusy && playerCollider.IsTouching(winConditionCollider))
        {
            StartCoroutine(toggleActive());

            //TODO Show the player he has won
        }
        if (Input.GetKey(KeyCode.E) && !IsActive && !IsBusy && playerCollider.IsTouching(blockingWallCollider))
        {
            StartCoroutine(toggleWall());
            
            //TODO Show the player he has won
        }
    }

    private IEnumerator toggleActive()
    {
        yield return new WaitForSeconds(1);
        IsActive = false;
    }

    private IEnumerator toggleWall()
    {
        yield return new WaitForSeconds(1);
        blockingWallCollider.isTrigger = true;
    }
}
