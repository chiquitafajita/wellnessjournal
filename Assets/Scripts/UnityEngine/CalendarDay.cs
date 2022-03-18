using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CalendarDay : MonoBehaviour
{

    public Image rating;
    public Button dayButton;
    public Image todayIndicator;
    public Text dateLabel;
    
    private DateTime date;

    private DBController database;

    public void Refresh(DBController database, DateTime date, bool isToday, bool exists, int mode){

        this.database = database;
        this.date = date;

        dateLabel.text = date.Day + "";
        dayButton.interactable = exists;
        if(exists){
            
            switch(mode){
                case 1:
                    dateLabel.text = database.GetDayRating(date) + "";
                    break;
                case 2:
                    dateLabel.text = database.GetDayGrade(date) + "";
                    break;
                default:
                    dateLabel.text = date.Day + "";
                    break;
            }
            
            // rating.enabled = true;
            // switch(database.GetDayRating(date)){
            //     case -2:
            //         rating.color = Color.red;
            //         break;
                    
            //     case -1:
            //         rating.color = new Color(1, .646F, 0);
            //         break;
                    
            //     case 0:
            //         rating.color = Color.yellow;
            //         break;
                    
            //     case 1:
            //         rating.color = Color.green;
            //         break;
                    
            //     case 2:
            //         rating.color = Color.blue;
            //         break;
                    
            // }

        }
        else{
            //rating.enabled = false;
        }

        todayIndicator.gameObject.SetActive(isToday);

    }

}
