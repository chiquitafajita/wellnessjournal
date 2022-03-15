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

    // Start is called before the first frame update
    void Awake() {

        days = new ObjectPool(dayTemplate);
        blanks = new ObjectPool(blankTemplate);
        RefreshCalendar(TimeKeeper.GetDate());
        
    }

    public void RefreshCalendar(DateTime includedDate){

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
            cd.Refresh(database, current, current == TimeKeeper.GetDate(), database.IsDayRecorded(current));

        }

        DateTime earliest = database.GetEarliestDate();
        lastMonth.interactable = includedDate.Month > earliest.Month || includedDate.Year > earliest.Year;

        DateTime today = TimeKeeper.GetDate();
        nextMonth.interactable = includedDate.Month < today.Month || includedDate.Year < today.Year;

    }

    public void GoToLastMonth(){

        DateTime last = new DateTime(current.Year, current.Month, 1).AddMonths(-1);
        RefreshCalendar(last);

    }

    public void GoToNextMonth(){

        DateTime next = new DateTime(current.Year, current.Month, 1).AddMonths(1);
        RefreshCalendar(next);

    }

}
