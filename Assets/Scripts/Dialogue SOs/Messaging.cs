using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sender
{
    Mom,
    Dad,
    GF,
    BobBobson,
    CarlosRoomate,
    AnthonyAnnoyingFriend,
    DeliveryGuy,
}

[CreateAssetMenu(menuName = "Dialogue/Messaging")]
public class Messaging : ScriptableObject
{
    public Sprite pfp;
    public Sender sender;
    public string message;


    public List<Response> responses;



    [Serializable]
    public struct Response
    {
        public string text;
        public bool isCorrect;
    }
}
