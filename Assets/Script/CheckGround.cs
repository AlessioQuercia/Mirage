using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    private PlayerControl player;

    // Use this for initialization
    void Start()
    {
        player = GetComponentInParent<PlayerControl>();
    }

    void OnCollisionStay2D(Collision2D col)
    {
        // Player on the ground or upon a moveable
        if (col.gameObject.tag == "Ground")
        {
            player.grounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            player.grounded = false;
        }
    }
}
