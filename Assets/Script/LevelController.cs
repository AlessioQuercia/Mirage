using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    public PuzzleController[] puzzleControllers;

    private bool isBusy;

    public bool IsBusy
    {
        get
        {
            return isBusy;
        }

        set
        {
            isBusy = value;
        }
    }

    // Use this for initialization
    void Start () {
        isBusy = false;
    }
	
	// Update is called once per frame
	void Update () {
        bool found = false;
		foreach(PuzzleController puzzle in puzzleControllers)
        {
            if (puzzle.IsBusy)
            {
                found = true;
                break;
            }
        }
        isBusy = found;
	}
}
