using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzleController : MonoBehaviour {

    private bool isActive = true;

    public bool IsActive
    {
        get
        {
            return isActive;
        }

        set
        {
            isActive = value;
        }
    }

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

    private bool isBusy;
}
