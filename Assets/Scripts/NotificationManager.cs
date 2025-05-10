using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotificationModes = Notification.NotificationModes;

public class NotificationManager : MonoBehaviour
{
    private static NotificationManager instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject notificationPrefab;
    public Notification onScreenPopup;

    [Space]
    public Transform notificationHolder;

    List<Notification> notifications = new();

    /// <summary>
    /// Sends a notification.
    /// </summary>
    public static void Send(App.ID app, string title, string description, string timeStamp = null,
        NotificationModes mode = NotificationModes.NotificationAndPopup)
    {
        // Set time to be the current time if none is specified
        timeStamp ??= Phone.CurrentTime;

        if (mode.HasFlag(NotificationModes.Popup))
            instance.InitPopup(app, title, description, timeStamp);

        if (mode.HasFlag(NotificationModes.Notification))
            instance.AddNotification(app, title, description, timeStamp);
    }

    void InitPopup(App.ID app, string title, string description, string timeStamp)
    {
        onScreenPopup.Init(app, title, description, timeStamp);

        // TODO: Animation, queue of on-screen notifs?
    }

    void AddNotification(App.ID app, string title, string description, string timeStamp)
    {
        Notification notif = Instantiate(notificationPrefab, notificationHolder).GetComponent<Notification>();
        notifications.Add(notif);
        notif.Init(app, title, description, timeStamp);
    }
}
