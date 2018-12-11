using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {
    //Mirage
    public float speed = 10f;
    public float maxSpeed = 2f;
    public float jumpPower = 6.5f;

    public bool grounded;
    bool jump = false;
    bool run = false;
    bool walk = false;
    bool talking = false;
    bool talk1 = false;
    bool food = false;
    bool accepted = false;
    bool poorManDis = false;

    private Rigidbody2D rb2d;
    Animator anim;

    public GameObject textBox1, textBox1b, textBox2;
    public GameObject text1, text2, text3, text4, text5;
    public GameObject flecha;
    public GameObject wall, poorMan;
    int posFlecha = 0;


	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        grounded = true;
        jump = false;
        run = false;

        textBox1.SetActive(false);
        textBox1b.SetActive(false);
        textBox2.SetActive(false);
        text1.SetActive(false);
        text2.SetActive(false);
        text3.SetActive(false);
        text4.SetActive(false);
        text5.SetActive(false);
        flecha.SetActive(false);
        flecha.transform.position = new Vector3(-50.3f, 0.1f, 0);
    }
	


	// Update is called once per frame
	void Update () {
        anim.SetBool("walking", walk);
        anim.SetBool("grounded", grounded);
        anim.SetBool("running", run);
        Vector3 mov = new Vector3(0, 0, 0);

        if (!talking)
        {
            
            if(this.transform.position.x > -4)
            {
                SceneManager.LoadScene("Chapter3-2");
            }
            
            mov = new Vector3(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"),
                0
            );

            transform.position = Vector3.MoveTowards(
                transform.position,
                transform.position + mov,
                maxSpeed * Time.deltaTime
            );

            /*
            if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
            {
                jump = true;
            }
            */

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (this.transform.position.x >= -55.8f && this.transform.position.x <= -53.6f && this.transform.position.y > -2.9f)
                {
                    if (!accepted)
                    {
                        talking = true;
                        talk1 = true;
                        posFlecha = 0;

                        textBox1.SetActive(true);
                        textBox1b.SetActive(true);
                        text1.SetActive(true);
                        text2.SetActive(true);
                        flecha.SetActive(true);
                    }
                    
                }
                else if (this.transform.position.x >= -48.5f && this.transform.position.x <= -47)
                {
                    
                    talking = true;
                    talk1 = false;

                    if (food)
                    {
                        textBox2.SetActive(true);
                        text4.SetActive(true);
                    }
                    else
                    {
                        textBox2.SetActive(true);
                        text3.SetActive(true);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                run = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                run = false;
            }

            if (run)
            {
                maxSpeed = 5;
            }
            else
            {
                maxSpeed = 2;
            }

            if (mov.x > 0 || mov.y != 0)
            {
                walk = true;
                speed = maxSpeed;
            }
            else if (mov.x < 0 || mov.y != 0)
            {
                walk = true;
                speed = -maxSpeed;
            }
            else
            {
                walk = false;
                speed = 0;
            }
        }
        else
        {
            walk = false;

            if (posFlecha == 0)
            {
                flecha.transform.position = new Vector3(-50.3f, 0.1f, -2);
            }
            else
            {
                flecha.transform.position = new Vector3(-50.3f, -0.21f, -2);
            }

            if (talk1)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    if(posFlecha == 0)
                    {
                        posFlecha = 1;
                    }
                    else
                    {
                        posFlecha = 0;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    if(posFlecha == 0)
                    {
                        food = true;
                        talking = false;
                        accepted = true;
                    }
                    else
                    {
                        talking = false;
                    }

                    textBox1.SetActive(false);
                    textBox1b.SetActive(false);
                    text1.SetActive(false);
                    text2.SetActive(false);
                    flecha.SetActive(false);
                    posFlecha = 0;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (food)
                    {
                        if (poorManDis)
                        {
                            talking = false;
                            textBox2.SetActive(false);
                            text5.SetActive(false);
                            wall.SetActive(false);
                            poorMan.SetActive(false);
                        }
                        else
                        {
                            text4.SetActive(false);
                            text5.SetActive(true);
                            poorManDis = true;
                        }
                    }
                    else
                    {
                        talking = false;
                        text3.SetActive(false);
                        textBox2.SetActive(false);
                    }
                }
            }
        }

        
    }



    private void FixedUpdate()
    {
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        //rb2d.AddForce(Vector2.right * speed * h);

        float limitedSpeed = Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed);
        //rb2d.velocity = new Vector2(limitedSpeed, rb2d.velocity.y);

        if (speed > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (speed < 0)
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
