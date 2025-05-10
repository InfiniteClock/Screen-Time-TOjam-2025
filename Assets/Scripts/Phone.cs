using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Phone : MonoBehaviour
{
    public int timeHours = 10;
    public int timeMinutes = 0;
    public string timeAMPM = "PM";
    public bool clockActive = true;

    public TextMeshProUGUI clockText;
    public static string CurrentTime { get; private set; } = string.Empty;

    private void Start()
    {
        StartCoroutine(ClockTick());
    }
    private void Update()
    {
        
    }

    private IEnumerator ClockTick(float secondsPerMin = 0.2f)
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
}
