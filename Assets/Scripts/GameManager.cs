using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public static bool IsGameClosing { get; private set; }

    private void OnApplicationQuit()
    {
        IsGameClosing = true;
    }
}
