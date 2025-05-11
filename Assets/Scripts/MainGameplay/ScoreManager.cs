using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    public static int correctOptions;
    public static int IncorrectOptions;
    public static int EndGameLimit;

    public static void IncrementCorrectOptions()
    {
        correctOptions++;
        OptionClicked();
    }

    public static void IncrementIncorrectOptions()
    {
        IncorrectOptions++;
        OptionClicked();
    }

    public static int DecrimentCorrectOptions()
    {
        return correctOptions--;
    }

    public static int DecrimentIncorrectOptions()
    {
        return IncorrectOptions--;
    }

    public static void resetVariables()
    {
        correctOptions = 0;
        IncorrectOptions = 0;
    }

    public static void OptionClicked()
    {
        int totalThingsClicked = IncorrectOptions + correctOptions;
        int totalThingsToClick = DialogueSpawner.MailSpawnedToday + DialogueSpawner.MessagesSpawnedToday + DialogueSpawner.NormalPostsSpawnedToday;

        if (totalThingsClicked == totalThingsToClick)
        {
            Debug.Log("Ended night");   
            Phone.Instance.EndNight();
        }
    }

    public static void CheckForEndGame()

    {
        if (IncorrectOptions >= EndGameLimit)
        {
            Debug.Log("game end");
        }
    }

}
