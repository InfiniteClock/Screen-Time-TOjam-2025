using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClose_Function : MonoBehaviour
{
    public bool isOpen;
    [Tooltip("Time in seconds for open/close to finish")]
    public float screenChangeTime = 0.1f;
    [Tooltip("If there is a scroll bar with a content child, select it here so it doesn't scroll to bottom when opened.")]
    public GameObject content;

    [Space(5)]
    [Tooltip("The position of the screen when opened.")]
    public Vector2 openPos = Vector2.zero;
    [Tooltip("The position of the screen when closed. This should be the origin point where the app is opened from.")]
    public Vector2 closePos = new Vector2(2f, -4f);
    
    private Coroutine coroutine;

    //reference to email selector to update email content
    [SerializeField] private EmailSelector emailSelector;

    private void Start()
    {
        // Hide if we are closed
        if (!isOpen)
        {
            gameObject.transform.localPosition = closePos;
            gameObject.transform.localScale = Vector3.zero;
        }
    }
    public void ChangeScreen()
    {
        // Toggle open vs closed
        if (isOpen)
            Close();
        else
            Open();
    }

    public void Open()
    {
        if (!isOpen)
        {
            //update content
            // I am going to kill you bruno
            if (emailSelector)
                emailSelector.UpdateOpenEmailText();

            isOpen = true;

            coroutine = StartCoroutine(TransitionScreen(1));
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            isOpen = false;

            coroutine = StartCoroutine(TransitionScreen(-1));
        }
    }

    public void SetState(bool open)
    {
        if (open)
            Open();
        else
            Close();
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
                // Move content so it doesn't scroll to the bottom
                if (content != null)
                    content.transform.localPosition = new Vector3(content.transform.localPosition.x, 0f, content.transform.localPosition.z);
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
