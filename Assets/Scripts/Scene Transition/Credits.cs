using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public string mainMenu;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(mainMenu);
        }
    }
}
