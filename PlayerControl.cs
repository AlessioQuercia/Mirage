using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    //Mirage
    public float speed = 10f;
    public float maxSpeed = 2f;
    public float jumpPower = 6.5f;

    public bool grounded;
    bool jump = false;
    bool run = false;

    private Rigidbody2D rb2d;
    Animator anim;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        anim.SetFloat("speed", Mathf.Abs(rb2d.velocity.x));
        anim.SetBool("grounded", grounded);
        anim.SetBool("running", run);

        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            run = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            run = false;
        }

        if (run)
        {
            maxSpeed = 4;
        }
        else
        {
            maxSpeed = 2;
        }
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        rb2d.AddForce(Vector2.right * speed * h);

        float limitedSpeed = Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed);
        rb2d.velocity = new Vector2(limitedSpeed, rb2d.velocity.y);

        if (h > 0.1f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        if (h < -0.1f)
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
