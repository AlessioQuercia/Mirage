using System;
using UnityEngine;

public class Camera32 : MonoBehaviour
{
    Transform target;
    Vector2 mov;

    private PlayerControl32 player;

    private float minCameraSize = 4.338718f;
    private float maxCameraSize = 9.845745f;

    private float maxResize = 150;
    private float resize = 0;
    private float startResizeHeightDown = -7.5f;
    private float resizeDelta = 1.8f;   //0.045f;
    private float delta_y;
    private float rescalePosition;
    private float lastLowestY = 100000000000;
    private float lastHighestY = -100000000000;

    void Awake()
    {
        player = FindObjectOfType<PlayerControl32>();
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
        if (target.position.x <= -15 || target.position.x >= 15)
        {
            transform.position = new Vector3(
                transform.position.x,
                target.position.y + 2.7f + rescalePosition,
                transform.position.z
            );
        }
        else if (target.position.x > -15 || target.position.x < 15)
        {
            transform.position = new Vector3(
                target.position.x,
                target.position.y + 2.7f + rescalePosition,
                transform.position.z
            ); 
        }

        if (target.position.y <= startResizeHeightDown && target.position.y < lastLowestY
                                                       && Camera.main.orthographicSize < maxCameraSize)//&& resize < maxResize)
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
                                                           && Camera.main.orthographicSize -0.5 > minCameraSize)//&& resize > 0)
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
