using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : PuzzleController {

    public GameObject levers;

    private GameObject player;
    private Collider2D playerCollider;
    private Collider2D trigger;

    public float verticalOffset;
    public float waitingTime;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = player.GetComponent<Collider2D>();

        Collider2D[] colliders = GameObject.Find("PoorMan").GetComponents<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            if (collider.isTrigger)
            {
                trigger = collider;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (trigger.IsTouching(playerCollider) && Input.GetKeyDown(KeyCode.E) && !IsBusy && IsActive)
        {
            StartCoroutine(MoveToPosition(levers.transform,
                new Vector2(levers.transform.position.x, levers.transform.position.y - verticalOffset), 4));
            IsActive = false;
        }
    }

    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        yield return new WaitForSeconds(waitingTime);
        IsBusy = true;
        var currentPos = transform.position;
        var t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        IsBusy = false;
    }
}
