using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraZoomController : MonoBehaviour {
    public float newOrthographicSize = 4;
    public Vector2 newPosition;

    public float zoomSpeed;
    public float moveSpeed;

    public float delay = 0.5f;

    public Collider2D trigger;

    private Collider2D player;
    private GameObject cineMachine;

    private bool hasTriggered = false;

    IEnumerator coroutine;

	// Use this for initialization
	void Start () {
        cineMachine = this.gameObject;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();

        if(trigger == null) {
            StartCoroutine(zoomControl());
        }
        else if (!trigger.isTrigger)
        {
            StartCoroutine(zoomControl());
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (trigger != null & trigger.isTrigger)
        {
            if (trigger.IsTouching(player) && !hasTriggered)
            {
                hasTriggered = true;
                zoomToPosition();
            }
        }
    }

    public IEnumerator zoomControl()
    {
        if (!hasTriggered) {
            hasTriggered = true;
            yield return new WaitForSeconds(delay);
            zoomToPosition();
        }
    }

    public void zoomToPosition()
    {
        if (cineMachine.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize < newOrthographicSize){
            StartCoroutine(zoomOut());
        }
        else if (cineMachine.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize > newOrthographicSize)
        {
            StartCoroutine(zoomIn());
        }

        moveToPosition();
    }

    public IEnumerator zoomIn()
    {
        while (cineMachine.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize > newOrthographicSize)
        {
            cineMachine.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize -= zoomSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator zoomOut()
    {
        while (cineMachine.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize < newOrthographicSize)
        {
            cineMachine.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize += zoomSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public void moveToPosition()
    {
        if (cineMachine.GetComponentInChildren<CinemachineFramingTransposer>().m_ScreenX < newPosition.x)
        {
            StartCoroutine(zoomXIn());
        }
        else if (cineMachine.GetComponentInChildren<CinemachineFramingTransposer>().m_ScreenX > newPosition.x)
        {
            StartCoroutine(zoomXOut());
        }

        if (cineMachine.GetComponentInChildren<CinemachineFramingTransposer>().m_ScreenY < newPosition.y)
        {
            StartCoroutine(zoomYIn());
        }
        else if (cineMachine.GetComponentInChildren<CinemachineFramingTransposer>().m_ScreenY > newPosition.y)
        {
            StartCoroutine(zoomYOut());
        }
    }

    public IEnumerator zoomXIn()
    {
        while (cineMachine.GetComponentInChildren<CinemachineFramingTransposer>().m_ScreenX < newPosition.x)
        {
            cineMachine.GetComponentInChildren<CinemachineFramingTransposer>().m_ScreenX += moveSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator zoomXOut()
    {
        while (cineMachine.GetComponentInChildren<CinemachineFramingTransposer>().m_ScreenX > newPosition.x)
        {
            cineMachine.GetComponentInChildren<CinemachineFramingTransposer>().m_ScreenX -= moveSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator zoomYIn()
    {
        while (cineMachine.GetComponentInChildren<CinemachineFramingTransposer>().m_ScreenY < newPosition.y)
        {
            cineMachine.GetComponentInChildren<CinemachineFramingTransposer>().m_ScreenY += moveSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator zoomYOut()
    {
        while (cineMachine.GetComponentInChildren<CinemachineFramingTransposer>().m_ScreenY > newPosition.y)
        {
            cineMachine.GetComponentInChildren<CinemachineFramingTransposer>().m_ScreenY -= moveSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
