using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera31 : MonoBehaviour {
    Transform target;
    Vector2 mov;

    public float debug = 2;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = new Vector3(
//            target.position.x,
            //target.position.y+2,
            target.position.x + debug,
            target.position.y - 25,
            transform.position.z
            );

        if(transform.position.x < -63.5f)
        {
            transform.position = new Vector3(
            -63.5f,
            transform.position.y,
            transform.position.z
            );
        }

        if (transform.position.y < 1.6f)
        {
            transform.position = new Vector3(
            transform.position.x,
            1.6f,
            transform.position.z
            );
        }
    }
}
