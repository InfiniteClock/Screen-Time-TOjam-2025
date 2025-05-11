using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectingEmail : MonoBehaviour
{
   
    public void selectEmail()
    {
        EmailSelector.currentEmailSelection = gameObject;
        Debug.Log(EmailSelector.currentEmailSelection);
    }

}
