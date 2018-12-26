using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround32 : MonoBehaviour
{
    private PlayerControl32 player;
    private Collider2D collider;
    private Collider2D playerRayastLeftCollider;
    private Collider2D playerRayastRightCollider;

    // Use this for initialization
    void Start()
    {
        player = GetComponentInParent<PlayerControl32>();
        collider = player.GetComponent<Collider2D>();
        playerRayastLeftCollider = player.RaycastLeft.GetComponent<Collider2D>();
        playerRayastRightCollider = player.RaycastRight.GetComponent<Collider2D>();
    }

    void OnCollisionStay2D(Collision2D col)
    {
        // Player on the ground or upon a moveable
        if ((col.gameObject.tag == "Ground" || col.gameObject.tag == "Moveable")
           && col.contacts[0].point.y <= col.contacts[0].otherCollider.bounds.min.y)
        {
            player.jumping = false;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            player.jumping = true;
        }

        if (col.gameObject.tag == "Moveable" && !playerRayastRightCollider.IsTouching(col.collider)
                                             && !playerRayastLeftCollider.IsTouching(col.collider))
        {
            player.jumping = true;
        }
    }
}
