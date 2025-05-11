using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tobo.Audio;
using UnityEngine;
using UnityEngine.UI;

public class EmailSelector : MonoBehaviour
{
    [SerializeField] public List<Email> PossibleEmails = new List<Email>();

    [SerializeField] private GameObject emailPrefab;
    private GameObject spawnedEmail;
    public static GameObject currentEmailSelection;
    [SerializeField] private Transform location;


    [Header("Open Email References")]
    [SerializeField] private Image profilePicture;
    [SerializeField] private TextMeshProUGUI sender;
    [SerializeField] private TextMeshProUGUI subject;
    [SerializeField] private TextMeshProUGUI TextContent;

    [SerializeField] public SelectingEmail selectedEmail;

    private int chosenEmail;
    [SerializeField] private OpenClose_Function openClose_Function;

    
    [SerializeField] GameObject confettiPrefab;
    [SerializeField] GameObject SpamButtonObj;
    [SerializeField] GameObject FowardButtonObj;
   

    private void instantiateEmail(GameObject email)
    {
        spawnedEmail = GameObject.Instantiate(emailPrefab, location);
        
        //picking random email
        chosenEmail = Random.Range(0, PossibleEmails.Count);
        
        //seting newly spawned email as spawned email

        spawnedEmail.GetComponentInChildren<TextMeshProUGUI>().text = PossibleEmails[chosenEmail].subject.ToString();
        spawnedEmail.transform.GetChild(0).GetComponent<Image>().sprite = PossibleEmails[chosenEmail].profileIcon;

        spawnedEmail.GetComponent<Button>().onClick.AddListener(() => GameObject.Find("Open Email").GetComponent<AppMenu>().SetActive(true));

        spawnedEmail.GetComponent<SelectingEmail>().emailID = chosenEmail; 
       
    }
    private void Update()
    {
        TestSpawnEmail();
    }
    private void TestSpawnEmail()
    {
        if (Input.GetKeyUp(KeyCode.T)) 
        {
            Debug.Log("spawned email");
            instantiateEmail(emailPrefab);
        }
    }

    public void UpdateOpenEmailText()
    {
        profilePicture.sprite = PossibleEmails[selectedEmail.emailID].profileIcon;
        sender.text = PossibleEmails[selectedEmail.emailID].sender.ToString();
        subject.text = PossibleEmails[selectedEmail.emailID].subject.ToString();
        TextContent.text = PossibleEmails[selectedEmail.emailID].body.ToString();

    }
    public void openEmailSpamButton()
    {
        for (int i = 0; i < PossibleEmails.Count; i++)
        {
            if (i == selectedEmail.emailID)
            {
                if (PossibleEmails[i].type == Email.Type.Spam)
                {
                    ScoreManager.IncrementCorrectOptions();
                    Debug.Log("correct option: " + ScoreManager.correctOptions);
                  
                    Instantiate(confettiPrefab, SpamButtonObj.transform.position, confettiPrefab.transform.rotation);
                    Sound.Confetti.PlayDirect();
                    
                    openClose_Function.Close();
                    Destroy(selectedEmail.gameObject);
                }
                else
                {
                    ScoreManager.IncrementIncorrectOptions();
                    Debug.Log("WRONG OPTION: " + ScoreManager.IncorrectOptions);
                    openClose_Function.Close();
                    Destroy(selectedEmail.gameObject);
                }
            }
        }
    }

    public void openEmailFowardButton()
    {
        for (int i = 0; i < PossibleEmails.Count; i++)
        {
            if (i == selectedEmail.emailID)
            {
                if (PossibleEmails[i].type == Email.Type.Normal)
                {
                    ScoreManager.IncrementCorrectOptions();
                    Debug.Log("correct option: " + ScoreManager.correctOptions);
                    openClose_Function.Close();
                    
                    Instantiate(confettiPrefab, FowardButtonObj.transform.position, confettiPrefab.transform.rotation);
                    Sound.Confetti.PlayDirect();

                    Destroy(selectedEmail.gameObject);

                }
                else
                {
                    ScoreManager.IncrementIncorrectOptions();
                    Debug.Log("WRONG OPTION: " + ScoreManager.IncorrectOptions);
                    openClose_Function.Close();
                    Destroy(selectedEmail.gameObject);
                }
            }
        }
    }

}
