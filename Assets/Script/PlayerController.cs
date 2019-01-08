using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Madatory scripts for character mechanics
    public CharacterController2D controller;
    public Animator animator;
    public Rigidbody2D rb2d;

    // Raycasts for environment detection
    RaycastHit2D frontRaycastHit;
    float frontRaycastDistance = 0.4f;
    RaycastHit2D upRaycastHit;
    float upRaycastDistance = 0.25f;
    RaycastHit2D downRaycastHit;
    float downRaycastDistance = 0.25f;
    Vector3 centrePointOfPlayer;
    Vector3 downRaycastsInitialPosition;

    // Public movement variables
    public bool isInControl = true;
    public float movementSpeed = 20f;
    public float climbingSpeed = 10f;

    // Public interaction variables
    public bool onMoveableObject = false;
    public bool onPickable = false;
    public bool onDialogue = false;
    public bool onLadder = false;
    public bool isClimbing = false;
    public bool onSwitch = false;
    public bool interact = false;

    // Other public components
    public bool checkInventory = false;

    // Inner movement variables
    float horizontalMove = 0f;
    float verticalMove = 0f;
    bool run = false;
    bool jump = false;
    bool crouch = false;
    float walkingSpeed = 20f;
    float runningSpeed = 40F;

    // Inner interaction variables
    bool isDragging = false;
    Collider2D currentLadderCollider = null;
    bool isBusy = false;

    // Other inner variables

    // Other components

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (controller.m_FacingRight)
        {
            centrePointOfPlayer = new Vector3(transform.position.x + GetComponent<BoxCollider2D>().offset.x, transform.position.y - 0.5f, transform.position.z);
            downRaycastsInitialPosition = new Vector3(transform.position.x + GetComponent<BoxCollider2D>().offset.x, transform.position.y - 1.25f, transform.position.z);
        }
        else
        {
            centrePointOfPlayer = new Vector3(transform.position.x - GetComponent<BoxCollider2D>().offset.x, transform.position.y - 0.5f, transform.position.z);
            downRaycastsInitialPosition = new Vector3(transform.position.x - GetComponent<BoxCollider2D>().offset.x, transform.position.y - 1.25f, transform.position.z);
        }
    }


    // Update is called once fer frame
    void Update()
    {
        // Analyzing environment with raycasts
        Physics2D.queriesStartInColliders = false;
        // Adjusting raycast position because the player object and its colliders are not aligned
        if (controller.m_FacingRight)
        {
            centrePointOfPlayer = new Vector3(transform.position.x + GetComponent<BoxCollider2D>().offset.x, transform.position.y - 0.5f, transform.position.z);
            downRaycastsInitialPosition = new Vector3(transform.position.x + GetComponent<BoxCollider2D>().offset.x, transform.position.y - 1.25f, transform.position.z);
        }
        else
        {
            centrePointOfPlayer = new Vector3(transform.position.x - GetComponent<BoxCollider2D>().offset.x, transform.position.y - 0.5f, transform.position.z);
            downRaycastsInitialPosition = new Vector3(transform.position.x - GetComponent<BoxCollider2D>().offset.x, transform.position.y - 1.25f, transform.position.z);
        }
        frontRaycastHit = Physics2D.Raycast(centrePointOfPlayer, Vector2.right * transform.localScale.x, frontRaycastDistance);
        upRaycastHit = Physics2D.Raycast(centrePointOfPlayer, Vector2.up, upRaycastDistance);
        downRaycastHit = Physics2D.Raycast(downRaycastsInitialPosition, Vector2.down, downRaycastDistance);

        // Initializing animations
        animator.SetFloat("speed", Mathf.Abs(rb2d.velocity.x));

        // Getting the pressed keys
        if (isInControl && Input.GetButtonDown("Jump"))
        {
            jump = true;

            if (!isBusy || isInControl)
            {
                animator.SetBool("jumping", true);
            }
        }

        if (Input.GetButtonDown("Crouch"))      // Crouching is useless for now
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        if (Input.GetButtonDown("Interact"))
        {
            interact = true;
        }

        if (Input.GetButtonDown("Check Inventory"))
        {
            checkInventory = true;
        }

        if (Input.GetButtonDown("Sprint"))
        {
            run = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            run = false;
        }

        if (run && !isDragging)
        {
            movementSpeed = runningSpeed;
        }
        else
        {
            movementSpeed = walkingSpeed;
        }

        horizontalMove = Input.GetAxisRaw("Horizontal") * movementSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * climbingSpeed;

    }

    // To stop the jumping animation
    public void OnLanding()
    {
        animator.SetBool("jumping", false);
    }

    // Getting the objects that the player is touching
    public void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.tag == "MoveableObject")
        {
            onMoveableObject = true;
        }
    }

    // Getting the objects that the player is not touching anymore
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "MoveableObject")
        {
            onMoveableObject = false;
        }
    }

    // Getting the trigggers that the player is touching
    public void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ladder")
        {
            currentLadderCollider = col;
            onLadder = true;
        }
    }

    // Getting the triggers that the player is not touching anymore
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ladder")
        {
            onLadder = false;
        }
    }

    void FixedUpdate()
    {
        // Appply an action (= coroutine) by priority : interact > inventory > move

        // Is the player in control ?
        if (isInControl)
        {
            // Drag an object
            if (isDragging || !isBusy && frontRaycastHit.collider != null && frontRaycastHit.collider.gameObject.tag == "MoveableObject" && interact)
            {
                // First loop : make the player grab the object
                if (!isDragging)
                {
                    isInControl = false;
                    StartCoroutine(StartDragging(frontRaycastHit.collider.gameObject));
                    isDragging = true;
                    isBusy = true;
                }
                // This is the "let go" loop
                else if (interact == true)
                {
                    Destroy(GetComponent("HingeJoint2D"));
                    frontRaycastHit.collider.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    animator.SetBool("movingObject", false);
                    animator.SetBool("dragging", false);
                    isDragging = false;
                    isBusy = false;
                }
                // As long as the player is dragging the object :
                else
                {
                    controller.Move(horizontalMove * Time.fixedDeltaTime, false, false, false);

                    // Setting the right dragging animation depending on the direction the player is facing
                    if (controller.m_FacingRight && horizontalMove < 0 || !controller.m_FacingRight && horizontalMove > 0)
                        animator.SetBool("dragging", true);
                    else
                        animator.SetBool("dragging", false);
                }
            }

            // Pick Up an object
            else if (!isBusy && onPickable && interact)
            {
                StartCoroutine(PickUp());
            }

            // Talk to PNGs
            else if (!isBusy && onDialogue && interact)
            {
                StartCoroutine(Talk());
            }

            // Climb up/down a Ladder
            else if (isClimbing || (!isBusy && ((downRaycastHit.collider != null && downRaycastHit.collider.gameObject.tag == "LadderFloor" && verticalMove < 0) || (onLadder && downRaycastHit.collider != null && downRaycastHit.collider.gameObject.tag == "Ground" && verticalMove > 0))))
            {
                // First loop : make the player get on the ladder
                if (!isClimbing)
                {
                    isInControl = false;
                    

                    if (downRaycastHit.collider != null && downRaycastHit.collider.gameObject.tag == "LadderFloor")
                    {
                        // The player is on top of the ladder
                        StartCoroutine(StartClimbing(currentLadderCollider, downRaycastHit.collider));
                    }
                    else
                    {
                        // The player is at the bottom of the ladder
                        StartCoroutine(StartClimbing(currentLadderCollider, null));
                    }

                    isClimbing = true;
                    isBusy = true;
                }
                else if ((upRaycastHit.collider != null && upRaycastHit.collider.gameObject.tag == "LadderFloor") || (downRaycastHit.collider != null && downRaycastHit.collider.gameObject.tag == "Ground"))
                {
                    // If the player was effectively climbing and approaches a "ground" platform
                    if (downRaycastHit.collider != null && downRaycastHit.collider.gameObject.tag == "Ground")
                    {
                        // The player is leaving the ladder at the bottom
                        rb2d.gravityScale = 1.25f;
                        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
                        isClimbing = false;
                        isBusy = false;
                        animator.SetBool("climbing", false);
                        animator.SetBool("exitLadderFromBelow", true);
                    }
                    else
                    {
                        if (verticalMove > 0)
                        {
                            // The player is leaving the ladder from above
                            isInControl = false;
                            rb2d.velocity = new Vector2(0, 0);
                            StartCoroutine(StopClimbingOnTopOfLadder(upRaycastHit.collider));
                        }
                        else
                        {
                            rb2d.velocity = new Vector2(0, verticalMove);
                            //Set the vertical animation
                            animator.SetFloat("vSpeed", rb2d.velocity.y);
                        }
                    }
                }
                // As long as the player is climbing
                else
                {
                    rb2d.velocity = new Vector2(0, verticalMove);
                    //Set the vertical animation
                    animator.SetFloat("vSpeed", rb2d.velocity.y);
                }
            }

            // To be removed ?
            else if (!isBusy && checkInventory)
            {
                StartCoroutine(Inventory());
            }

            // Normal movement
            else if (!isBusy)
            {
                // Moving the character
                controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, true);
            }
        }

        // Turning unpressed input variable to false
        jump = false;
        interact = false;
        checkInventory = false;
    }

    // Coroutine to make the initialize the dragging procedure
    IEnumerator StartDragging(GameObject moveableObject)
    {
        while (onMoveableObject == false)
        {
            controller.Move(transform.localScale.x * movementSpeed * Time.fixedDeltaTime, false, false, false);
            yield return null;
        }
        movementSpeed = walkingSpeed;
        HingeJoint2D draggingJoint = (HingeJoint2D)gameObject.AddComponent<HingeJoint2D>();
        draggingJoint.connectedBody = moveableObject.GetComponent<Rigidbody2D>();
        moveableObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        animator.SetBool("movingObject", true);
        isInControl = true;
    }

    IEnumerator PickUp()
    {
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator Switch()
    {
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator Talk()
    {
        yield return null;
    }

    // Coroutine to make the initialize the climbing procedure
    IEnumerator StartClimbing(Collider2D ladder, Collider2D ladderFloorCollider)
    {
        // Move in the middle of the ladder
        float target = (ladder.gameObject.transform.position.x) + (ladder.offset.x);
        if (Math.Abs(target - centrePointOfPlayer.x) <= GetComponent<BoxCollider2D>().offset.x)
        {
            // Do nothing, the player is quite in the middle of the ladder already
        }
        else if ((transform.position.x + GetComponent<BoxCollider2D>().offset.x) <= target)
        {
            // The player is too much on the left
            while ((centrePointOfPlayer.x) < target - 0.1f)
            {
                controller.Move(movementSpeed * Time.fixedDeltaTime, false, false, true);
                yield return null;
            }
        }
        else
        {
            // The player is too much on the right
            while ((centrePointOfPlayer.x) > target + 0.1f)
            {
                controller.Move((-1) * movementSpeed * Time.fixedDeltaTime, false, false, true);
                yield return null;
            }
        }

        // Sart climbing
        rb2d.gravityScale = 0;
        rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        animator.SetBool("climbing", true);
        if (ladderFloorCollider != null)
        {
            // The player enters the ladder on top
            // Set the animation
            animator.SetBool("enterLadderFromAbove", true);

            // Enter the ladder
            ladderFloorCollider.isTrigger = true;
//            int delay = 40;
//            while (delay != 0)
//            {
//                delay -= 1;
////                rb2d.velocity = new Vector2(0, -climbingSpeed);
//                //Set the entering ladder animation
//                yield return null;
//            }
            while (animator.GetBool("enterLadderFromAbove"))
            {
                isInControl = false;
                yield return null;
            }

        }
        else
        {
            animator.SetBool("exitLadderFromBelow", false);
            animator.SetBool("enterLadderFromBelow", true);
            // The player enters the ladder from the bottom
            int delay = 10;
            while (delay != 0)
            {
                delay -= 1;
                rb2d.velocity = new Vector2(0, climbingSpeed);
                //Set the vertical animation
                animator.SetFloat("vSpeed", rb2d.velocity.y);
                yield return null;
            }
            animator.SetBool("enterLadderFromBelow", false);
        }
        isInControl = true;
    }

    // Coroutine to get out of a ladder on top of it
    IEnumerator StopClimbingOnTopOfLadder(Collider2D ladderFloorCollider)
    {
        // Set the animation
        animator.SetBool("climbing", false);
        animator.SetBool("exitLadderFromAbove", true);

//        // Moving out of the ladder
//        int delay = 1000;
//        while (delay != 0)
//        {
//            delay -= 1;
////            rb2d.velocity = new Vector2(0, climbingSpeed);
//            //Set the vertical animation
//            yield return null;
//        }

        while (animator.GetBool("exitLadderFromAbove"))
        {
            isInControl = false;
            yield return null;
        }
            
        // Reestablish gravity and colliders
        ladderFloorCollider.isTrigger = false;
        rb2d.gravityScale = 1.25f;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        isClimbing = false;
        isBusy = false;
        isInControl = true;
    }

    IEnumerator Inventory()
    {
        yield return null;
    }

    // To visualize raycasts
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(centrePointOfPlayer, centrePointOfPlayer + Vector3.right * transform.localScale.x * frontRaycastDistance);
        Gizmos.DrawLine(centrePointOfPlayer, centrePointOfPlayer + Vector3.up * upRaycastDistance);
        Gizmos.DrawLine(downRaycastsInitialPosition, downRaycastsInitialPosition + Vector3.down * downRaycastDistance);
    }

    
    public void AlertObservers(string message)
    {
        if (message.Equals("ExitLadderFromAboveAnimationEnded"))
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y + 1.35f,
                transform.position.z);
            
            // End the animation
            animator.SetBool("exitLadderFromAbove", false);
        }
        
        if (message.Equals("EnterLadderFromAboveAnimationEnded"))
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y - 1.2f,
                transform.position.z);
            
            // End the animation
            animator.SetBool("enterLadderFromAbove", false);
        }
    }
    
}