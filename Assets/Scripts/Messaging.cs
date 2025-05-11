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

    public Sender sender;
    public string message;
    public string correctOption1;
    public string correctOption2;
    public string wrongOption;

    public List<Response> responses;



    [Serializable]
    public struct Response
    {
        public string text;
        public bool isCorrect;
    }
}
