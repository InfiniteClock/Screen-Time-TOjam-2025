using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NotificationAmountText : MonoBehaviour
{
    [Tooltip("Select 'none' to show total notifications")]
    public App.ID app;

    [Space]
    public TMPro.TMP_Text text;
    public GameObject container;

    public void Update()
    {
        int notificationCount;
        // Total notifs if app is none, otherwise just for this app
        if (app == App.ID.None)
            notificationCount = NotificationManager.Notifications.Count;
        else
            notificationCount = NotificationManager.Notifications.Where((notif) => notif.app == app).Count();

        // Show "9+" if we have more than 9 notifications
        text.text = notificationCount > 9 ? "9+" : notificationCount.ToString();
        container.SetActive(notificationCount > 0);
    }
}
