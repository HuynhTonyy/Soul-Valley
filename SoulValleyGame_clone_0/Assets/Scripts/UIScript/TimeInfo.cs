using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeInfo : MonoBehaviour, ITimeTracker
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText; 
    

    // Start is called before the first frame update
    void Start()
    {
        TimeManager.Instance.RegisterTracker(this);
    }

    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        int hour = timeStamp.hour;
        int minute = timeStamp.minute;
        int day = timeStamp.day;
        string season = timeStamp.season.ToString();
        int year = timeStamp.year;
        string dayOfTheWeek = timeStamp.GetDayOfTheWeek().ToString();
        string prefix = " AM";
        if(hour > 12)
        {
            hour -= 12;
            prefix = " PM";
        }
        timeText.SetText(hour + ":" + minute.ToString("00") + prefix);
        dateText.SetText(season + " " + day + " (" + dayOfTheWeek.Substring(0,3) + ")");
    }
}
