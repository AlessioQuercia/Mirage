using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour {

    public TriggerZone triggerZone;

    private Scene scene;

    // Use this for initialization
    void Start () {
        scene = SceneManager.GetActiveScene();
	}
	
	// Update is called once per frame
	void Update () {
        if (triggerZone.isTriggered)
        {
            Application.LoadLevel(scene.name);
        }
    }
}
