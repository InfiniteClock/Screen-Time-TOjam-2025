using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectingEmail : MonoBehaviour
{
    public int emailID;
    [SerializeField] EmailSelector selector;

    private void Start()
    {
        selector = GameObject.Find("EmailSelector").GetComponent<EmailSelector>();
        //selector = transform.Find("EmailSelector").GetComponent<EmailSelector>();
    }

    public void selectEmail()
    {
        Debug.Log("clicking on email");
        
        EmailSelector.currentEmailSelection = gameObject;
        selector.selectedEmail = gameObject.GetComponent<SelectingEmail>();
        
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
