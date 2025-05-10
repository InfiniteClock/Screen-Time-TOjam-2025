using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppMenu : MonoBehaviour
{
    OpenClose_Function openCloser;
    App app;
    

    private void Awake()
    {
        openCloser = GetComponent<OpenClose_Function>();
    }

    private void OnEnable()
    {
        if (app == null)
            app = GetComponentInParent<App>();

        app.MenuOpened(this);
    }

    private void OnDisable()
    {
        app.MenuClosed(this);
    }

    public void SetActive(bool active)
    {
        if (openCloser)
            openCloser.SetState(active);
        else
            gameObject.SetActive(active);
    }
}
