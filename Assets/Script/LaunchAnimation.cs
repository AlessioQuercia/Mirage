using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchAnimation : MonoBehaviour {

    // A script to launch an animation when the character walks into a trigger
    public float delay = 0f;
    public string animationToLaunch;
    public bool activableMultipleTimes = false;
    public bool faceRightDuringAnimation = true;

    private PlayerController player;
    private bool alreadyBeenActivated = false;
    private bool playerIsFacingRight = false;

    // Use this for initialization
    void Start () {

        player = FindObjectOfType<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (activableMultipleTimes)
            {
                player.isInControl = false;
                playerIsFacingRight = player.controller.m_FacingRight;
                player.controller.m_FacingRight = faceRightDuringAnimation;
                player.animator.SetBool(animationToLaunch, true);
            }
            else
            {
                if (!alreadyBeenActivated)
                {
                    alreadyBeenActivated = true;
                    player.isInControl = false;
                    playerIsFacingRight = player.controller.m_FacingRight;
                    player.controller.m_FacingRight = faceRightDuringAnimation;
                    player.animator.SetBool(animationToLaunch, true);
                }
            }
        }
        player.animator.SetBool(animationToLaunch, false);
        player.controller.m_FacingRight = playerIsFacingRight;
        player.isInControl = true;
    }
}
