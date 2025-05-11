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

    [Space]
    public List<string> usernames;
    public List<string> words;
    public List<Sprite> profilePictures;
    public List<Sprite> images;
    public float imageChance;


    /// <summary>
    /// Adds a post - doesn't send a notification
    /// </summary>
    public static void AddPost(SocialMediaPosts post)
    {
        bool addImage = Random.value < instance.imageChance;

        GameObject spawnedPost;
        // Spawn an image post if we have an image
        if (addImage)
            spawnedPost = Instantiate(instance.imagePostPrefab, instance.contentParent);
        else
            spawnedPost = Instantiate(instance.textPostPrefab, instance.contentParent);

        // PFP (Child 0)
        Sprite pfp = instance.profilePictures[Random.Range(0, instance.profilePictures.Count)];
        spawnedPost.transform.GetChild(0).GetComponent<Image>().sprite = pfp;

        // Name (Child 1)
        string name = instance.usernames[Random.Range(0, instance.usernames.Count)];
        spawnedPost.transform.GetChild(1).GetComponent<TMP_Text>().text = name;

        // Body (Child 2)
        string body = post.body;
        spawnedPost.transform.GetChild(2).GetComponent<TMP_Text>().text = body;

        int buttonIndex = 3;

        if (addImage)
        {
            Sprite image = instance.images[Random.Range(0, instance.images.Count)];
            spawnedPost.transform.GetChild(3).GetComponent<Image>().sprite = image;
            buttonIndex = 4;
        }

        // TODO: Set up button callback
        spawnedPost.transform.GetChild(buttonIndex).GetComponent<Button>().onClick.AddListener(() => Debug.Log("Liked"));
    }


    protected override void OnAppOpened() { }

    protected override BackButtonAction OnBackButtonPressed()
    {
        return BackButtonAction.GoBackAMenu;
    }

    protected override void OnAppClosed() { }
}

public class BranchesPost : MonoBehaviour
{
    public SocialMediaPosts post;
}
