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
        app = GetComponentInParent<App>();
        openCloser = GetComponent<OpenClose_Function>();
    }

    private void OnEnable()
    {
        if (!openCloser)
            app.MenuOpened(this);
        else if (openCloser.isOpen)
            app.MenuOpened(this);
    }

    private void OnDisable()
    {
        app.MenuClosed(this);
    }

    public void SetActive(bool active)
    {
        if (openCloser)
        {
            // Check if our state has changed
            if (active != openCloser.isOpen)
            {
                openCloser.SetState(active);
                if (active)
                    app.MenuOpened(this);
                else
                    app.MenuClosed(this);
            }
        }
        else
            gameObject.SetActive(active);
    }

    public void Open() => SetActive(true);
    public void Close() => SetActive(false);
}
