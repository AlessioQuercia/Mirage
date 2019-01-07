using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpControl : MonoBehaviour {
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        this.gameObject.SetActive(false);
    }
}
