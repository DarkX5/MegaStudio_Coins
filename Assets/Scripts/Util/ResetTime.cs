using System;
using UnityEngine;

[Serializable]
public class ResetTime
{
    public ResetTime()
    {
        hours = minutes = seconds = 0;
    }
    public ResetTime(int hours, int minutes, int seconds)
    {
        this.hours = hours;
        this.minutes = minutes;
        this.seconds = seconds;
    }
    
    [SerializeField] private int hours;
    [SerializeField] private int minutes;
    [SerializeField] private int seconds;
    public int Hours { get { return hours; } }
    public int Minutes { get { return minutes; } }
    public int Seconds { get { return seconds; } }
}
