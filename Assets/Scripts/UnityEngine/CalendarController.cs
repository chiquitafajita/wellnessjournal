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
    public PreviousDayView previousDayView;
    public Transform calendarGrid;
    public Text monthLabel;
    public Text weekGrade;
    public Text monthGrade;
    public Text weekRating;
    public Text monthRating;

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

            go = blanks.CreateNew();
            go.transform.SetParent(calendarGrid);
            go.transform.SetAsLastSibling();
            go.SetActive(true);

        }

        // add days that are actually in the month
        for(int i = 0; i < DateTime.DaysInMonth(year, month); i++){

            current = new DateTime(year, month, i+1);

            go = days.CreateNew();
            go.transform.SetParent(calendarGrid);
            go.transform.SetAsLastSibling();
            CalendarDay cd = go.GetComponent<CalendarDay>();
            cd.Refresh(this, current, GetDayLabel(current), database.IsDayRecorded(current));

        }

        DateTime earliest = database.GetEarliestDate();
        lastMonth.interactable = includedDate.Month > earliest.Month || includedDate.Year > earliest.Year;

        DateTime today = TimeKeeper.GetDate();
        nextMonth.interactable = includedDate.Month < today.Month || includedDate.Year < today.Year;

        weekGrade.text = "Week Grade :  " + database.GetWeekGrade(today);
        monthGrade.text = "Month Grade :  " + database.GetMonthGrade(today);
        weekRating.text = "Week Rating :  " + Math.Round(database.GetWeekRating(today), 2) + " / 5";
        monthRating.text = "Month Rating :  " + Math.Round(database.GetMonthRating(today), 2) + " / 5";

    }

    public void GoToLastMonth(){

        Refresh(new DateTime(current.Year, current.Month, 1).AddMonths(-1));

    }

    public void GoToNextMonth(){

        Refresh(new DateTime(current.Year, current.Month, 1).AddMonths(1));

    }

    public void SetMode(int mode){

        this.mode = mode;
        Refresh(current);

    }

    public string GetDayLabel(DateTime date){

        // default is day of month
        string output = date.Day + "";

        // if day exists in database
        if(database.IsDayRecorded(current)){
            
            switch(mode){

                // get mood/vibes/etc rating
                case 1:
                    output = database.GetDayRating(date) + "";
                    break;

                // get dose consistency grade
                case 2:
                    output = database.GetDayGrade(date) + "";
                    break;
            }
            
        }

        return output;

    }

    public void ViewPreviousDay(DateTime date){

        previousDayView.ViewDay(date);

    }

}
