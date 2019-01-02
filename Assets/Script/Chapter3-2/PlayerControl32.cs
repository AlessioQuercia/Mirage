using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerControl32 : MonoBehaviour
{
    //Mirage
    
    //Ray Casting
    [Header("Raycast Directions")]
    public Transform RaycastRight;
    public Transform RaycastLeft;
    public Transform RaycastUp;
    public Transform RaycastDown;

    //Parameters
    [Header("Parameters")]   
    public float speed = 10f;
    public float vSpeed = 0f;
    public float maxSpeed = 1f;
    public float jumpPower = 6.5f;
    public float climbSpeed = 3;
    public float climbVelocity;
    public float gravityStore;

    [FormerlySerializedAs("grounded")] public bool jumping;
    public bool onLadder;
    public bool onMoveableObject;
    public bool nextToMoveableObject;
    public bool onSwitch;

    bool jump;
    bool run;
    public bool climbing;
    public bool movingObject;
    public bool interacting;
    public bool dragging;

    private BoxCollider2D floorCollider;
    private Rigidbody2D hayCart;

    private Rigidbody2D rb2d;
    Animator anim;

    public bool flippedRight;
    private float flipDegree;
    private float flipDelta = 0.3f;

    public bool jumped;
    private long jumpReload;

    public bool isDead;
    private bool done;
    private bool done1;

    public bool disabledMovements;
    
    //	public Rigidbody2D movObjRb2d;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        floorCollider = GameObject.Find("Floor").GetComponent<BoxCollider2D>();

        hayCart = GameObject.Find("HayCart").GetComponent<Rigidbody2D>();

        gravityStore = rb2d.gravityScale;

        jumping = false;
        flippedRight = true;
        flipDegree = 1;

        isDead = false;
        
        GameController.instance.OpenDoors();
    }

    // Update is called once per frame
    void Update()
    {
        if (!disabledMovements)
        {
            if (transform.position.x >= 18 && !done)
            {
                GameController.instance.LoadScene(3);
                done = true;
            }

            if (isDead && !done)
            {
                gameObject.SetActive(false);
                GameController.instance.GameOver();
                done = true;
            }

            anim.SetFloat("speed", Mathf.Abs(rb2d.velocity.x));

            anim.SetBool("jumping", jumping);

            if (climbing)
            {
                rb2d.gravityScale = 0f;
                climbVelocity = climbSpeed * Input.GetAxisRaw("Vertical");

                rb2d.velocity = new Vector2(0, climbVelocity);

                if (!onLadder || Input.GetKeyDown(KeyCode.E))
                {
                    climbing = false;
                    hayCart.bodyType = RigidbodyType2D.Dynamic;
                    floorCollider.enabled = true;
                }
            }
            else
            {
                rb2d.gravityScale = gravityStore;

                if (jumped)
                    jumpReload++;

                if (jumpReload >= 60)
                {
                    jumped = false;
                    jumpReload = 0;
                }

                if (!jumped && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space) ||
                                Input.GetKeyDown(KeyCode.W)) && !jumping && !movingObject)
                {
                    jump = true;
                }

                if (!nextToMoveableObject && !movingObject && Input.GetKeyDown(KeyCode.LeftShift)) // && !onWall)
                {
                    run = true;
                }
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    run = false;
                }

                if (onLadder && !movingObject && !onMoveableObject && Input.GetKeyDown(KeyCode.E))
                {
                    climbing = true;
                    hayCart.bodyType = RigidbodyType2D.Static;
                    floorCollider.enabled = false;
                }

                if (rb2d.transform.position.y < floorCollider.transform.position.y)
                    hayCart.bodyType = RigidbodyType2D.Static;

                if (run)
                {
                    maxSpeed = 4;
                }
                else if (movingObject)
                {
                    maxSpeed = 1f;
                    hayCart.mass = 1;
                }
                else
                {
                    maxSpeed = 2f;
                    hayCart.mass = 100;
                }

            }
        }
    }

    private void FixedUpdate()
    {
        if (!disabledMovements)
        {
            float h = Input.GetAxis("Horizontal");

            if (!climbing)
            {
                rb2d.AddForce(Vector2.right * speed * h);

                float limitedSpeed = Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed);
                rb2d.velocity = new Vector2(limitedSpeed, rb2d.velocity.y);

                if ((Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) && !jumping)
                    rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }

            if (h > 0.1f && !movingObject)
            {
                if (flipDegree + flipDelta < 1)
                    flipDegree += flipDelta;
                else
                    flipDegree = 1;
                transform.localScale = new Vector3(flipDegree, 1f, 1f);
                flippedRight = true;
            }

            if (h < -0.1f && !movingObject)
            {
                if (flipDegree - flipDelta > -1)
                    flipDegree -= flipDelta;
                else
                    flipDegree = -1;
                transform.localScale = new Vector3(flipDegree, 1f, 1f);
                flippedRight = false;
            }

            if (jump)
            {
                rb2d.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                jumped = true;
                jump = false;
                jumping = true;
            }

            if (movingObject)
            {
                if (flippedRight && h < 0 || !flippedRight && h > 0)
                    dragging = true;
                else
                    dragging = false;
            }

            anim.SetBool("interacting", interacting);

            if (interacting = true)
                interacting = false;

            anim.SetBool("climbing", climbing);

            anim.SetBool("movingObject", movingObject);
            
            anim.SetBool("dragging", dragging);

            anim.SetBool("jumping", jumping);

            //Set the vertical animation
            anim.SetFloat("vSpeed", rb2d.velocity.y);
        }
    }
}
