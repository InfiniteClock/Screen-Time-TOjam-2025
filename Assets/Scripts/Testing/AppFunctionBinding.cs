using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppFunctionBinding : MonoBehaviour
{
    // Has local functions so UI buttons can call them

    public void Back()
    {
        App.BackButtonPressed();
    }

    public void Home()
    {
        App.HomeButtonPressed();
    }
}
