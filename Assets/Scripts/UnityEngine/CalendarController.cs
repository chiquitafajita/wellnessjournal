using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CalendarController : MonoBehaviour
{

    public DBController database;
    public GameObject dayTemplate;
    public GameObject blankTemplate;
    public Transform calendarGrid;
    public Text monthLabel;

    private List<GameObject> blanks;
    private List<GameObject> days;

    // Start is called before the first frame update
    void Start() {

        days = new List<GameObject>();
        blanks = new List<GameObject>();
        RefreshCalendar(TimeKeeper.GetDate());
        
    }

    public void RefreshCalendar(){

        // set all existing day buttons inactive
        for(int i = 0; i < days.Count; i++){
            days[i].SetActive(false);
        }

        DateTime today = TimeKeeper.GetDate();
        int daysToLoad = 28 + TimeKeeper.GetDayOfWeek();

        for(int i = 0; i <= daysToLoad; i++){

            if(i >= days.Count){
                days.Add(GameObject.Instantiate(dayTemplate));
                days[i].transform.SetParent(calendarGrid);
            }

            days[i].SetActive(true);
            CalendarDay cd = days[i].GetComponent<CalendarDay>();
            cd.Refresh(database, today.AddDays(i  - daysToLoad), i == daysToLoad, i < 28);

        }

    }

    public void RefreshCalendar(DateTime includedDate){

        int year = includedDate.Year;
        int month = includedDate.Month;

        monthLabel.text = TimeKeeper.GetMonth(month) + " " + year;
        
        // add blanks in first week of the month
        DateTime current = new DateTime(year, month, 1);
        for(int d = 0; d < (int)current.DayOfWeek; d++){

            if(d >= blanks.Count){
                blanks.Add(GameObject.Instantiate(blankTemplate));
                blanks[d].transform.SetParent(calendarGrid);
            }
            
            blanks[d].transform.SetAsLastSibling();
            blanks[d].SetActive(true);

        }

        // add days that are actually in the month
        for(int i = 0; i < DateTime.DaysInMonth(year, month); i++){

            current = new DateTime(year, month, i+1);

            if(i >= days.Count){
                days.Add(GameObject.Instantiate(dayTemplate));
                days[i].transform.SetParent(calendarGrid);
            }

            days[i].SetActive(true);
            days[i].transform.SetAsLastSibling();
            CalendarDay cd = days[i].GetComponent<CalendarDay>();
            cd.Refresh(database, current, i+1 == TimeKeeper.GetDate().Day, true);

        }

    }

}
