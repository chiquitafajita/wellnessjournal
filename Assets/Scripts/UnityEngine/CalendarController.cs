using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CalendarController : MonoBehaviour
{

    public DBController database;
    public GameObject dayTemplate;
    public Transform calendarGrid;

    private List<GameObject> days;

    // Start is called before the first frame update
    void Start() {

        days = new List<GameObject>();
        RefreshCalendar();
        
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

}
