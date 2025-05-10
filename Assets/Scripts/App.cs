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

    readonly Stack<AppMenu> menuHistory = new();
    bool updateHistory = true; // Set to false when closing/opening to keep history

    /// <summary>
    /// Is this <see cref="App"/> currently open?
    /// </summary>
    public bool IsOpen => Current == this;


    /// <summary>
    /// What <see cref="App"/> is currently open. Null if on the home screen.
    /// </summary>
    public static App Current { get; private set; }

    private static readonly Dictionary<ID, App> all = new();
    private static App lastOpenedApp; // Used to clear history if we open a new app

    // Add and remove from 'All' list
    private void OnEnable()
    {
        all.Add(id, this);
    }
    private void OnDisable()
    {
        all.Remove(id);
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
    /// <remarks>History is still enabled (if you close a menu it will be written to history)</remarks>
    protected abstract void OnAppClosed();


    public void Open()
    {
        // Check if we have another app open currently
        if (Current != null && Current != this)
        {
            // Don't reset history
            Current.Close(false);
        }

        // Delete the history on the last app we opened before this
        if (lastOpenedApp != null && lastOpenedApp != this)
            lastOpenedApp.Close(true);

        // Set us as the active app
        Current = lastOpenedApp = this;

        // If we have no open menus, start history now so the actual app object gets added
        if (menuHistory.Count == 0)
        {
            updateHistory = true;
            // Turn on main object
            actualAppObject.SetActive(true);
        }
        else
        {
            // Turn on everything, then turn on history (so we don't have things in there twice)
            foreach (AppMenu menu in menuHistory)
                menu.gameObject.SetActive(true);

            // Start updating history now
            updateHistory = true;
        }

        OnAppOpened();
    }

    public void Close(bool resetMenuHistory)
    {
        OnAppClosed();

        // Pause history
        updateHistory = false;

        // Turn off all menus
        if (resetMenuHistory)
        {
            while (menuHistory.Count > 0)
                menuHistory.Pop().gameObject.SetActive(false);
        }
        else
        {
            // Turn off without clearing history
            foreach (AppMenu menu in menuHistory)
                menu.gameObject.SetActive(false);
        }

        // This should be a menu too, but let's just make sure
        actualAppObject.SetActive(false);

        Current = null;
    }

    public void Back()
    {
        BackButtonAction action = OnBackButtonPressed();

        // Turn off the current menu
        if (action == BackButtonAction.GoBackAMenu && menuHistory.Count > 0)
            // Don't pop - the MenuClosed callback removes it for us
            menuHistory.Peek().gameObject.SetActive(false);
    }


    /// <summary>
    /// Call this to add the <paramref name="menu"/> to the history
    /// </summary>
    /// <param name="menu"></param>
    public void MenuOpened(AppMenu menu)
    {
        if (!updateHistory)
            return;

        // Make sure this menu isn't already open, then open it
        if (menuHistory.Count == 0 || menuHistory.Peek() != menu)
            menuHistory.Push(menu);
        else
            Debug.LogWarning($"Tried to open menu '{menu.name}', but that menu is already open for {homeScreenName}!");
    }

    /// <summary>
    /// Call this to remove the <paramref name="menu"/> from the history
    /// </summary>
    /// <param name="menu"></param>
    public void MenuClosed(AppMenu menu)
    {
        if (!updateHistory)
            return;

        // Remove the menu if it is currently opened
        if (menuHistory.Count > 0 && menuHistory.Peek() == menu)
            menuHistory.Pop();
        else
            Debug.LogWarning($"Tried to close menu '{menu.name}', but that was not the active menu for {homeScreenName}!");

        // If we close the actual app, we are donezo
        if (menu.gameObject == actualAppObject)
            Close(false);
    }

    private void OnApplicationQuit()
    {
        // Stop warnings when closing game
        updateHistory = false;
    }


    /// <summary>
    /// Returns the <see cref="App"/> with the given <paramref name="id"/>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static App Get(ID id) => all[id];
    /// <summary>
    /// Opens the <see cref="App"/> with the given <paramref name="id"/>
    /// </summary>
    /// <param name="id"></param>
    public static void Open(ID id) => all[id].Open();
    /// <summary>
    /// Goes back in the current <see cref="App"/>
    /// </summary>
    public static void BackButtonPressed()
    {
        if (Current != null)
            Current.Back();
    }
    /// <summary>
    /// Goes home
    /// </summary>
    public static void HomeButtonPressed()
    {
        if (Current != null)
        {
            Current.Close(false);
        }
    }

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
