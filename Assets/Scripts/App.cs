using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherit to create custom apps.
/// </summary>
public abstract class App : MonoBehaviour
{
    public ID id;
    public string homeScreenName;
    public string notificationName;
    public Sprite icon;
    public GameObject actualAppObject;

    Stack<GameObject> menuHistory = new Stack<GameObject>();

    private static Dictionary<ID, App> All = new Dictionary<ID, App>();
    /// <summary>
    /// What <see cref="App"/> is currently open. Null if on the home screen.
    /// </summary>
    public static App Current { get; private set; }

    // Add and remove from 'All' list
    private void OnEnable()
    {
        All.Add(id, this);
    }
    private void OnDisable()
    {
        All.Remove(id);
    }

    /// <summary>
    /// Called after the actual app object has been enabled.
    /// </summary>
    protected abstract void OnAppOpened();
    /// <summary>
    /// Called before the back button is pressed.
    /// </summary>
    protected abstract BackButtonAction OnBackButtonPressed();
    /// <summary>
    /// Called before the app object has been disabled.
    /// </summary>
    protected abstract void OnAppClosed();


    public void Open()
    {
        // Check if we have another app open currently
        if (Current != null && Current != this)
        {
            // Don't reset history
            Current.Close(false);
        }
    }

    public void Close(bool resetMenuHistory)
    {

    }

    

    public static App Get(ID id) => All[id];

    public enum ID
    {
        None,
        Mail,
        Messages,
        CommunityBoard,
    }

    public enum BackButtonAction
    {
        None,
        GoBackAMenu
    }
}
