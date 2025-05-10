using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppMenu : MonoBehaviour
{
    App app;

    private void OnEnable()
    {
        if (app == null)
            app = GetComponentInParent<App>();
    }
}
