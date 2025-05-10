using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestApp : App
{
    protected override void OnAppOpened()
    {
        
    }

    protected override BackButtonAction OnBackButtonPressed()
    {
        // Can return "None" and take a custom action if desired
        return BackButtonAction.GoBackAMenu;
    }

    protected override void OnAppClosed()
    {

    }
}
