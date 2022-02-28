using System.Collections;
using System.Collections.Generic;
using System;

public class Medication : IComparable<Medication>
{
    
    public int ID { get; set; }
    public string Name { get; set; }
    public TimeSpan NotifyTime { get; set; }
    public TimeSpan Window { get; set; }
    public bool Taken { get; set; }
    public DateTime LastTaken { get; set; }
    public int Stars { get; set; }
    public bool[] Weekdays { get; set; }
    public int Color { get; set; }
    public int Shape { get; set; }
    public bool Active { get; set; }

    public Medication(){
        ID = -1;
        Name = "Medication";
        NotifyTime = new TimeSpan(TimeKeeper.GetTime().Hours, TimeKeeper.GetTime().Minutes, 0);
        Window = new TimeSpan(1, 0, 0);
        Taken = false;
        Stars = 1;
        Weekdays = new bool[]{true, true, true, true, true, true, true};
        Color = 0;
        Shape = 0;
        Active = true;
    }

    // returns 0 if early
    // returns 1 if within window
    // returns 2 if after window
    // returns 3 if taken
    public int GetStatus(){

        // if medication has been taken today
        if(Taken)
            return 3;

        // if current time is less than notify time
        if(TimeKeeper.GetTime() < NotifyTime)
            return 0;

        // if we are still within ideal window
        else if(TimeKeeper.GetTime() < NotifyTime + Window)
            return 1;

        // if we are past the ideal window
        return 2;

    }

    // returns difference between notify time and current time
    // if medication is late, the difference is negative
    public TimeSpan GetTimeUntil(){

        return NotifyTime - TimeKeeper.GetTime();

    }

    public Medication Take(){

        Taken = true;
        LastTaken = TimeKeeper.GetDateTime();
        return this;

    }

    public Medication Refresh(){

        if(GetDaysSinceLastTaken() > 0){
            Taken = false;
        }

        return this;

    }

    public int GetDaysSinceLastTaken(){

        return (TimeKeeper.GetDateTime() - LastTaken).Days;

    }

    // public int GetDosesMissed(){



    // }

    public int CompareTo(Medication other){

        int comp = (int)(NotifyTime - other.NotifyTime).TotalMinutes;
        if(comp == 0) comp = Stars - other.Stars;
        if(comp == 0) comp = Name.CompareTo(other.Name);
        return comp;

    }

    public string GetSqlColumns(){

        string values = "(";
        values += "name,";
        values += "time,";
        values += "sun,";
        values += "mon,";
        values += "tue,";
        values += "wed,";
        values += "thu,";
        values += "fri,";
        values += "sat,";
        values += "active,";
        values += "stars,";
        values += "color,";
        values += "shape)";


        return values;
        
    }

    public string GetSqlValues(){

        string values = "('";
        values += Name + "',";
        values += NotifyTime.Ticks + ",";
        values += GetBit(Weekdays[0]) + ",";
        values += GetBit(Weekdays[1]) + ",";
        values += GetBit(Weekdays[2]) + ",";
        values += GetBit(Weekdays[3]) + ",";
        values += GetBit(Weekdays[4]) + ",";
        values += GetBit(Weekdays[5]) + ",";
        values += GetBit(Weekdays[6]) + ",";
        values += GetBit(Active) + ",";
        values += Stars + ",";
        values += Color + ",";
        values += Shape + ")";


        return values;
        
    }

    private int GetBit(bool boolean){
        return boolean ? 1 : 0;
    }

}
