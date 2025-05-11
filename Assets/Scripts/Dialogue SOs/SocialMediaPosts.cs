using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Dialogue/post")]
public class SocialMediaPosts : ScriptableObject
{
    public Type type;
    public string body;

    public enum Type
    {
        Normal,
        ad
    }



}
