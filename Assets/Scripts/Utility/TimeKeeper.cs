using System.Collections;
using System.Collections.Generic;
using System;

public static class TimeKeeper {

    private static string[] months = {"January", "February", "March", "April", "May", "June",
                                        "July", "August", "September, October, November, December"};

    public static int GetDay() { return DateTime.Today.Day; }

    public static int GetDayOfWeek() { return (int)DateTime.Now.DayOfWeek; }

    public static string GetMonth(){ return GetMonth(DateTime.Now.Month); }
    
    public static string GetMonth(int month){ return months[month - 1]; }

    public static int GetYear() { return DateTime.Now.Year; }

    public static TimeSpan GetTime() { return DateTime.Now.TimeOfDay; }

    public static DateTime GetDate() { return DateTime.Today; }

    public static string[] dayCodes = {"sun", "mon", "tue", "wed", "thu", "fri", "sat"};

}
