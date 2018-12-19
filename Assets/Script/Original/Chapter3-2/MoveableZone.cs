using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableZone : MonoBehaviour {
    private PlayerControl32 player;

    private Rigidbody2D rb2d;
	
    private float maxSpeed = 1f;

    private bool canRelease;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerControl32>();

        rb2d = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            player.onMoveableObject = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            player.onMoveableObject = false;
        }
    }
	
    // Update is called once per frame
    void Update () 
    {
        if (player.onMoveableObject && Input.GetKeyDown(KeyCode.E))
        {
            player.moveObject = true;
        }
		
        if (player.moveObject && Input.GetKeyUp(KeyCode.E))
        {
            canRelease = true;
        }
		
        if (player.moveObject && canRelease && Input.GetKeyDown(KeyCode.E))
        {
            player.moveObject = false;
            canRelease = false;
        }
    }

    private void FixedUpdate()
    {
        if (player.moveObject)
        {
            float h = Input.GetAxis("Horizontal");
//            player.onMoveableObject = true;
            rb2d.AddForce(Vector2.right * 720 * h);
            float limSpeed = 0;
            limSpeed = Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed);
            rb2d.velocity = new Vector2(limSpeed, rb2d.velocity.y);
            
            if ((Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) && player.grounded)
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
    }
}
