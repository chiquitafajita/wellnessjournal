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
    public RectTransform calendarItem;
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

    // refresh calendar assuming same date
    public void Refresh(){

        Refresh(current);

    }

    // refresh calendar view with given date
    public void Refresh(DateTime includedDate){

        // get year and month
        int year = includedDate.Year;
        int month = includedDate.Month;

        // set month label, incl. year
        monthLabel.text = TimeKeeper.GetMonth(month) + " " + year;

        // clear object pools
        days.Clear();
        blanks.Clear();

        
        // add blanks in first week of the month
        current = new DateTime(year, month, 1);
        GameObject go;
        for(int d = 0; d < (int)current.DayOfWeek; d++){

            // create blank object
            go = blanks.CreateNew();
            go.transform.SetParent(calendarGrid);
            go.transform.SetAsLastSibling();
            go.SetActive(true);

        }

        // add days that are actually in the month
        for(int i = 0; i < DateTime.DaysInMonth(year, month); i++){

            // set current date
            current = new DateTime(year, month, i+1);

            // create day object
            go = days.CreateNew();
            go.transform.SetParent(calendarGrid);
            go.transform.SetAsLastSibling();

            // refresh object with data about the day
            CalendarDay cd = go.GetComponent<CalendarDay>();
            cd.Refresh(this, current, GetDayLabel(current, mode), database.IsDayRecorded(current));

        }

        // change height of calendar box to include more days
        int count = blanks.Count + days.Count;
        if(count > 35){
            calendarItem.sizeDelta = new Vector2(216, 210);
        }
        else if(count > 28){
            calendarItem.sizeDelta = new Vector2(216, 180);
        }
        else{
            calendarItem.sizeDelta = new Vector2(216, 150);
        }

        // only enable "prev month" button if this is not the earliest month
        DateTime earliest = database.GetEarliestDate();
        lastMonth.interactable = includedDate.Month > earliest.Month || includedDate.Year > earliest.Year;

        // only enable "next month" button if this is not the most recent month
        DateTime today = TimeKeeper.GetDate();
        nextMonth.interactable = includedDate.Month < today.Month || includedDate.Year < today.Year;

        // set labels
        weekGrade.text = "  Week Grade :  " + database.GetWeekGrade(today);
        weekRating.text = "  Week Rating :  " + Math.Round(database.GetWeekRating(today), 2) + " / 5";
        monthGrade.text = "  Month Grade :  " + database.GetMonthGrade(today);
        monthRating.text = "  Month Rating :  " + Math.Round(database.GetMonthRating(today), 2) + " / 5";

    }

    // go to previous month (current month - 1)
    public void GoToLastMonth(){

        Refresh(new DateTime(current.Year, current.Month, 1).AddMonths(-1));

    }

    // go to next month (current month + 1)
    public void GoToNextMonth(){

        Refresh(new DateTime(current.Year, current.Month, 1).AddMonths(1));

    }

    // set mode (0 = dates, 1 = ratings, 2 = grades)
    public void SetMode(int mode){

        this.mode = mode;
        Refresh(current);

    }

    // get label for a calendar day given mode variable
    public string GetDayLabel(DateTime date, int mode){

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

    // click calendar day to view complete record
    public void ViewPreviousDay(DateTime date){

        previousDayView.ViewDay(date);

    }

}
