using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailSelector : MonoBehaviour
{
    [SerializeField] private List<Email> PossibleEmails = new List<Email>();

    [SerializeField] private GameObject emailPrefab;
    private GameObject spawnedEmail;
    public static GameObject currentEmailSelection;
    [SerializeField] private Transform location;

    private int chosenEmail;
    private void instantiateEmail(GameObject email)
    {
        spawnedEmail = GameObject.Instantiate(emailPrefab, location);
        
        //picking random email
        chosenEmail = Random.Range(0, PossibleEmails.Count);
        
        //seting newly spawned email as spawned email

        spawnedEmail.GetComponentInChildren<TextMeshProUGUI>().text = PossibleEmails[chosenEmail].subject.ToString();
        spawnedEmail.transform.GetChild(0).GetComponent<Image>().sprite = PossibleEmails[chosenEmail].profileIcon;

        spawnedEmail.GetComponent<Button>().onClick.AddListener(GameObject.Find("Open Email").GetComponent<OpenClose_Function>().ChangeScreen);
        



    }
    private void Update()
    {
        TestSpawnEmail();
    }
    private void TestSpawnEmail()
    {
        Debug.Log("running");
        if (Input.GetKeyUp(KeyCode.T)) 
        {
            Debug.Log("spawned email");
            instantiateEmail(emailPrefab);
        }
    }
}
