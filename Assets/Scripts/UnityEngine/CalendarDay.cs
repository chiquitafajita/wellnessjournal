using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CalendarDay : MonoBehaviour
{

    public Image rating;
    public Button dayButton;
    public Image todayIndicator;
    public Text dateLabel;
    
    private DateTime date;

    private CalendarController calendar;

    public void Refresh(CalendarController calendar, DateTime date, String label, bool exists){

        this.calendar = calendar;
        this.date = date;

        dayButton.interactable = exists;
        dateLabel.text = label;

        todayIndicator.gameObject.SetActive(date == TimeKeeper.GetDate());

    }

    public void ClickSelf(){

        calendar.ViewPreviousDay(date);

    }

}
