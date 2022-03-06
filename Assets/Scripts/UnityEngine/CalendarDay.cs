using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CalendarDay : MonoBehaviour
{

    public Button dayButton;
    public Image square;
    public Image todayIndicator;
    
    private DateTime date;

    private DBController database;

    public void Refresh(DBController database, DateTime date, bool isToday, bool isBeforeThisWeek){

        this.database = database;
        this.date = date;


        dayButton.interactable = database.IsDayRecorded(date);
        square.color = isBeforeThisWeek ? new Color(1, 1, 1, 0.5F) : Color.white;
        Debug.Log(square.color);
        todayIndicator.gameObject.SetActive(isToday);

    }

}
