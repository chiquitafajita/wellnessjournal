using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DayRater : MonoBehaviour
{
    
    public MedicationController controller;
    public DBController database;
    public GameObject[] starOns;

    private DateTime date;

    // refresh GUI with given date
    public void Refresh(DateTime date){

        this.date = date;
        RefreshAppearance(database.GetDayRating(date));

    }

    // refresh rating display by setting star objects active or inactive
    private void RefreshAppearance(int rating){

        for(int s = 0; s < 5; s++){
            starOns[s].SetActive(s < rating);
        }

    }

    // change rating of day (called by GUI)
    public void ChangeRating(int rating){

        RefreshAppearance(rating);
        database.UpdateDayRating(date, rating);
        //controller.Refresh();

    }

}
