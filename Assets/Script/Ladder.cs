using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public PlatformEffector2D effector;
    private PlayerController player;

    // Use this for initialization
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isClimbing)
        {
            effector.rotationalOffset = 0;
        }
        else if (player.rb2d.velocity.y < 0)
        {
            effector.rotationalOffset = 180;
        }
        else if (player.rb2d.velocity.y > 0)
        {
            effector.rotationalOffset = 0;
        }
    }
}
