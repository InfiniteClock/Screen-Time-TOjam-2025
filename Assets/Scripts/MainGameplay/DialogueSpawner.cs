using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSpawner : MonoBehaviour
{
    public static DialogueSpawner Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Vector2Int messagesPerAppPerNight = new Vector2Int(7, 11);

    public static int MailSpawnedToday { get; set; }
    public static int MessagesSpawnedToday { get; set; }
    public static int PostsSpawnedToday { get; set; }
    public static int AdsSpawnedToday { get; set; }
    public static int NormalPostsSpawnedToday => PostsSpawnedToday - AdsSpawnedToday;

    // THIS IS REALLY STUPID I DONT CARE GAHHHH
    public static int normalPostsLiked;


    public void Day1()
    {
        Reset();

        MailSpawnedToday = SpawnMail();
    }

    public void Day2()
    {
        int mailLeft = MailSpawnedToday - ScoreManager.correctOptions - ScoreManager.IncorrectOptions;

        Reset();

        MailSpawnedToday = SpawnMail() + SpawnMail(mailLeft);
        MessagesSpawnedToday = SpawnMessages();
    }

    public void Day3()
    {
        int stuffLeft = MailSpawnedToday + MessagesSpawnedToday - ScoreManager.correctOptions - ScoreManager.IncorrectOptions;

        Reset();

        MailSpawnedToday = SpawnMail() + SpawnMail(Mathf.FloorToInt(stuffLeft / 2f));
        MessagesSpawnedToday = SpawnMessages() + SpawnMessages(Mathf.CeilToInt(stuffLeft / 2f));
        PostsSpawnedToday = SpawnPosts();
    }

    public int SpawnMail(int? count = null)
    {
        int mail = count ?? Random.Range(messagesPerAppPerNight.x, messagesPerAppPerNight.y + 1);
        for (int i = 0; i < mail; i++)
            EmailSelector.AddEmail(Notification.NotificationModes.Notification, Random.Range(1, 13) + "hr");
        return mail;
    }

    public int SpawnMessages(int? count = null)
    {
        int messages = count ?? Random.Range(messagesPerAppPerNight.x, messagesPerAppPerNight.y + 1);
        for (int i = 0; i < messages; i++)
            MessagesApp.AddMessage(Notification.NotificationModes.Notification, Random.Range(1, 13) + "hr");
        return messages;
    }

    public int SpawnPosts(int? count = null)
    {
        int posts = count ?? Random.Range(messagesPerAppPerNight.x, messagesPerAppPerNight.y + 1);
        for (int i = 0; i < posts; i++)
        {
            var post = BranchesApp.AddPost(Notification.NotificationModes.Notification, Random.Range(1, 13) + "hr");
            if (post.type == SocialMediaPosts.Type.ad)
                AdsSpawnedToday++;
        }
        return posts;
    }

    public void Reset()
    {
        MailSpawnedToday = 0;
        MessagesSpawnedToday = 0;
        AdsSpawnedToday = 0;
        PostsSpawnedToday = 0;
        normalPostsLiked = 0;

        ScoreManager.resetVariables();
        NotificationManager.Reset();
    }
}
