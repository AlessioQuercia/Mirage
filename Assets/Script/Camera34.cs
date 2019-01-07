using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera34 : MonoBehaviour
{
    Transform target;

    private PlayerController player;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        target = player.transform;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        print(Camera.main.orthographicSize);
        transform.position = new Vector3(target.position.x, 0, -1);
    }
}
