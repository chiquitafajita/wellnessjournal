using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MicrojournalController : MonoBehaviour
{
    public DBController database;
    public PreviousDayView previousDayView;
    public GameObject itemTemplate;
    public Transform contentWindow;

    private ObjectPool items;

    public void Refresh(){

        if(items == null)
            items = new ObjectPool(itemTemplate);

        items.Clear();

        for(DateTime date = TimeKeeper.GetDate(); date >= database.GetEarliestDate() && date >= TimeKeeper.GetDate().AddDays(-31); date = date.AddDays(-1)){
            string entry = database.GetDayTags(date);
            if(!string.IsNullOrEmpty(entry)){
                GameObject go = items.CreateNew();
                go.transform.SetParent(contentWindow);
                go.transform.SetAsLastSibling();

                MicrojournalEntry mje = go.GetComponent<MicrojournalEntry>();
                mje.Refresh(this, date, database.GetDayRating(date), entry);
            }
        }

    }

    // click calendar day to view complete record
    public void ViewPreviousDay(DateTime date){

        previousDayView.ViewDay(date);

    }

}
