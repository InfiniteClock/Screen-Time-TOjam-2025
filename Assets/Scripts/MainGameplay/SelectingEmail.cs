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

}
