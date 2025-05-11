using System.Collections;
using System.Collections.Generic;
using Tobo.Audio;
using Tobo.Util;
using UnityEngine;

public class HandCursor : MonoBehaviour
{
    public GameObject hover, tap;

    private void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition).WithZ(0);

        tap.SetActive(Input.GetKey(KeyCode.Mouse0));
        hover.SetActive(!Input.GetKey(KeyCode.Mouse0));

        if (Input.GetKeyDown(KeyCode.Mouse0))
            Sound.Tap.PlayDirect();
    }
}
