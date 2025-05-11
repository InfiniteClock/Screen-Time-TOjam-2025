using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BranchesApp : App
{
    private static BranchesApp instance;
    private void Awake()
    {
        instance = this;
    }

    // They could be one object but dealing with dynamic content scaling in a scroll view is a pain
    public GameObject textPostPrefab;
    public GameObject imagePostPrefab;
    public Transform contentParent;
    public List<SocialMediaPosts> allPosts;

    [Space]
    public List<string> usernames;
    public List<string> words;
    public List<Sprite> profilePictures;
    public List<Sprite> images;
    public float imageChance;

    public static SocialMediaPosts AddPost(Notification.NotificationModes mode, string timestamp = "now")
    {
        SocialMediaPosts post = RandomPost();
        AddPost(post, mode, timestamp);
        return post;
    }

    /// <summary>
    /// Adds a post - doesn't send a notification
    /// </summary>
    public static void AddPost(SocialMediaPosts post, Notification.NotificationModes mode, string timestamp = "now")
    {
        bool addImage = Random.value < instance.imageChance;

        GameObject spawnedPost;
        // Spawn an image post if we have an image
        if (addImage)
            spawnedPost = Instantiate(instance.imagePostPrefab, instance.contentParent);
        else
            spawnedPost = Instantiate(instance.textPostPrefab, instance.contentParent);

        // Make ads more green
        if (post.type == SocialMediaPosts.Type.ad)
            spawnedPost.GetComponent<Image>().color = new Color(0.75f, 1f, 0.65f);
        else
            spawnedPost.transform.GetChild(2).gameObject.SetActive(false); // Disable ad label

        // PFP (Child 0)
        Sprite pfp = instance.profilePictures[Random.Range(0, instance.profilePictures.Count)];
        spawnedPost.transform.GetChild(0).GetComponent<Image>().sprite = pfp;

        // Name (Child 1)
        string name = "@" + instance.usernames[Random.Range(0, instance.usernames.Count)];
        spawnedPost.transform.GetChild(1).GetComponent<TMP_Text>().text = name;

        // Body (Child 3)
        string body = post.body;
        string w1 = GetRandomWord(), w2 = GetRandomWord(), w3 = GetRandomWord();
        body = body.Replace("<word 1>", w1).Replace("<word 2>", w2).Replace("<word 3>", w3);

        Notification.Send(new NotificationInfo(instance.id, "@" + name + " on Branches", body, timestamp), mode);
   
        string GetRandomWord() => instance.words[Random.Range(0, instance.words.Count)];

        spawnedPost.transform.GetChild(3).GetComponent<TMP_Text>().text = body;

        int buttonIndex = 4;

        if (addImage)
        {
            Sprite image = instance.images[Random.Range(0, instance.images.Count)];
            spawnedPost.transform.GetChild(4).GetComponent<Image>().sprite = image;
            buttonIndex = 5;
        }

        //BranchesPost postComp = spawnedPost.AddComponent<BranchesPost>();
        //postComp.post = post;

        // TODO: Set up button callback
        Button button = spawnedPost.transform.GetChild(buttonIndex).GetComponent<Button>();
        button.onClick.AddListener(() => instance.ClickedPost(button, post));
    }

    public static SocialMediaPosts RandomPost() => instance.allPosts[Random.Range(0, instance.allPosts.Count)];

    void ClickedPost(Button button, SocialMediaPosts post)
    {
        // Make like button red
        button.GetComponent<Image>().color = Color.red;

        if (post.type == SocialMediaPosts.Type.ad)
            ScoreManager.IncrementIncorrectOptions();
        else
            ScoreManager.IncrementCorrectOptions();

        NotificationManager.DialogueClicked(post);
    }


    protected override void OnAppOpened() { }

    protected override BackButtonAction OnBackButtonPressed()
    {
        return BackButtonAction.GoBackAMenu;
    }

    protected override void OnAppClosed() { }
}

/*
public class BranchesPost : MonoBehaviour
{
    public SocialMediaPosts post;
}
*/
