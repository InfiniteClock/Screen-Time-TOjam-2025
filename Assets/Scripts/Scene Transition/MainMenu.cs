using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameScene;
    public string creditsScene;
    public void PressadaButton()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void PressadaOtherButton()
    {
        SceneManager.LoadScene(creditsScene);
    }
}
