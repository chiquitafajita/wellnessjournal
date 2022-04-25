using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MicrojournalEntry : MonoBehaviour
{

    public Text content;
    public Text dateLabel;
    public Text rating;

    private DateTime date;
    private MicrojournalController controller;

    public void Refresh(MicrojournalController controller, DateTime date, int stars, String entry){

        // set date and controller
        this.date = date;
        this.controller = controller;

        // set entry text, date, and day rating
        content.text = entry;
        dateLabel.text = TimeKeeper.GetMonth(date.Month) + " " + date.Day + ", " + date.Year;
        rating.text = stars + "";

    }

    // when clicking this entry, view the record of that day
    public void ClickSelf(){

        controller.ViewPreviousDay(date);

    }
}
