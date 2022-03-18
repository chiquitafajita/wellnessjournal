using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CalendarController : MonoBehaviour
{

    public Button lastMonth;
    public Button nextMonth;
    public DBController database;
    public GameObject dayTemplate;
    public GameObject blankTemplate;
    public Transform calendarGrid;
    public Text monthLabel;

    private DateTime current;

    private ObjectPool blanks;
    private ObjectPool days;

    private int mode;   // 0 = dates, 1 = moods, 2 = scores

    // Start is called before the first frame update
    public void Setup() {

        days = new ObjectPool(dayTemplate);
        blanks = new ObjectPool(blankTemplate);
        Refresh(TimeKeeper.GetDate());
        
    }

    public void Refresh(){

        Refresh(current);

    }

    public void Refresh(DateTime includedDate){

        int year = includedDate.Year;
        int month = includedDate.Month;

        monthLabel.text = TimeKeeper.GetMonth(month) + " " + year;

        days.Clear();
        blanks.Clear();

        GameObject go;
        
        // add blanks in first week of the month
        current = new DateTime(year, month, 1);
        for(int d = 0; d < (int)current.DayOfWeek; d++){

            go = blanks.GetObject();
            go.transform.SetParent(calendarGrid);
            go.transform.SetAsLastSibling();
            go.SetActive(true);

        }

        // add days that are actually in the month
        for(int i = 0; i < DateTime.DaysInMonth(year, month); i++){

            current = new DateTime(year, month, i+1);

            go = days.GetObject();
            go.transform.SetParent(calendarGrid);
            go.transform.SetAsLastSibling();
            CalendarDay cd = go.GetComponent<CalendarDay>();
            cd.Refresh(database, current, current == TimeKeeper.GetDate(), database.IsDayRecorded(current), mode);

        }

        DateTime earliest = database.GetEarliestDate();
        lastMonth.interactable = includedDate.Month > earliest.Month || includedDate.Year > earliest.Year;

        DateTime today = TimeKeeper.GetDate();
        nextMonth.interactable = includedDate.Month < today.Month || includedDate.Year < today.Year;

    }

    public void GoToLastMonth(){

        DateTime last = new DateTime(current.Year, current.Month, 1).AddMonths(-1);
        Refresh(last);

    }

    public void GoToNextMonth(){

        DateTime next = new DateTime(current.Year, current.Month, 1).AddMonths(1);
        Refresh(next);

    }

    public void SetMode(int mode){

        this.mode = mode;
        Refresh(current);

    }

}
