using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour {

	public CinemachineVirtualCamera vcam;

	public CameraZoomInZone camZoomInZone;

	public CameraZoomOutZone camZoomOutZone;
    
	[Header("Camera Zoom")]
	public float MaxZoomIn = 4;
	
	public float MaxZoomOut = 10;

	public float zoomDelta = 0.1f;

	[Header("Camera Position")]
	public float MinScreenY = 0.78f;
	
	public float MaxScreenY = 0.92f;
	
	public float repositionDelta = 0.01f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (camZoomInZone != null && camZoomOutZone != null)
		{
			if (!camZoomOutZone.zoomOut && camZoomInZone.zoomIn && vcam.m_Lens.OrthographicSize > MaxZoomIn)
			{
				vcam.m_Lens.OrthographicSize -= zoomDelta;

				if (vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY > MinScreenY)
					vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY -= repositionDelta;
				else
					vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = MinScreenY;
			}
			else if (vcam.m_Lens.OrthographicSize <= MaxZoomIn || camZoomOutZone.zoomOut)
			{
				camZoomInZone.zoomIn = false;
			}
				
			
			if (!camZoomInZone.zoomIn && camZoomOutZone.zoomOut && vcam.m_Lens.OrthographicSize < MaxZoomOut)
			{
				vcam.m_Lens.OrthographicSize += zoomDelta;

				if (vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY < MaxScreenY)
					vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY += repositionDelta;
				else
					vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = MaxScreenY;
			}
			else if (vcam.m_Lens.OrthographicSize >= MaxZoomOut || camZoomInZone.zoomIn)
			{
				camZoomOutZone.zoomOut = false;
			}
		}
	}
}
