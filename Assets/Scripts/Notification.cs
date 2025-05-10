using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [NonSerialized] public App.ID app;
    [NonSerialized] public string title;
    [NonSerialized] public string description;
    [NonSerialized] public string timeStamp;

    public Image icon;
    //public TMP_Text appTitleText;
    public TMP_Text notificationTitleText;
    public TMP_Text descriptionText;
    public TMP_Text timeStampText;

    /// <summary>
    /// Sets the UI fields of this notification
    /// </summary>
    public void Init(App.ID app, string title, string description, string timeStamp)
    {
        this.app = app;
        this.title = title;
        this.description = description;
        this.timeStamp = timeStamp;

        icon.sprite = App.Get(app).icon;
        //appTitleText.text = App.Get(app).notificationName;
        notificationTitleText.text = title;
        descriptionText.text = description;
        // The popup notification might not have a time stamp
        if (timeStampText != null)
            timeStampText.text = timeStamp;
    }

    // TODO: Add support to take you to a specific AppMenu?
    /// <summary>
    /// Opens the app that this notification came from
    /// </summary>
    public void OpenApp()
    {
        App.Open(app);
    }

    private void OnDestroy()
    {
        NotificationManager.RemoveNotification(this);
    }



    // Redirect to NotificationManager
    /// <summary>
    /// Sends a notification.
    /// </summary>
    public static void Send(App.ID app, string title, string description, string timeStamp,
        NotificationModes mode = NotificationModes.NotificationAndPopup)
    {
        NotificationManager.Send(app, title, description, timeStamp, mode);
    }

    [Flags]
    public enum NotificationModes
    {
        Popup = 1,
        Notification = 2,
        NotificationAndPopup = Popup & Notification
    }
}
