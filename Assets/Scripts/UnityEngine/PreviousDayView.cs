using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PreviousDayView : MonoBehaviour
{

    public MedicationController controller;
    public DBController database;
    public TagWriter pastTags;
    public DayRater pastRating;

    public Button lastDay;
    public Button nextDay;
    public Text dayLabel;

    public GameObject calendar;
    public GameObject menu;
    public GameObject tabs;

    public Transform contentGrid;
    
    private DateTime date;

    public GameObject itemTemplate;
    private ObjectPool pool;
    
    public void ViewDay(DateTime date){

        if(pool == null)
            pool = new ObjectPool(itemTemplate);

        this.date = date;
        pastRating.Refresh(date);
        pastTags.Refresh(date);

        dayLabel.text = TimeKeeper.GetMonth(date.Month) + " " + date.Day;

        DateTime earliest = database.GetEarliestDate();
        lastDay.interactable = date > earliest;

        DateTime today = TimeKeeper.GetDate();
        nextDay.interactable = date < today;

        calendar.SetActive(false);
        tabs.SetActive(false);
        menu.SetActive(true);

        List<Medication> meds = database.GetDosesForDay(date, true);
        pool.Clear();
        for(int i = 0; i < meds.Count; i++){

            GameObject go = pool.GetObject();
            go.transform.SetParent(contentGrid);
            go.transform.SetAsLastSibling();
            go.transform.localScale = Vector3.one;

            MedicationPastItem mi = go.GetComponent<MedicationPastItem>();
            mi.Refresh(database, meds[i], date);

        }

    }

    public void LastDay(){

        Debug.Log("Click");
        ViewDay(date.AddDays(-1));

    }

    public void NextDay(){

        Debug.Log("Click");
        ViewDay(date.AddDays(1));

    }

    public void Close(){

        calendar.SetActive(true);
        tabs.SetActive(true);
        menu.SetActive(false);
        controller.Refresh();

    }

}
