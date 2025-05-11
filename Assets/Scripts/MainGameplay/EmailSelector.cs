using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmailSelector : MonoBehaviour
{
    [SerializeField] private List<ScriptableObject> PossibleEmails = new List<ScriptableObject>();
    [SerializeField] private TextMeshProUGUI sender;
    [SerializeField] private GameObject emailPrefab;
    private GameObject spawnedEmail;

    private int chosenEmail;



    private void instantiateEmail(GameObject email)
    {
        //picking random email
        chosenEmail = Random.Range(0, PossibleEmails.Count);
        
        //seting newly spawned email as spawned email
        spawnedEmail = GameObject.Instantiate(emailPrefab);
       
        //spawnedEmail
        
    }
}
