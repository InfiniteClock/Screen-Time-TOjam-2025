using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Email")]
public class Email : ScriptableObject
{
    public Type type;
    public Sender sender;
    public string subject;
    public string body;

    public enum Type
    {
        Normal,
        Spam
    }

    public enum Sender
    {
        Boss,
        JerkBet375,
        Greg,
        DingCambodia,
        YellCanada,
        WetFlicks,
        Loopsoft,

    }
}
