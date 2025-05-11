using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Phone : MonoBehaviour
{
    public int timeHours = 10;
    public int timeMinutes = 0;
    public string timeAMPM = "PM";
    public int alarmHours = 8;
    public int alarmMinutes = 0;

    public bool clockActive = true;
    public int night = 1;
    public TextMeshProUGUI clockText;

    public PhoneHand hand;
    public PostFXAdjust FX;
    public static string CurrentTime { get; private set; } = string.Empty;
    private Coroutine ClockRoutine;
    private float currentSleepy = -0.4f;
    private float permanentSleepy = 0f;
    private void Start()
    {
        // Run tutorials first
        // Set initial notifications for night
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
        // 50/50 ratio to start
        currentSleepy += currentSleep;

        if (currentSleepy + permanentSleepy > 1f)
        {
            // Passout
        }
        float ratio1 = currentSleepy * 0.5f + permanentSleepy * 0.5f;
        float ratio2 = currentSleepy * 0.75f + permanentSleepy * 0.25f;
        float ratio3 = currentSleepy * 0.25f + permanentSleepy * 0.75f;
        float ratio4 = currentSleepy * 0f + permanentSleepy * 1.0f;
        float ratio5 = currentSleepy * 1.0f + permanentSleepy * 0.0f;
        hand.swayDistSpeed = ratio3;
        hand.swayRotSpeed = ratio3;
        hand.swayTime = ratio4;
        hand.swayRot = ratio4;
        FX.DistortLensIntensity(Mathf.Lerp(0f,-10f,ratio1), Mathf.Lerp(0f,-30f,ratio1), Mathf.Lerp(5f,2f,ratio1));
        FX.DistortLensScale(Mathf.Lerp(1f, 0.95f, ratio3), Mathf.Lerp(1f, 1.05f, ratio5), Mathf.Lerp(5f, 2f, ratio1));
        FX.ChromaticIntensity(Mathf.Lerp(0f, 0.5f, ratio1), Mathf.Lerp(0f, 1f, ratio1), Mathf.Lerp(5f, 2f, ratio1));
        FX.VignetteIntensity(Mathf.Lerp(0f, 0.3f, ratio5), Mathf.Lerp(0f, 0.6f, ratio5), Mathf.Lerp(5f, 2f, ratio5));
        FX.VignetteSmoothness(Mathf.Lerp(1f, 0.75f, ratio2), 1f, Mathf.Lerp(5f, 2f, ratio2));

    }
    public void StartNight()
    {
        timeHours = 10;
        timeMinutes = 0;
        timeAMPM = "PM";
        ClockRoutine = StartCoroutine(ClockTick());
    }
    public void EndNight()
    {
        StopCoroutine(ClockRoutine);

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
    }
}
