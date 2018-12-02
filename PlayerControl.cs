using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerControl : MonoBehaviour {
    //Mirage
    public float speed = 10f;
    public float maxSpeed = 1f;
    public float jumpPower = 6.5f;
    public float climbSpeed;
    public float climbVelocity;
    public float gravityStore;

    public bool grounded;
    public bool onLadder;
    public bool onMoveableObject;
	public bool onSwitch;
    
    bool jump = false;
    bool run = false;
    public bool climb = false;
    public bool moveObject = false;

	private BoxCollider2D floorCollider;

    private Rigidbody2D rb2d;
    Animator anim;
	
//	public Rigidbody2D movObjRb2d;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

		floorCollider = GameObject.Find("Floor1").GetComponent<BoxCollider2D>();
		
	    gravityStore = rb2d.gravityScale;
	}
	
	// Update is called once per frame
	void Update () {
        anim.SetFloat("speed", Mathf.Abs(rb2d.velocity.x));
        anim.SetBool("grounded", grounded);
        anim.SetBool("running", run);

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.W)) && grounded && !moveObject)
        {
            jump = true;
        }

        if (!moveObject && Input.GetKeyDown(KeyCode.LeftShift))// && !onWall)
        {
            run = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            run = false;
        }

	    if (onLadder && !moveObject && !onMoveableObject && Input.GetKeyDown(KeyCode.E))
	    {
	        climb = true;
		    floorCollider.enabled = false;
	    }

		if (!onLadder)
		{
			climb = false;
			floorCollider.enabled = true;
		}
		
	    if (climb)
	    {
	        rb2d.gravityScale = 0f;
	        climbVelocity = climbSpeed * Input.GetAxisRaw("Vertical");

	        rb2d.velocity = new Vector2(rb2d.velocity.x, climbVelocity);
	    }
	    else
	    {
	        rb2d.gravityScale = gravityStore;
	    }

        if (run)
        {
            maxSpeed = 4;
        }
        else if (moveObject)
        {
	        maxSpeed = 1f;
        }
        else
        {
            maxSpeed = 2f;
        }
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

		rb2d.AddForce(Vector2.right * speed * h);
	    
	    float limitedSpeed = Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed);
	    rb2d.velocity = new Vector2(limitedSpeed, rb2d.velocity.y);

	    if (h > 0.1f && !moveObject)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        if (h < -0.1f && !moveObject)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (jump)
        {
            rb2d.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jump = false;
        }

    }
}
