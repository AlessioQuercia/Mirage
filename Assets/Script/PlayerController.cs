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
    float frontRaycastDistance = 0.5f;
    Vector3 raycastsInitialPosition;

    // Public movement variables
    public bool isInControl = true;
    public float movementSpeed = 20f;
    public float climbingSpeed = 10f;

    // Public interaction variables
    public bool onMoveableObject = false;
    public bool onPickable = false;
    public bool onDialogue = false;
    public bool onLadder = false;
    public bool onSwitch = false;
    public bool interact = false;       // Does it have to be a public, as it is called from Pu but by another script ?

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
    bool isBusy = false;

    // Other inner variables

    // Other components

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        raycastsInitialPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
    }


    // Update is called once fer frame
    void Update()
    {
        // Analyzing environment with raycasts
        Physics2D.queriesStartInColliders = false;
        raycastsInitialPosition = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
        frontRaycastHit = Physics2D.Raycast(raycastsInitialPosition, Vector2.right * transform.localScale.x, frontRaycastDistance);

        // Initializing animations
        animator.SetFloat("speed", Mathf.Abs(rb2d.velocity.x));

        // Getting the pressed keys
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("grounded", false);    // To be changed for a more logical term
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
        animator.SetBool("grounded", true);
    }

    // Getting what the player is touching
    public void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.tag == "MoveableObject")
        {
            onMoveableObject = true;
        }
    }

    // Getting what the player is not touching anymore
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "MoveableObject")
        {
            onMoveableObject = false;
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
                    isDragging = false;
                    isBusy = false;
                }
                // As long as the player is dragging the object :
                else
                {
                    controller.Move(horizontalMove * Time.fixedDeltaTime, false, false, false);
                }
            }

            // Pick Up an object
            else if (!isBusy && onPickable && interact)
            {
                StartCoroutine(PickUp());
            }
            else if (!isBusy && onDialogue && interact)
            {
                StartCoroutine(Talk());
            }
            else if (!isBusy && onLadder && (verticalMove != 0))
            {
                StartCoroutine(Climb());
            }
            else if (!isBusy && checkInventory)
            {
                StartCoroutine(Inventory());
            }
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

    IEnumerator Climb()
    {
        // Move in the middle of the ladder
        yield return null;
    }

    IEnumerator Inventory()
    {
        yield return null;
    }

    // To visualize raycasts
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(raycastsInitialPosition, raycastsInitialPosition + Vector3.right * transform.localScale.x * frontRaycastDistance);
    }
}