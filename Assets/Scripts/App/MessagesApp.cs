using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class MessagesApp : App
{
    private static MessagesApp instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject messagePrefab;
    public Transform contentParent;
    public List<Messaging> allMessages;

    /// <summary>
    /// Adds a message - doesn't send a notification
    /// </summary>
    public static void AddMessage(Messaging message)
    {
        GameObject spawnedMessage = Instantiate(instance.messagePrefab, instance.contentParent);

        // PFP (Child 0)
        spawnedMessage.transform.GetChild(0).GetComponent<Image>().sprite = message.pfp;

        // Name (Child 1)
        spawnedMessage.transform.GetChild(1).GetComponent<TMP_Text>().text = message.sender.ToString();

        // Body (Child 2)
        spawnedMessage.transform.GetChild(3).GetComponent<TMP_Text>().text = message.message;

        // Callback to open message
        Button button = spawnedMessage.GetComponent<Button>();
        button.onClick.AddListener(() => instance.ClickedMessage(message));
    }

    void ClickedMessage(Messaging message)
    {
        NotificationManager.DialogueClicked(message);

        // TODO: Open message in sub-menu

        /*
        // Make like button red
        button.GetComponent<Image>().color = Color.red;

        if (post.type == SocialMediaPosts.Type.ad)
            ScoreManager.IncrementIncorrectOptions();
        else
            ScoreManager.IncrementCorrectOptions();
        */
    }


    protected override void OnAppOpened() { }

    protected override BackButtonAction OnBackButtonPressed()
    {
        return BackButtonAction.GoBackAMenu;
    }

    protected override void OnAppClosed() { }
}
