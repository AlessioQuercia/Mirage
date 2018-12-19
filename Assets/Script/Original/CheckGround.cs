using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    private PlayerControl32 player;
    private Collider2D collider;

    // Use this for initialization
    void Start()
    {
        player = GetComponentInParent<PlayerControl32>();
        collider = player.GetComponent<Collider2D>();
    }

    void OnCollisionStay2D(Collision2D col)
    {
        // Player on the ground or upon a moveable
        if ((col.gameObject.tag == "Ground" || col.gameObject.tag == "Moveable")
                                               && col.contacts[0].point.y <= col.contacts[0].otherCollider.transform.position.y)
        {
            player.grounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground" || col.gameObject.tag == "Moveable")
        {
            player.grounded = false;
        }
    }
}
