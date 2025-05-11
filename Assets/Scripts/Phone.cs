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
    public static string CurrentTime { get; private set; } = string.Empty;
    private Coroutine ClockRoutine;
    private void Start()
    {
        // Run tutorials first
        // Set initial notifications for night
        StartNight();
    }

    private IEnumerator ClockTick(float secondsPerMin = 2f)
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
            }
            int minuteTens = timeMinutes / 10;
            int minuteOnes = timeMinutes % 10;
            CurrentTime = timeHours + ":" + minuteTens + "" + minuteOnes + timeAMPM;
            clockText.text = CurrentTime;
            yield return new WaitForSeconds(secondsPerMin);
        }
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
    }
}
