using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUpNotificationSpawner : MonoBehaviour
{
    public Messaging momRecipeLink;
    public Messaging dadMoneyLink;
    public Messaging gfTrendLink;
    public Messaging anthonyLink;

    void Start()
    {
        // TODO: Come back to this later when menus are implemented
        // When you send an email/text it might send a notification
        // Which would mean I gotta rework the system a bit
        //NotificationManager.AddNotificationWhenDialogueClicked(momRecipeLink, new NotificationInfo(""))
    }
}
