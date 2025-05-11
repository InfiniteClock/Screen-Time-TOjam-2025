using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    public static int correctOptions;
    public static int IncorrectOptions;
    public static int EndGameLimit;

    public static int IncrementCorrectOptions()
    {
       return correctOptions++;
    }

    public static int IncrementIncorrectOptions()
    {
        return IncorrectOptions++;
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

    public static void CheckForEndGame()

    {
        if (IncorrectOptions >= EndGameLimit)
        {
            Debug.Log("game end");
        }
    }

}
