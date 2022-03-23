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

    public Button prevDay;
    public Button nextDay;
    public Text dayLabel;

    public GameObject menu;
    public GameObject main;

    public Transform contentGrid;
    
    private DateTime date;

    public GameObject itemTemplate;
    private ObjectPool pool;
    
    public void ViewDay(DateTime date){

        // if pool is null, create
        if(pool == null)
            pool = new ObjectPool(itemTemplate);

        // set date and refresh persistent UI objects
        this.date = date;
        pastRating.Refresh(date);
        pastTags.Refresh(date);

        // display date
        dayLabel.text = TimeKeeper.GetMonth(date.Month) + " " + date.Day;

        // only enable "prev day" button if this is not the first day
        DateTime earliest = database.GetEarliestDate();
        prevDay.interactable = date > earliest;

        // only enable "next day" button if this is not the last day
        DateTime today = TimeKeeper.GetDate();
        nextDay.interactable = date < today;

        main.SetActive(false);
        menu.SetActive(true);

        List<Medication> meds = database.GetDosesForDay(date, true);
        pool.Clear();
        for(int i = 0; i < meds.Count; i++){

            GameObject go = pool.CreateNew();
            go.transform.SetParent(contentGrid);
            go.transform.SetAsLastSibling();
            go.transform.localScale = Vector3.one;

            MedicationPastItem mi = go.GetComponent<MedicationPastItem>();
            mi.Refresh(database, meds[i], date);

        }

    }

    public void LastDay(){

        ViewDay(date.AddDays(-1));

    }

    public void NextDay(){

        ViewDay(date.AddDays(1));

    }

    public void Close(){

        main.SetActive(true);
        menu.SetActive(false);
        controller.Refresh();

    }

}
