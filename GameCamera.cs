using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

public class GameCamera : MonoBehaviour {
    Transform target;
    Vector2 mov;

	private PlayerControl player;

	private float minCameraSize = 7.19428f;
	private float maxCameraSize = 13.40429f;
	
	private float maxResize = 150;
	private float resize = 0;
	private float startResizeHeightDown = -7f;
	private float resizeDelta = 1.5f;	//0.045f;
	private float delta_y;
	private float rescalePosition;
	private float lastLowestY = 100000000000;
	private float lastHighestY = -100000000000;
	private float upperFloor = -1.614f;
	private float lowerFloor = -13.332f;
	
    void Awake()
    {
	    player = FindObjectOfType<PlayerControl>();
        target = player.transform;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(
            target.position.x,
            target.position.y + 5 + rescalePosition,
            transform.position.z
            );

		if (target.position.y <= startResizeHeightDown && target.position.y < lastLowestY
		                                               && Camera.main.orthographicSize <= maxCameraSize)//&& resize < maxResize)
		{
			if (lastLowestY != 100000000000)
				delta_y = Math.Abs(lastLowestY - target.position.y);
			
			Camera.main.orthographicSize += resizeDelta * delta_y;

			rescalePosition += resizeDelta * delta_y;
			
			lastLowestY = target.position.y;

			lastHighestY = -100000000000;
			
			resize++;
		}
		else if (target.position.y > startResizeHeightDown && target.position.y > lastHighestY  
		                                                   && Camera.main.orthographicSize >= minCameraSize)//&& resize > 0)
		{
			if (lastHighestY != -100000000000)
				delta_y = Math.Abs(lastHighestY - target.position.y);
			
			Camera.main.orthographicSize -= resizeDelta * delta_y;
			
			rescalePosition -= resizeDelta * delta_y;
			
			lastHighestY = target.position.y;

			lastLowestY = 100000000000;

			resize--;
		}
	}
}
