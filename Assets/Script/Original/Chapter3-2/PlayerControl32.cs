using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerControl32 : MonoBehaviour
{
    //Mirage
    public float speed = 10f;
    public float vSpeed = 0f;
    public float maxSpeed = 1f;
    public float jumpPower = 6.5f;
    public float climbSpeed = 3;
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
    public bool interact = false;

    private BoxCollider2D floorCollider;
    private Rigidbody2D hayCart;

    private Rigidbody2D rb2d;
    Animator anim;

    //	public Rigidbody2D movObjRb2d;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        floorCollider = GameObject.Find("Floor").GetComponent<BoxCollider2D>();

        hayCart = GameObject.Find("Rock").GetComponent<Rigidbody2D>();

        gravityStore = rb2d.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", Mathf.Abs(rb2d.velocity.x));
        anim.SetBool("grounded", grounded);
        anim.SetBool("running", run);

        if (climb)
        {
            rb2d.gravityScale = 0f;
            climbVelocity = climbSpeed * Input.GetAxisRaw("Vertical");

            rb2d.velocity = new Vector2(rb2d.velocity.x, climbVelocity);

            if (!onLadder || Input.GetKeyDown(KeyCode.E))
            {
                climb = false;
                hayCart.bodyType = RigidbodyType2D.Dynamic;
                floorCollider.enabled = true;
            }
        }
        else
        {
            rb2d.gravityScale = gravityStore;

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
                hayCart.bodyType = RigidbodyType2D.Static;
                floorCollider.enabled = false;
            }

            if (rb2d.transform.position.y < floorCollider.transform.position.y)
                hayCart.bodyType = RigidbodyType2D.Static;

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
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        
        if (!climb)
        {
            rb2d.AddForce(Vector2.right * speed * h);

            float limitedSpeed = Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed);
            rb2d.velocity = new Vector2(limitedSpeed, rb2d.velocity.y);
            
            if ((Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) && grounded)
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

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
            anim.SetBool("grounded", false);
        }

        anim.SetBool("interact", interact);

        if (interact = true)
            interact = false;

        anim.SetBool("climb", climb);

        anim.SetBool("moveObject", moveObject);

        anim.SetBool("grounded", grounded);

        //Set the vertical animation
        anim.SetFloat("vSpeed", rb2d.velocity.y);
    }
}
