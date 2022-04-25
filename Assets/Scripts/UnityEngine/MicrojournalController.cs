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

        // if pool DNE, create new pool
        if(items == null)
            items = new ObjectPool(itemTemplate);

        // clear pool of items
        items.Clear();

        // for each date within the last 31 days
        for(DateTime date = TimeKeeper.GetDate(); date >= database.GetEarliestDate() && date >= TimeKeeper.GetDate().AddDays(-31); date = date.AddDays(-1)){
            
            // get microjournal entry from database
            string entry = database.GetDayTags(date);

            // if entry is not empty or null, create microjournal object to display it
            if(!string.IsNullOrEmpty(entry)){

                // create from pool, put in content grid
                GameObject go = items.CreateNew();
                go.transform.SetParent(contentWindow);
                go.transform.SetAsLastSibling();

                // refresh object with information about this entry (date, entry, rating)
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
