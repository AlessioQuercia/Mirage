﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{

    private PlayerControl32 player;
    
    public bool lever1_on;
    public bool lever2_on;
    public bool lever3_on;
    public bool lever4_on;
    public bool lever5_on;

    public float platformSpeed = 3f;

    public float hideDown = -7.3f;
    public float hideUp = 2.5f;

    private float p1_x; // = -10.83f;
    private float p2_x; // = -6.53f;
    private float p3_x; // = -5.04f;
    private float p4_x; // = 0.85f;
    private float p5_x; // = 5.96f;

    public float p1_stop = -0.55f;
    public float p2_stop = -0.55f;
    public float p3_stop = -0.36f;
    public float p4_stop = 0.3f;
    public float p5_stop = 0.87f;

    [Header("Platforms")]
    public Rigidbody2D p1rb;
    public Rigidbody2D p2rb;
    public Rigidbody2D p3rb;
    public Rigidbody2D p4rb;
    public Rigidbody2D p5rb;

    [Header("Levers")]
    public Collider2D lever1;
    public Collider2D lever2;
    public Collider2D lever3;
    public Collider2D lever4;
    public Collider2D lever5;

    private bool p1_toStop;
    private bool p2_toStop;
    private bool p3_toStop;
    private bool p4_toStop;
    private bool p5_toStop;


    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerControl32>();

        p1_x = p1rb.transform.position.x;
        p2_x = p2rb.transform.position.x;
        p3_x = p3rb.transform.position.x;
        p4_x = p4rb.transform.position.x;
        p5_x = p5rb.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.onSwitch && Input.GetKeyDown(KeyCode.E))
        {
            String leverName = getLeverName();
            turn(leverName);
            activateLever(leverName);
            player.interact = true;
        }

        if (p1_toStop && (p1rb.velocity.y > 0 && p1rb.position.y >= p1_stop ||
                          p1rb.velocity.y < 0 && p1rb.position.y <= hideDown))
        {
            p1rb.velocity = new Vector2(0, 0);
            p1_toStop = false;
        }

        if (p2_toStop && (p2rb.velocity.y > 0 && p2rb.position.y >= p2_stop ||
                          p2rb.velocity.y < 0 && p2rb.position.y <= hideDown))
        {
            p2rb.velocity = new Vector2(0, 0);
            p2_toStop = false;
        }

        if (p3_toStop && (p3rb.velocity.y > 0 && p3rb.position.y >= hideUp ||
                          p3rb.velocity.y < 0 && p3rb.position.y <= p3_stop))
        {
            p3rb.velocity = new Vector2(0, 0);
            p3_toStop = false;
        }

        if (p4_toStop && (p4rb.velocity.y > 0 && p4rb.position.y >= hideUp ||
                          p4rb.velocity.y < 0 && p4rb.position.y <= p4_stop))
        {
            p4rb.velocity = new Vector2(0, 0);
            p4_toStop = false;
        }

        if (p5_toStop && (p5rb.velocity.y > 0 && p5rb.position.y >= hideUp ||
                          p5rb.velocity.y < 0 && p5rb.position.y <= p5_stop))
        {
            p5rb.velocity = new Vector2(0, 0);
            p5_toStop = false;
        }

        //		if (lever1_on && p1rb.position.x < p1_stop && p2rb.position.x < p2_stop)
        //			lever1_on = false;
        //		
        //		else if (!lever1_on && p1rb.position.x > hideDown && p2rb.position.x > hideDown)
        //			lever1_on = true;
        //		
        //		if (lever2_on && p1rb.position.x < p1_stop && p3rb.position.x > p3_stop)
        //			lever2_on = false;
        //		
        //		else if (!lever2_on && p1rb.position.x > hideDown && p2rb.position.x < p2_stop && p3rb.position.x < hideUp)
        //			lever2_on = true;
        //
        //		if (lever3_on && p4rb.position.x > p4_stop)
        //			lever3_on = false;
        //		
        //		else if (!lever3_on && p3rb.position.x > p3_stop && p4rb.position.x < hideUp)
        //			lever3_on = true;
        //		
        //		if (lever4_on && p4rb.position.x > p4_stop && p5rb.position.x > p5_stop)
        //			lever4_on = false;
        //		
        //		else if (!lever4_on && p3rb.position.x > p3_stop && p4rb.position.x < hideUp && p5rb.position.x < hideUp)
        //			lever4_on = false;
        //		
        //		if (lever5_on && p3rb.position.x > p3_stop && p5rb.position.x > p5_stop)
        //			lever5_on = false;
        //		
        //		else if (!lever5_on && p3rb.position.x < hideUp && p5rb.position.x < hideUp)
        //			lever5_on = true;

    }

    private void turn(String leverName)
    {
        GameObject selectedLever = null;
        switch (leverName)
        {
            case "Lever1":
                lever1_on = !lever1_on;
                selectedLever = lever1.gameObject;
                break;

            case "Lever2":
                lever2_on = !lever2_on;
                selectedLever = lever2.gameObject;
                break;

            case "Lever3":
                lever3_on = !lever3_on;
                selectedLever = lever3.gameObject;
                break;

            case "Lever4":
                lever4_on = !lever4_on;
                selectedLever = lever4.gameObject;
                break;

            case "Lever5":
                lever5_on = !lever5_on;
                selectedLever = lever5.gameObject;
                break;
        }
        if (selectedLever.transform.localScale.x == 1)
            selectedLever.transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (selectedLever.transform.localScale.x == -1)
            selectedLever.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private String getLeverName()
    {
        String leverName = "";

        if (player.GetComponent<Collider2D>().IsTouching(lever1))
            leverName = lever1.name;
        else if (player.GetComponent<Collider2D>().IsTouching(lever2))
            leverName = lever2.name;
        else if (player.GetComponent<Collider2D>().IsTouching(lever3))
            leverName = lever3.name;
        else if (player.GetComponent<Collider2D>().IsTouching(lever4))
            leverName = lever4.name;
        else if (player.GetComponent<Collider2D>().IsTouching(lever5))
            leverName = lever5.name;

        return leverName;
    }

    private void activateLever(String leverName)
    {
        switch (leverName)
        {
            case "Lever1":
                {
                    if (lever1_on)
                    {
                        // Activate platform 1
                        if (p1rb.transform.position.y < p1_stop)
                        {
                            p1rb.velocity = new Vector2(p1rb.velocity.x, platformSpeed);
                        }
                        // Activate platform 2
                        if (p2rb.transform.position.y < p2_stop)
                        {
                            p2rb.velocity = new Vector2(p2rb.velocity.x, platformSpeed);
                        }
                    }
                    else
                    {
                        // Deactivate platform 1
                        if (p1rb.transform.position.y > hideDown)
                        {
                            p1rb.velocity = new Vector2(p1rb.velocity.x, -platformSpeed);
                        }

                        // Deactivate platform 2
                        if (p2rb.transform.position.y > hideDown)
                        {
                            p2rb.velocity = new Vector2(p2rb.velocity.x, -platformSpeed);
                        }
                    }

                    p1_toStop = true;
                    p2_toStop = true;

                }
                break;

            case "Lever2":
                {
                    if (lever2_on)
                    {
                        // Activate platform 1
                        if (p1rb.transform.position.y < p1_stop)
                        {
                            p1rb.velocity = new Vector2(p1rb.velocity.x, platformSpeed);
                        }

                        // Deactivate platform 2
                        if (p2rb.transform.position.y > hideDown)
                        {
                            p2rb.velocity = new Vector2(p2rb.velocity.x, -platformSpeed);
                        }

                        // Activate platform 3
                        if (p3rb.transform.position.y > p3_stop)
                        {
                            p3rb.velocity = new Vector2(p3rb.velocity.x, -platformSpeed);
                        }
                    }
                    else
                    {
                        // Deactivate platform 1
                        if (p1rb.transform.position.y > hideDown)
                        {
                            p1rb.velocity = new Vector2(p1rb.velocity.x, -platformSpeed);
                        }


                        // Deactivate platform 2
                        if (p3rb.transform.position.y < hideUp)
                        {
                            p3rb.velocity = new Vector2(p2rb.velocity.x, platformSpeed);
                        }
                    }

                    p1_toStop = true;
                    p2_toStop = true;
                    p3_toStop = true;

                }
                break;

            case "Lever3":
                {
                    if (lever3_on)
                    {
                        // Deactivate platform 3
                        if (p3rb.transform.position.y < hideUp)
                        {
                            p3rb.velocity = new Vector2(p3rb.velocity.x, platformSpeed);
                        }

                        //					if (p2rb.transform.position.y < p2_stop)
                        //					{
                        //						p2rb.velocity = new Vector2(p2rb.velocity.x, platformSpeed);
                        //					}

                        // Activate platform 4
                        if (p4rb.transform.position.y > p4_stop)
                        {
                            p4rb.velocity = new Vector2(p4rb.velocity.x, -platformSpeed);
                        }
                    }
                    else
                    {
                        //					if (p2rb.transform.position.y > hideDown)
                        //					{
                        //						p2rb.velocity = new Vector2(p2rb.velocity.x, -platformSpeed);
                        //					}

                        // Deactivate platform 4
                        if (p4rb.transform.position.y < hideUp)
                        {
                            p4rb.velocity = new Vector2(p4rb.velocity.x, platformSpeed);
                        }
                    }

                    p3_toStop = true;
                    p4_toStop = true;

                }
                break;

            case "Lever4":
                {
                    if (lever4_on)
                    {
                        if (p3rb.transform.position.y < hideUp)
                        {
                            p3rb.velocity = new Vector2(p3rb.velocity.x, platformSpeed);
                        }
                        if (p4rb.transform.position.y > p4_stop)
                        {
                            p4rb.velocity = new Vector2(p4rb.velocity.x, -platformSpeed);
                        }
                        if (p5rb.transform.position.y > p5_stop)
                        {
                            p5rb.velocity = new Vector2(p5rb.velocity.x, -platformSpeed);
                        }
                    }
                    else
                    {
                        if (p4rb.transform.position.y < hideUp)
                        {
                            p4rb.velocity = new Vector2(p4rb.velocity.x, platformSpeed);
                        }

                        if (p5rb.transform.position.y < hideUp)
                        {
                            p5rb.velocity = new Vector2(p5rb.velocity.x, platformSpeed);
                        }
                    }

                    p3_toStop = true;
                    p4_toStop = true;
                    p5_toStop = true;

                }
                break;

            case "Lever5":
                {
                    if (lever5_on)
                    {
                        if (p3rb.transform.position.y > p3_stop)
                        {
                            p3rb.velocity = new Vector2(p3rb.velocity.x, -platformSpeed);
                        }
                        if (p5rb.transform.position.y > p5_stop)
                        {
                            p5rb.velocity = new Vector2(p5rb.velocity.x, -platformSpeed);
                        }
                    }
                    else
                    {
                        if (p3rb.transform.position.y < hideUp)
                        {
                            p3rb.velocity = new Vector2(p3rb.velocity.x, platformSpeed);
                        }

                        if (p5rb.transform.position.y < hideUp)
                        {
                            p5rb.velocity = new Vector2(p5rb.velocity.x, platformSpeed);
                        }
                    }

                    p3_toStop = true;
                    p5_toStop = true;

                }
                break;
        }
    }
}
