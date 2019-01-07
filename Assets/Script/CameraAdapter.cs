using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraAdapter : MonoBehaviour
{

    public CinemachineVirtualCamera vcam;
    public float newCameraSize;
    public float newAltitude;

    private bool increaseLens;
    private bool increaseAltitude;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(AdaptCamera());

        }
    }

    IEnumerator AdaptCamera()
    {
        if(vcam.m_Lens.OrthographicSize <= newCameraSize)
        {
            increaseLens = true;
        }
        else
        {
            increaseLens = false;
        }

        if (vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY <= newAltitude)
        {
            increaseAltitude = true;
        }
        else
        {
            increaseAltitude = false;
        }

        while ((Mathf.Max(vcam.m_Lens.OrthographicSize, newCameraSize) - Mathf.Min(vcam.m_Lens.OrthographicSize, newCameraSize)) >= 0 && (Mathf.Max(vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY, newAltitude) - Mathf.Min(vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY, newAltitude)) >= 0)
        {
            if (Mathf.Max(vcam.m_Lens.OrthographicSize, newCameraSize) - Mathf.Min(vcam.m_Lens.OrthographicSize, newCameraSize) != 0)
            {
                if (increaseLens)
                {
                    vcam.m_Lens.OrthographicSize += 0.1f;
                }
                else
                {
                    vcam.m_Lens.OrthographicSize -= 0.1f;
                }
            }

            if (Mathf.Max(vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY, newAltitude) - Mathf.Min(vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY, newAltitude) != 0)
            {
                if (increaseAltitude)
                {
                    vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY += 0.1f;
                }
                else
                {
                    vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY -= 0.1f;
                }
            }

            yield return null;
        }
    }
}
