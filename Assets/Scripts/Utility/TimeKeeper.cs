using System.Collections;
using System.Collections.Generic;
using System;

public static class TimeKeeper {

    private static string[] months = {"January", "February", "March", "April", "May", "June",
                                        "July", "August", "September, October, November, December"};

    public static int GetDay() { return DateTime.Now.Day; }

    public static int GetDayOfWeek() { return (int)DateTime.Now.DayOfWeek; }

    public static string GetMonth(){ return months[DateTime.Now.Month - 1]; }

    public static int GetYear() { return DateTime.Now.Year; }

    public static TimeSpan GetTime() { return DateTime.Now.TimeOfDay; }

    public static DateTime GetDateTime() { return DateTime.Now; }

}
