using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayRater : MonoBehaviour
{
    
    public MedicationController controller;
    public DBController database;
    public GameObject[] starOns;

    public void Refresh(){

        int rating = database.GetDayRating(TimeKeeper.GetDate());
        for(int s = 0; s < 5; s++){
            starOns[s].SetActive(s < rating);
        }

    }

    public void ChangeRating(int rating){

        database.UpdateDayRating(TimeKeeper.GetDate(), rating);
        controller.Refresh();

    }

}
