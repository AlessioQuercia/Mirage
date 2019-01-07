using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerControl34 : MonoBehaviour
{
    //Mirage
    public float speed = 10f;
    public float vSpeed = 0f;
    public float maxSpeed = 1f;
    public float jumpPower = 6.5f;
    public float gravityStore;

    public bool grounded;

    bool jump = false;
    bool run = false;

    private Rigidbody2D rb2d;
    Animator anim;

    //	public Rigidbody2D movObjRb2d;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        gravityStore = rb2d.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", Mathf.Abs(rb2d.velocity.x));
        anim.SetBool("grounded", grounded);
        anim.SetBool("running", run);


        rb2d.gravityScale = gravityStore;

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.W)) && grounded)
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))// && !onWall)
        {
            run = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            run = false;
        }

        if (run)
        {
            maxSpeed = 4;
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

        if ((Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) && grounded)
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);

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
            anim.SetBool("grounded", false);
        }

        anim.SetBool("grounded", grounded);

        //Set the vertical animation
        anim.SetFloat("vSpeed", rb2d.velocity.y);
    }
}
