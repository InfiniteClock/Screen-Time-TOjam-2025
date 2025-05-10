using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Notifications : MonoBehaviour
{
    public bool open;
    public float screenChangeTime;
    public GameObject content;

    private Vector2 openPos = Vector2.zero;
    private Vector2 closePos = new Vector2(2f, -4f);
    private Coroutine coroutine;

    private void Start()
    {
        gameObject.transform.localPosition = closePos;
        gameObject.transform.localScale = Vector3.zero;
        open = false;
    }
    public void ChangeScreen()
    {
        if (!open)
        {
            coroutine = StartCoroutine(TransitionScreen(1));
            open = true;
        }
        else
        {
            coroutine = StartCoroutine(TransitionScreen(-1));
            open = false;
        }
    }
    private IEnumerator TransitionScreen(int direction)
    {
        float timer = 0f;
        while (timer < screenChangeTime)
        {
            timer += Time.deltaTime;
            if (direction > 0)
            {
                gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / screenChangeTime);
                gameObject.transform.localPosition = Vector2.Lerp(closePos, openPos, timer / screenChangeTime);
                content.transform.localPosition = new Vector3 (content.transform.localPosition.x, 0f, content.transform.localPosition.z);
            }
            else
            {
                gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, 1 - timer / screenChangeTime);
                gameObject.transform.localPosition = Vector2.Lerp(openPos, closePos, timer / screenChangeTime);
            }
            yield return null;
        }
    }
}
