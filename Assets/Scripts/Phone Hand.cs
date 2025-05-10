using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneHand : MonoBehaviour
{
    [Range(0.01f, 1f)]
    public float swayRotSpeed;
    [Range(0.01f, 1f)]
    public float swayDistSpeed;

    public float swayTime;
    [Range(0f, 1f)]
    public float swayDist;
    public float distance;
    [Range(0f, 1f)]
    public float swayRot;
    public float rotation;
    public AnimationCurve curve;

    private float rotTimer;
    private float distTimer;
    private Vector3 startPos;
    private void Start()
    {
        startPos = transform.position;
    }
    private void Update()
    {
        float rotTime = swayTime / swayRotSpeed;
        if (rotTimer < rotTime)
            rotTimer += Time.deltaTime;
        else
            rotTimer = 0f;
        float distTime = swayTime / swayDistSpeed;
        if (distTimer < distTime)
            distTimer += Time.deltaTime;
        else
            distTimer = 0f;

        transform.eulerAngles = Vector3.Lerp(new Vector3(0f,0f,-rotation) * swayRot, new Vector3(0f, 0f, rotation) * swayRot, curve.Evaluate(rotTimer/rotTime));
        transform.position = new Vector3(startPos.x+Mathf.Lerp(-distance * swayDist, distance * swayDist, curve.Evaluate(distTimer/distTime)), startPos.y, startPos.z);
    }

}
