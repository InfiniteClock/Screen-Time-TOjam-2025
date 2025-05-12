using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    public static int correctOptions;
    public static int IncorrectOptions;
    public static int EndGameLimit;

    public static void IncrementCorrectOptions()
    {
        correctOptions++;
        OptionClicked();
    }

    public static void IncrementIncorrectOptions()
    {
        int numb = Random.Range(1, Phone.Instance.night+1);
        switch (numb)
        {
            case 1:
                DialogueSpawner.MailSpawnedToday++;
                EmailSelector.AddEmail(Notification.NotificationModes.Popup);
                break;
            case 2:
                DialogueSpawner.MessagesSpawnedToday++;
                MessagesApp.AddMessage(Notification.NotificationModes.Popup);
                break;
            case 3:
                DialogueSpawner.PostsSpawnedToday++;
                var post = BranchesApp.AddPost(Notification.NotificationModes.Popup);
                if (post.type == SocialMediaPosts.Type.ad)
                    DialogueSpawner.AdsSpawnedToday++;
                break;
        }
        IncorrectOptions++;
        OptionClicked();
    }

    public static int DecrimentCorrectOptions()
    {
        return correctOptions--;
    }

    public static int DecrimentIncorrectOptions()
    {
        return IncorrectOptions--;
    }

    public static void resetVariables()
    {
        correctOptions = 0;
        IncorrectOptions = 0;
    }

    public static void OptionClicked()
    {
        int totalThingsClicked = IncorrectOptions + correctOptions;
        int totalThingsToClick = DialogueSpawner.MailSpawnedToday + DialogueSpawner.MessagesSpawnedToday + DialogueSpawner.NormalPostsSpawnedToday;

        if (totalThingsClicked == totalThingsToClick)
        {
            Debug.Log("Ended night");   
            Phone.Instance.EndNight();
        }
    }

    public static void CheckForEndGame()

    {
        if (IncorrectOptions >= EndGameLimit)
        {
            Debug.Log("game end");
        }
    }

}
