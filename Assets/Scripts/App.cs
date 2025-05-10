using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{
    public ID id;
    public string homeScreenName;
    public string notificationName;
    public Sprite icon;

    public static Dictionary<ID, App> All = new Dictionary<ID, App>();

    // Add and remove from 'All' list
    private void OnEnable()
    {
        All.Add(id, this);
    }
    private void OnDisable()
    {
        All.Remove(id);
    }

    

    public static App Get(ID id) => All[id];

    public enum ID
    {
        None,
        Mail,
        Messages,
        CommunityBoard,
    }
}
