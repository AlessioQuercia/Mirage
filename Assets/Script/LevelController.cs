using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    public PuzzleController[] puzzleControllers;

    private PlayerController player;
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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        /*if (IsBusy)
        {
            player.isInControl = false;
        }
        else
        {
            player.isInControl = true;
        }*/

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
