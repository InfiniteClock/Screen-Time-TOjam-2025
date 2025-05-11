using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Dialogue/Email")]
public class Email : ScriptableObject
{
    public Type type;
    public Sender sender;
    public Sprite profileIcon;
    [TextArea(2, 8)]
    public string subject;
    [TextArea(2, 8)]
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
        Mom,
        Dad,
        GF,
        Anthony
    }
}
