using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
    Transform target;
    Vector2 mov;

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
            target.position.x,
            target.position.y,
            transform.position.z
            );
    }
}
