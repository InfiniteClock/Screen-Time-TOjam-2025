using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NotificationModes = Notification.NotificationModes;

public class NotificationManager : MonoBehaviour
{
    private static NotificationManager instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject notificationPrefab;
    public Transform notificationHolder;
    public Notification onScreenPopup;

    [Space]
    public float popupShowTime = 2f;
    public float popupMoveTime = 0.5f;
    public Transform popupOffscreen;
    public Transform popupOnscreen;

    bool popupCoroutineRunning;

    Queue<PopupDescriptor> popupQueue = new();
    List<Notification> notifications = new();

    public static List<Notification> Notifications => instance.notifications;

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
        popupQueue.Enqueue(new PopupDescriptor(app, title, description, timeStamp));
    }

    // onScreenPopup.Init(app, title, description, timeStamp);

    void AddNotification(App.ID app, string title, string description, string timeStamp)
    {
        Notification notif = Instantiate(notificationPrefab, notificationHolder).GetComponent<Notification>();
        notifications.Add(notif);
        notif.Init(app, title, description, timeStamp);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Notification.Send((App.ID)Random.Range(1, 4), "Notif", "Description!!!", "now");

        // Show popups if any are waiting
        if (popupQueue.Count > 0 && !popupCoroutineRunning)
        {
            PopupDescriptor popup = popupQueue.Dequeue();
            StartCoroutine(ShowPopup(popup));
        }
    }

    IEnumerator ShowPopup(PopupDescriptor popup)
    {
        popupCoroutineRunning = true;
        onScreenPopup.Init(popup.app, popup.title, popup.description, popup.timeStamp);

        float moveTimer = 0;
        while ((moveTimer += Time.deltaTime) < popupMoveTime)
        {
            float fac = Tobo.Util.Ease.SmoothStop3(moveTimer / popupMoveTime);
            onScreenPopup.transform.position = Vector3.Lerp(popupOffscreen.position, popupOnscreen.position, fac);
            yield return null;
        }

        yield return new WaitForSeconds(popupShowTime);

        while ((moveTimer -= Time.deltaTime) > 0)
        {
            float fac = Tobo.Util.Ease.SmoothStop3(moveTimer / popupMoveTime);
            onScreenPopup.transform.position = Vector3.Lerp(popupOffscreen.position, popupOnscreen.position, fac);
            yield return null;
        }

        popupCoroutineRunning = false;
    }

    public static void RemoveNotification(Notification notification)
    {
        if (GameManager.IsGameClosing)
            return;

        instance.notifications.Remove(notification);
    }

    struct PopupDescriptor
    {
        public App.ID app;
        public string title;
        public string description;
        public string timeStamp;

        public PopupDescriptor(App.ID app, string title, string description, string timeStamp)
        {
            this.app = app;
            this.title = title;
            this.description = description;
            this.timeStamp = timeStamp;
        }
    }
}
