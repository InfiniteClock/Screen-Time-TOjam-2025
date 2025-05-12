using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

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

    [Space]
    public AppMenu subMenu;
    public Image subPFP;
    public TMP_Text subName;
    public TMP_Text subBody;

    [Space]
    public List<Button> choiceButtons;
    public List<TMP_Text> choiceTexts;

    public static void AddMessage(Notification.NotificationModes mode, string timestamp = "now")
    {
        Messaging message = RandomMessage();
        AddMessage(message);
        Notification.Send(new NotificationInfo(instance.id, message.sender.ToString(), message.message, timestamp), mode);
    }

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
        spawnedMessage.transform.GetChild(2).GetComponent<TMP_Text>().text = message.message;

        // Callback to open message
        Button button = spawnedMessage.GetComponent<Button>();
        button.onClick.AddListener(() => instance.ClickedMessage(spawnedMessage, message));
    }

    public static Messaging RandomMessage() => instance.allMessages[Random.Range(0, instance.allMessages.Count)];

    void ClickedMessage(GameObject msgObj, Messaging message)
    {
        NotificationManager.DialogueClicked(message);

        subMenu.SetActive(true);
        subPFP.sprite = message.pfp;
        subName.text = message.sender.ToString();
        subBody.text = message.message;

        // Terrible code to randomize between the 3 reponses
        int first = Random.Range(0, 3);
        int second = Random.Range(0, 2);
        if (first == 0) second++;
        else if (first == 1)
            if (second == 1) second = 2;
        var nums = new List<int> { 0, 1, 2 };
        nums.Remove(first); nums.Remove(second);
        int third = nums[0];

        SetButton(0, first);
        SetButton(1, second);
        SetButton(2, third);

        void SetButton(int buttonIndex, int responseIndex)
        {
            
            choiceButtons[buttonIndex].onClick.RemoveAllListeners();
            choiceButtons[buttonIndex].onClick.AddListener(() => ChoiceClicked(msgObj, message.responses[responseIndex].isCorrect));
            choiceTexts[buttonIndex].text = message.responses[responseIndex].text;
        }
    }

    void ChoiceClicked(GameObject msgObj, bool correct)
    {
        subMenu.SetActive(false);

        if (correct)
            ScoreManager.IncrementCorrectOptions();
        else
            ScoreManager.IncrementIncorrectOptions();

        // Get rid of the message itself
        Destroy(msgObj);
    }


    protected override void OnAppOpened() { }

    protected override BackButtonAction OnBackButtonPressed()
    {
        return BackButtonAction.GoBackAMenu;
    }

    protected override void OnAppClosed() { }
}
