using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationLauncher : MonoBehaviour
{

    // A script to launch an animation when the character walks into a trigger
    public float animationDuration = 0f;
    public string animationToLaunch;
    public bool faceRightDuringAnimation = false;

    private PlayerController player;
    private bool alreadyBeenActivated = false;
    private bool playerWasFacingRight = false;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Detect when the player entered the trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!alreadyBeenActivated)
            {
                alreadyBeenActivated = true;
                player.isInControl = false;
                StartCoroutine(launchAnimation());
            }
        }
    }

    // A coroutine to block the character and apply a choen animation
    IEnumerator launchAnimation()
    {
        // Memorize the previous state and apply the new one
        playerWasFacingRight = player.controller.m_FacingRight;
        player.controller.m_FacingRight = faceRightDuringAnimation;
        player.animator.SetBool(animationToLaunch, true);

        // Wait for the animation duration
        yield return new WaitForSeconds(animationDuration);

        // Put everything back to normal
        player.animator.SetBool(animationToLaunch, false);
        player.controller.m_FacingRight = playerWasFacingRight;
        player.isInControl = true;
    }
}
