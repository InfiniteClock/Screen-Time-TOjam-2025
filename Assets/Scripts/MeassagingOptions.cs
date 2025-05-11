using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Dialogue/Options")]
public class MeassagingOptions : ScriptableObject
{

    public Type type;
    public string message;
    public int Number;

    public enum Type
    {
        Correct,
        Wrong
    }
}
