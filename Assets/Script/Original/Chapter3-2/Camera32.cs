using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

public class Camera32 : MonoBehaviour
{
    private Camera cameraObject;

    public float startPositionX = 2.0f;
    public float startPositionY = 1.74f;

    public float maxOrthographicSize = 10.5f;
    public float minOrthographicSize = 8.81f;

    public float zoomSpeed;
    public float panSpeed;

    private bool zoomOut = false;

    private Transform playerTransform;
    private PuzzleController puzzle;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        cameraObject = GetComponent<Camera>();
    }

    // Use this for initialization
    void Start()
    {
        cameraObject.transform.position = new Vector3(startPositionX, startPositionY, 0);
        Camera.main.orthographicSize = minOrthographicSize;

        puzzle = GameObject.Find("Puzzle").GetComponent<PuzzleController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Moveable"))
        {
            zoomOut = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 translateY;
        if (puzzle.IsActive && zoomOut == true && Camera.main.orthographicSize < maxOrthographicSize)
        {
            translateY = new Vector3(0, -panSpeed * Time.deltaTime, 0);
            Camera.main.orthographicSize += zoomSpeed * Time.deltaTime;
            cameraObject.transform.Translate(translateY);
            print("Ortho " + Camera.main.orthographicSize);
            print("Y " + cameraObject.transform.position.y);
        }
        else if (!puzzle.IsActive && playerTransform.position.y > -4 && Camera.main.orthographicSize > minOrthographicSize)
        {
            translateY = new Vector3(0, panSpeed * Time.deltaTime, 0);
            Camera.main.orthographicSize -= zoomSpeed * Time.deltaTime;
            cameraObject.transform.Translate(translateY);
            print("Ortho " + Camera.main.orthographicSize);
            print("Y " + cameraObject.transform.position.y);
        }
    }
}
