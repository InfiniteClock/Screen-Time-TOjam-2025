using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Phone : MonoBehaviour
{
    public int timeHours = 10;
    public int timeMinutes = 0;
    public string timeAMPM = "PM";
    public int alarmHours = 8;
    public int alarmMinutes = 0;
    public int maxNotifsBeforeEnd = 0;

    public bool clockActive = true;
    public int night = 1;
    public TextMeshProUGUI clockText;
    public GameObject thoughtBubble;
    public TextMeshProUGUI tutText;

    public PhoneHand hand;
    public PostFXAdjust FX;

    public GameObject passoutScreen;
    public GameObject gameoverScreen;
    public static string CurrentTime { get; private set; } = string.Empty;
    private Coroutine ClockRoutine;
    private Coroutine TutorialRoutine;
    private float currentSleepy = -0.4f;
    private float permanentSleepy = 0f;

    private bool firstTimeMail = true;
    private bool firstTimeMessages = true;
    private bool firstTimeBranches = true;
    private void Start()
    {
        StartNight();
    }

    private IEnumerator ClockTick(float secondsPerMin = 1f)
    {
        while (clockActive)
        {
            if (timeMinutes < 59)
                timeMinutes += 1;
            else
            {
                timeMinutes -= 59;
                if (timeHours < 12)
                {
                    timeHours += 1;
                    if (timeHours == 12)
                    {
                        if (timeAMPM == "AM")
                            timeAMPM = "PM";
                        else
                            timeAMPM = "AM";
                    }
                }
                else
                {
                    timeHours -= 11;

                }
                AdjustSleepy(0.2f);
            }
            int minuteTens = timeMinutes / 10;
            int minuteOnes = timeMinutes % 10;
            CurrentTime = timeHours + ":" + minuteTens + "" + minuteOnes + timeAMPM;
            clockText.text = CurrentTime;
            yield return new WaitForSeconds(secondsPerMin);
        }
    }
    public void AdjustSleepy(float currentSleep)
    {
        // Increase current night's sleepiness modifier
        currentSleepy += currentSleep;

        // If total sleepiness surpasses 1 (100%) then pass out
        if (currentSleepy + permanentSleepy >= 1f)
        {
            StartCoroutine(Passout());
            return;
        }

        float ratio1 = currentSleepy * 0.5f + permanentSleepy * 0.5f;
        float ratio2 = currentSleepy * 0.75f + permanentSleepy * 0.25f;
        float ratio3 = currentSleepy * 0.25f + permanentSleepy * 0.75f;
        float ratio4 = currentSleepy * 0f + permanentSleepy * 1.0f;
        float ratio5 = currentSleepy * 1.0f + permanentSleepy * 0.0f;
        hand.swayDistSpeed = ratio3;
        hand.swayRotSpeed = ratio3;
        hand.swayDist = ratio4;
        hand.swayRot = ratio4;
        FX.DistortLensIntensity(Mathf.Lerp(0f,-10f,ratio1), Mathf.Lerp(0f,-30f,ratio1), Mathf.Lerp(5f,2f,ratio1));
        FX.DistortLensScale(Mathf.Lerp(1f, 0.95f, ratio3), Mathf.Lerp(1f, 1.05f, ratio5), Mathf.Lerp(5f, 2f, ratio1));
        FX.ChromaticIntensity(Mathf.Lerp(0f, 0.5f, ratio1), Mathf.Lerp(0f, 1f, ratio1), Mathf.Lerp(5f, 2f, ratio1));
        FX.VignetteIntensity(Mathf.Lerp(0f, 0.3f, ratio5), Mathf.Lerp(0f, 0.6f, ratio5), Mathf.Lerp(5f, 2f, ratio5));
        FX.VignetteSmoothness(Mathf.Lerp(1f, 0.75f, ratio2), 1f, Mathf.Lerp(5f, 2f, ratio2));

    }
    private IEnumerator Passout()
    {
        if (ClockRoutine != null) 
            StopCoroutine(ClockRoutine);
        if (TutorialRoutine != null)
            StopCoroutine(TutorialRoutine);

        // Time until full passout should be half of time to full Vignette Intensity * 2.5
        FX.VignetteIntensity(0.3f, 1f, 4f);

        float timer = 0f;
        SpriteRenderer screen; 

        // Passout or Gameover screen
        if (currentSleepy / 2f + permanentSleepy >= 1f || NotificationManager.Notifications.Count >= maxNotifsBeforeEnd)
            screen = gameoverScreen.GetComponent<SpriteRenderer>();
        else
            screen = passoutScreen.GetComponent<SpriteRenderer>();
        yield return new WaitForSeconds(5f);
        while (timer < 5f)
        {
            timer += Time.deltaTime;
            screen.color = new Color(255f, 255f, 255f, Mathf.Lerp(0f, 1f, timer / 5f));
            yield return null;
        }
        FX.DistortLensIntensity();
        FX.DistortLensScale();
        FX.ChromaticIntensity();
        yield return new WaitForSeconds(10f);
        if (currentSleepy / 2f + permanentSleepy >= 1f || NotificationManager.Notifications.Count >= maxNotifsBeforeEnd)
            SceneManager.LoadScene("Credits");
        else
            EndNight();
    }
    public void StartNight()
    {
        passoutScreen.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        gameoverScreen.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        timeHours = 10;
        timeMinutes = 0;
        timeAMPM = "PM";
        ClockRoutine = StartCoroutine(ClockTick());

        switch (night)
        {
            case 1:
                App.Get(App.ID.Mail).ShowButtonOnHomeScreen = true;
                App.Get(App.ID.Messages).ShowButtonOnHomeScreen = false;
                App.Get(App.ID.CommunityBoard).ShowButtonOnHomeScreen = false;

                DialogueSpawner.Instance.Day1();

                /*
                EmailSelector.AddEmail(Notification.NotificationModes.NotificationAndPopup);
                BranchesApp.AddPost(Notification.NotificationModes.NotificationAndPopup);
                MessagesApp.AddMessage(Notification.NotificationModes.NotificationAndPopup);
                */


                if (TutorialRoutine != null)
                    StopCoroutine(TutorialRoutine);
                TutorialRoutine = StartCoroutine(TutorialOne(5f));
                break;
            case 2:
                App.Get(App.ID.Mail).ShowButtonOnHomeScreen = true;
                App.Get(App.ID.Messages).ShowButtonOnHomeScreen = true;
                App.Get(App.ID.CommunityBoard).ShowButtonOnHomeScreen = false;

                DialogueSpawner.Instance.Day2();

                if (TutorialRoutine != null)
                    StopCoroutine(TutorialRoutine);
                TutorialRoutine = StartCoroutine(TutorialTwo(5f));
                break;
            case 3:
                App.Get(App.ID.Mail).ShowButtonOnHomeScreen = true;
                App.Get(App.ID.Messages).ShowButtonOnHomeScreen = true;
                App.Get(App.ID.CommunityBoard).ShowButtonOnHomeScreen = true;

                DialogueSpawner.Instance.Day3();

                if (TutorialRoutine != null)
                    StopCoroutine(TutorialRoutine);
                TutorialRoutine = StartCoroutine(TutorialThree(5f));
                break;
            default:
                Debug.Log("Night under construction!");
                break;
        }
    }
    public void EndNight()
    {
        if (ClockRoutine != null)
            StopCoroutine(ClockRoutine);
        if (TutorialRoutine != null)
            StopCoroutine(TutorialRoutine);


        // Switch to night over scene

        int sleptHours = alarmHours - timeHours;
        int sleptMinutes = alarmMinutes - timeMinutes;
        if (sleptMinutes < 0)
        {
            sleptMinutes += 60;
            sleptHours--;
        }
        string sleepGained = sleptHours + ":" + sleptMinutes + " hrs";

        // Change to next night
        permanentSleepy += Mathf.Clamp(currentSleepy, 0f, 1f) / 2f;
        if (night < 3)
            night++;
        else
            // Win the game
        StartCoroutine(NewDay());
    }

    public IEnumerator TutorialOne(float duration)
    {
        float timer = 0f;
        while (timer < 0.25f)
        {
            timer += Time.deltaTime;
            thoughtBubble.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / 0.25f);
            yield return null;
        }

        timer = 0f;
        tutText.text = "Welcome to Screentime!";
        yield return new WaitForSeconds(duration);

        timer = 0f;
        tutText.text = "Your goal is to clear your notifications so you can get to bed on time.";
        yield return new WaitForSeconds(duration);

        timer = 0f;
        tutText.text = "Make sure you are answering your notifications properly - wrong answers can lead to more notifications popping up.";
        yield return new WaitForSeconds(duration);

        timer = 0f;
        tutText.text = "Press the BELL button to see your notifications, and see how late it is.";
        yield return new WaitForSeconds(duration);

        timer = 0f;
        tutText.text = "Press the SQUARE button to return to the home page.";
        yield return new WaitForSeconds(duration);

        timer = 0f;
        tutText.text = "Press the BACK ARROW button to return to the last page you were on.";
        yield return new WaitForSeconds(duration);

        timer = 0f;
        tutText.text = "You only have one APP for now, so all your notifications will be in there. Click it to see what's inside and clear your notifications!";
        yield return new WaitForSeconds(duration);

        tutText.text = "";
        timer = 0f;
        while (timer < 0.25f)
        {
            timer += Time.deltaTime;
            thoughtBubble.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / 0.25f); 
            yield return null;
        }
    }
    public IEnumerator TutorialTwo(float duration)
    {
        float timer = 0f;
        while (timer < 0.25f)
        {
            timer += Time.deltaTime;
            thoughtBubble.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / 0.25f);
            yield return null;
        }

        timer = 0f;
        tutText.text = "You made it to night 2!";
        yield return new WaitForSeconds(duration);

        timer = 0f;
        tutText.text = "Careful not to stay up too late! \nYou may notice some adverse effects as you miss out on your sleep.";
        yield return new WaitForSeconds(duration);

        timer = 0f;
        tutText.text = "Time passes quickly when you're on your phone.";
        yield return new WaitForSeconds(duration);

        timer = 0f;
        tutText.text = "You may notice you have a new app! \nOpen it up and see what's inside.";
        yield return new WaitForSeconds(duration);

        tutText.text = "";
        timer = 0f;
        while (timer < 0.25f)
        {
            timer += Time.deltaTime;
            thoughtBubble.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / 0.25f);
            yield return null;
        }
    }
    public IEnumerator TutorialThree(float duration)
    {
        float timer = 0f;
        while (timer < 0.25f)
        {
            timer += Time.deltaTime;
            thoughtBubble.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / 0.25f);
            yield return null;
        }

        timer = 0f;
        tutText.text = "You made it to night 3! \nFeeling sleepy yet?";
        yield return new WaitForSeconds(duration);

        timer = 0f;
        tutText.text = "You've got another app! Let's see what this one is like.";
        yield return new WaitForSeconds(duration);

        tutText.text = "";
        timer = 0f;
        while (timer < 0.25f)
        {
            timer += Time.deltaTime;
            thoughtBubble.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / 0.25f);
            yield return null;
        }
    }
    public IEnumerator TutorialMail(float duration)
    {
        float timer = 0f;
        while (timer < 0.25f)
        {
            timer += Time.deltaTime;
            thoughtBubble.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / 0.25f);
            yield return null;
        }

        timer = 0f;
        tutText.text = "This is the mail app. \nThe mail needs to be sorted out.";
        yield return new WaitForSeconds(duration);

        timer = 0f;
        tutText.text = "Tap a message for more details, then either delete or forward it.";
        yield return new WaitForSeconds(duration);

        timer = 0f;
        tutText.text = "Spam mail needs to be deleted. \nTap the X on the message to delete them.";
        yield return new WaitForSeconds(duration);

        timer = 0f;
        tutText.text = "Important mail needs to be forwarded. \nTap the arrow on the message to delete them.";
        yield return new WaitForSeconds(duration);

        tutText.text = "";
        timer = 0f;
        while (timer < 0.25f)
        {
            timer += Time.deltaTime;
            thoughtBubble.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / 0.25f);
            yield return null;
        }
    }
    public IEnumerator TutorialMessages(float duration)
    {
        float timer = 0f;
        while (timer < 0.25f)
        {
            timer += Time.deltaTime;
            thoughtBubble.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / 0.25f);
            yield return null;
        }

        timer = 0f;
        tutText.text = "This is the messages app! \nMake sure to answer everyone's messages nicely.";
        yield return new WaitForSeconds(duration);

        timer = 0f;
        tutText.text = "Tap a message for more details, then pick a response.";
        yield return new WaitForSeconds(duration);

        timer = 0f;
        tutText.text = "Hint: Most people prefer when you pick a nice response.";
        yield return new WaitForSeconds(duration);

        tutText.text = "";
        timer = 0f;
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            thoughtBubble.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / 0.25f);
            yield return null;
        }
    }
    public IEnumerator TutorialBranches(float duration)
    {
        float timer = 0f;
        while (timer < 0.25f)
        {
            timer += Time.deltaTime;
            thoughtBubble.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / 0.25f);
            yield return null;
        }

        timer = 0f;
        tutText.text = "This is the community forums app. \nLike content while ignoring ads.";
        yield return new WaitForSeconds(duration);

        timer = 0f;
        tutText.text = "Tap the heart to like a Branch. \nIgnore the ads or you will get more messages.";
        yield return new WaitForSeconds(duration);

        tutText.text = "";
        timer = 0f;
        while (timer < 0.25f)
        {
            timer += Time.deltaTime;
            thoughtBubble.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / 0.25f);
            yield return null;
        }
    }
    private IEnumerator NewDay()
    {
        float timer = 0f;
        while (timer < 3f)
        {
            timer += Time.deltaTime;
            // Perform any in between night effects here
            yield return null;
        }
        StartNight();
    }

    public void NewAppOpened(int appNumber)
    {
        if (appNumber == 1 && firstTimeMail)
        {
            if (TutorialRoutine != null)
                StopCoroutine(TutorialRoutine);
            TutorialRoutine = StartCoroutine(TutorialMail(5f));
            firstTimeMail = false;
        }
        if (appNumber == 2 && firstTimeMessages)
        {
            if (TutorialRoutine != null)
                StopCoroutine(TutorialRoutine);
            TutorialRoutine = StartCoroutine(TutorialMessages(5f));
            firstTimeMessages = false;
        }
        if (appNumber == 3 && firstTimeBranches)
        {
            if (TutorialRoutine != null)
                StopCoroutine(TutorialRoutine);
            TutorialRoutine = StartCoroutine(TutorialBranches(5f));
            firstTimeBranches = false;
        }
    }
}
