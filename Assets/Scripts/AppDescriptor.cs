using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/App Descriptor")]
public class AppDescriptor : ScriptableObject
{
    public App.ID id;
    public string homeScreenName;
    public string notificationName;
    public Sprite icon;

    private void OnValidate()
    {
        // Set names to the object name if they are empty
        if (string.IsNullOrEmpty(homeScreenName))
            homeScreenName = name;
        if (string.IsNullOrEmpty(notificationName))
            notificationName = name;
    }
}
