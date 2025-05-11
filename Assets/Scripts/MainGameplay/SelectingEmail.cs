using System.Collections;
using System.Collections.Generic;
using Tobo.Audio;
using UnityEngine;

public class SelectingEmail : MonoBehaviour
{
    public int emailID;
    [SerializeField] EmailSelector selector;
    [SerializeField] GameObject confettiPrefab;
    [SerializeField] GameObject SpamButtonObj;
    [SerializeField] GameObject FowardButtonObj;
 

    private void Start()
    {
        selector = GameObject.Find("EmailSelector").GetComponent<EmailSelector>();
        //selector = transform.Find("EmailSelector").GetComponent<EmailSelector>();
    }

    public void selectEmail()
    {
        Debug.Log("clicking on email");
        
        EmailSelector.currentEmailSelection = gameObject;
        selector.selectedEmail = this;
        
    }

    public void SpamButton()
    {
        for (int i  = 0; i < selector.PossibleEmails.Count;i++)
        {
            if(i == emailID)
            {
                if (selector.PossibleEmails[i].type == Email.Type.Spam)
                {
                    ScoreManager.IncrementCorrectOptions();
                    Debug.Log("correct option: " + ScoreManager.correctOptions);
                    Instantiate(confettiPrefab,SpamButtonObj.transform.position,confettiPrefab.transform.rotation);
                    Sound.Confetti.PlayDirect();

                    Destroy(gameObject);
                }
                else
                {
                    ScoreManager.IncrementIncorrectOptions();
                    Debug.Log("WRONG OPTION: " + ScoreManager.IncorrectOptions);
                    Destroy(gameObject);
                }
            }
        }

    }

    public void FowardButton()
    {
        for (int i = 0; i < selector.PossibleEmails.Count;i++)
        {
            if (i == emailID)
            {
                if (selector.PossibleEmails[i].type == Email.Type.Normal)
                {
                    ScoreManager.IncrementCorrectOptions();
                    Debug.Log("correct option: " + ScoreManager.correctOptions);
                    Instantiate(confettiPrefab, FowardButtonObj.transform.position, confettiPrefab.transform.rotation);
                    Sound.Confetti.PlayDirect();

                    Destroy(gameObject);
                }
                else
                {
                    ScoreManager.IncrementIncorrectOptions();
                    Debug.Log("WRONG OPTION: " + ScoreManager.IncorrectOptions);
                    Destroy(gameObject);
                }
            }
        }
    }


}
