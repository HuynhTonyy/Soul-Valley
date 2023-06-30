using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameTimeStamp
{
    public int year;
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }
    //With this set up the first day of the game gonna be sunday
    public enum DayOfTheWeek
    {
        Saturday,
        Sunday, 
        Monday, 
        Tuesday, 
        Wednesday,
        Thursday,
        Friday
    }
    public Season season;
    public int day;
    public int hour;
    public int minute;
    public GameTimeStamp(int year, Season season, int day, int hour, int minute)
    {
        this.year = year;
        this.season = season;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
    }
    public GameTimeStamp(GameTimeStamp gameTimeStamp)
    {
        this.year = gameTimeStamp.year;
        this.season = gameTimeStamp.season;
        this.day = gameTimeStamp.day;
        this.hour = gameTimeStamp.hour;
        this.minute = gameTimeStamp.minute;
    }
    public void UpdateClock()
    {
        minute++;
        if(minute >= 60)
        {
            minute = 0;
            hour++;
        }
        if (hour >= 24)
        {
            hour = 0;
            day++;
        }
        if (day > 30)
        {
            day = 1;
            if (season == Season.Winter)
            {
                season = Season.Spring;
                year++;
            }
            else
            {
                season++;
            }
        }
    }
    public DayOfTheWeek GetDayOfTheWeek()
    {
        int daysPassed = YearsToDays(year) + SeasonsToDays(season) + day;
        int dayIndex = daysPassed % 7;
        return (DayOfTheWeek)dayIndex;
    }
    public static int HoursToMinutes(int hours)
    {
        return hours * 60;
    }
    public static int DaysToHours(int days)
    {
        return days * 24;
    }
    public static int SeasonsToDays(Season season)
    {
        int seasonIndex = (int)season;
        return seasonIndex * 30;
    }
    public static int YearsToDays(int years)
    {
        return years* 4 * 30;
    }
    public static int CompareTimeStamp(GameTimeStamp gameTimeStampX, GameTimeStamp gameTimeStampY, bool toHour)
    {
        int hourX = DaysToHours(YearsToDays(gameTimeStampX.year) + SeasonsToDays(gameTimeStampX.season) + gameTimeStampX.day) + gameTimeStampX.hour;
        int hourY = DaysToHours(YearsToDays(gameTimeStampY.year) + SeasonsToDays(gameTimeStampY.season) + gameTimeStampY.day) + gameTimeStampY.hour;
        if (toHour)
        {
            return Mathf.Abs(hourY - hourX);
        }
        else
        {
            return Mathf.Abs((HoursToMinutes(hourX) + gameTimeStampX.minute) - (HoursToMinutes(hourY) + gameTimeStampY.minute));

        }
    }
}
