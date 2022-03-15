using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
using UnityEngine;

public class Medication : IComparable<Medication>
{
    
    public int ID { get; set; }
    public string Name { get; set; }
    public TimeSpan NotifyTime { get; set; }
    public bool Taken { get; set; }
    public int Stars { get; set; }
    public bool[] Weekdays { get; set; }
    public int Color { get; set; }
    public int Shape { get; set; }
    public bool Active { get; set; }
    public int Status { get; set; }

    public Medication(){
        ID = -1;
        Name = "Medication";
        NotifyTime = new TimeSpan(TimeKeeper.GetTime().Hours, TimeKeeper.GetTime().Minutes, 0);
        Taken = false;
        Stars = 1;
        Weekdays = new bool[]{true, true, true, true, true, true, true};
        Color = 0;
        Shape = 0;
        Active = true;
    }

    public Medication(IDataReader reader){
        ID = int.Parse(reader[0].ToString());
        Name = reader[1].ToString();
        NotifyTime = new TimeSpan(long.Parse(reader[2].ToString()));
        Weekdays = new bool[7];
        Weekdays[0] = bool.Parse(reader[3].ToString());
        Weekdays[1] = bool.Parse(reader[4].ToString());
        Weekdays[2] = bool.Parse(reader[5].ToString());
        Weekdays[3] = bool.Parse(reader[6].ToString());
        Weekdays[4] = bool.Parse(reader[7].ToString());
        Weekdays[5] = bool.Parse(reader[8].ToString());
        Weekdays[6] = bool.Parse(reader[9].ToString());
        Active = bool.Parse(reader[10].ToString());
        Stars = int.Parse(reader[11].ToString());
        Color = int.Parse(reader[12].ToString());
        Shape = int.Parse(reader[13].ToString());

    }

    // returns 0 if early
    // returns 1 if within window
    // returns 2 if after window
    public int GetTimePosition(){

        // if current time is less than notify time
        if(TimeKeeper.GetTime() < NotifyTime)
            return 0;

        // if we are still within ideal window
        else if(TimeKeeper.GetTime() < NotifyTime.Add(new TimeSpan(1, 0, 0)))
            return 1;


        // if we are past the ideal window
        return 2;

    }

    // returns difference between notify time and current time
    // if medication is late, the difference is negative
    public TimeSpan GetTimeUntil(){

        return NotifyTime - TimeKeeper.GetTime();

    }

    public Medication Refresh(){

        if(GetDaysSinceLastTaken() > 0){
            Taken = false;
        }

        return this;

    }

    public int GetDaysSinceLastTaken(){

        return 0;

    }

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

    public string GetUpdate(){

        string values = "";
        values += "name='" + Name + "',";
        values += "time=" + NotifyTime.Ticks + ",";
        values += "sun=" + GetBit(Weekdays[0]) + ",";
        values += "mon=" + GetBit(Weekdays[1]) + ",";
        values += "tue=" + GetBit(Weekdays[2]) + ",";
        values += "wed=" + GetBit(Weekdays[3]) + ",";
        values += "thu=" + GetBit(Weekdays[4]) + ",";
        values += "fri=" + GetBit(Weekdays[5]) + ",";
        values += "sat=" + GetBit(Weekdays[6]) + ",";
        values += "active=" + GetBit(Active) + ",";
        values += "stars=" + Stars + ",";
        values += "color=" + Color + ",";
        values += "shape=" + Shape;

        return values;

    }

}
