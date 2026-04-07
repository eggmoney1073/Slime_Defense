using UnityEngine;

public class TimeCalculater
{
    public static string Calculate_MM_SS_MS(float time)
    {
        int minutes = (int)Mathf.Floor(time / 60f);
        string minutesString = minutes < 10 ? "0" + minutes.ToString() : minutes.ToString();

        int seconds = (int)Mathf.Floor(time % 60f);
        string secondsString = seconds < 10 ? "0" + seconds.ToString() : seconds.ToString();

        int milliseconds = (int)((time - Mathf.Floor(time)) * 1000f);
        string millisecondsString = milliseconds < 100 ? (milliseconds < 10 ? "00" + milliseconds.ToString() : "0" + milliseconds.ToString()) : milliseconds.ToString();

        return minutesString + " : " + secondsString + " : " + millisecondsString;
    }

    public static string Calculate_MM_SS(float time)
    {
        int minutes = (int)Mathf.Floor(time / 60f);
        string minutesString = minutes < 10 ? "0" + minutes.ToString() : minutes.ToString();

        int seconds = (int)Mathf.Floor(time % 60f);
        string secondsString = seconds < 10 ? "0" + seconds.ToString() : seconds.ToString();

        return minutesString + " : " + secondsString;
    }
}
