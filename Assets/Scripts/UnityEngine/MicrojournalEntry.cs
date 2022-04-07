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

        this.date = date;
        this.controller = controller;

        content.text = entry;
        dateLabel.text = TimeKeeper.GetMonth(date.Month) + " " + date.Day + ", " + date.Year;
        rating.text = stars + "";

    }

    public void ClickSelf(){

        controller.ViewPreviousDay(date);

    }
}
