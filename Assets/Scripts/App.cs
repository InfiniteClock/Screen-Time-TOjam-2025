using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{
    public AppDescriptor descriptor; // Holds details like name, sprite

    public static Dictionary<ID, App> All = new Dictionary<ID, App>();

    // Add and remove from 'All' list
    private void OnEnable()
    {
        All.Add(descriptor.id, this);
    }
    private void OnDisable()
    {
        All.Remove(descriptor.id);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
