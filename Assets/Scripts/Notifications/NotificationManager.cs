using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static App;
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

    // TODO: Reset function

    Queue<NotificationInfo> popupQueue = new();
    List<Notification> notifications = new();

    HashSet<int> dialoguesOpenedToday = new();
    Dictionary<int, DelayedNotification> dialogueTriggers = new(); // Don't reset 

    public static List<Notification> Notifications => instance.notifications;

    /// <summary>
    /// Sends a notification.
    /// </summary>
    public static void Send(NotificationInfo info, NotificationModes mode = NotificationModes.NotificationAndPopup)
    {
        // Set time to be now if none is specified
        info.timeStamp ??= "now";

        if (mode.HasFlag(NotificationModes.Popup))
            instance.InitPopup(info);

        if (mode.HasFlag(NotificationModes.Notification))
            instance.AddNotification(info);
    }

    void InitPopup(NotificationInfo info)
    {
        popupQueue.Enqueue(info);
    }

    void AddNotification(NotificationInfo info)
    {
        Notification notif = Instantiate(notificationPrefab, notificationHolder).GetComponent<Notification>();
        notifications.Add(notif);
        notif.Init(info);
    }

    private void Update()
    {
        // Show popups if any are waiting
        if (popupQueue.Count > 0 && !popupCoroutineRunning)
        {
            NotificationInfo popup = popupQueue.Dequeue();
            StartCoroutine(ShowPopup(popup));
        }
    }

    IEnumerator ShowPopup(NotificationInfo popup)
    {
        popupCoroutineRunning = true;
        onScreenPopup.Init(popup);

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

    IEnumerator SendNotificationAfterTime(DelayedNotification notif)
    {
        yield return new WaitForSeconds(notif.delay);

        Notification.Send(notif.info, notif.mode);
    }

    
    /// <summary>
    /// Will send the Notification described in <paramref name="info"/> when the <paramref name="email"/> is clicked, after a <paramref name="delay"/>
    /// </summary>
    /// <remarks>Call once in Start. This isn't reset between days.</remarks>
    public static void AddNotificationWhenDialogueClicked(Email email, NotificationInfo info, float delay,
        NotificationModes mode = NotificationModes.NotificationAndPopup)
    {
        instance.AddNotifWhenClicked(email.GetHashCode(), info, delay, mode);
    }
    /// <summary>
    /// Will send the Notification described in <paramref name="info"/> when the <paramref name="message"/> is clicked, after a <paramref name="delay"/>
    /// </summary>
    /// <remarks>Call once in Start. This isn't reset between days.</remarks>
    public static void AddNotificationWhenDialogueClicked(Messaging message, NotificationInfo info, float delay,
        NotificationModes mode = NotificationModes.NotificationAndPopup)
    {
        instance.AddNotifWhenClicked(message.GetHashCode(), info, delay, mode);
    }
    /// <summary>
    /// Will send the Notification described in <paramref name="info"/> when the <paramref name="post"/> is clicked, after a <paramref name="delay"/>
    /// </summary>
    /// <remarks>Call once in Start. This isn't reset between days.</remarks>
    public static void AddNotificationWhenDialogueClicked(SocialMediaPosts post, NotificationInfo info, float delay,
        NotificationModes mode = NotificationModes.NotificationAndPopup)
    {
        instance.AddNotifWhenClicked(post.GetHashCode(), info, delay, mode);
    }

    void AddNotifWhenClicked(int id, NotificationInfo info, float delay, NotificationModes mode)
    {
        dialogueTriggers.Add(id, new DelayedNotification { info = info, delay = delay, mode = mode });
    }

    /// <summary>
    /// Call when any <paramref name="email"/> is clicked.
    /// </summary>
    public static void DialogueClicked(Email email)
    {
        instance.DialogueClicked(email.GetHashCode());
    }
    /// <summary>
    /// Call when any <paramref name="message"/> is clicked.
    /// </summary>
    public static void DialogueClicked(Messaging message)
    {
        instance.DialogueClicked(message.GetHashCode());
    }
    /// <summary>
    /// Call when any <paramref name="post"/> is clicked.
    /// </summary>
    public static void DialogueClicked(SocialMediaPosts post)
    {
        instance.DialogueClicked(post.GetHashCode());
    }

    void DialogueClicked(int id)
    {
        // Check if this is the first time opening the dialogue
        if (!dialoguesOpenedToday.Contains(id))
        {
            dialoguesOpenedToday.Add(id);

            if (dialogueTriggers.ContainsKey(id))
                StartCoroutine(SendNotificationAfterTime(dialogueTriggers[id]));
        }
    }

    public static void RemoveNotification(Notification notification)
    {
        if (GameManager.IsGameClosing)
            return;

        instance.notifications.Remove(notification);
    }

    public static void Reset()
    {
        // Clear all queues and lists
        instance.dialoguesOpenedToday.Clear();
        instance.StopAllCoroutines();

        // Destroy notification objects
        for (int i = instance.notifications.Count - 1; i >= 0; i--)
            Destroy(instance.notifications[i].gameObject);

        instance.notifications.Clear();
        instance.popupQueue.Clear();
        // Note: Don't reset dialogueTriggers (these are set once and are the same forever)

        // Reset popup
        instance.onScreenPopup.transform.position = instance.popupOffscreen.position;
        instance.popupCoroutineRunning = false;
    }

    struct DelayedNotification
    {
        public NotificationInfo info;
        public float delay;
        public NotificationModes mode;
    }
}
