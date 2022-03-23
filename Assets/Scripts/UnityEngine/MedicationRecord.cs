using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MedicationRecord : MonoBehaviour
{
    public Text nameLabel;
    public Text timeLabel;
    public Toggle[] stars;
    public Text[] weekdays;
    private int id;

    private MedicationList list;

    public void Refresh(MedicationList list, Medication med){

        // set list and ID
        this.list = list;
        id = med.ID;

        // update labels for name and if med was taken
        nameLabel.text = med.Name;
        TimeSpan time = med.NotifyTime;
        int hour = time.Hours;
        int min = time.Minutes;

        // if PM
        if(hour > 11){

            if(hour > 12)
                hour -= 12;
            timeLabel.text = hour < 10 ? "0" + hour + ":" : hour + ":";
            timeLabel.text += min < 10 ? "0" + min : min + "";
            timeLabel.text += " PM";

        }

        // if AM
        else{
            if(hour == 0)
                hour = 12;
            timeLabel.text = hour < 10 ? "0" + hour + ":" : hour + ":";
            timeLabel.text += min < 10 ? "0" + min : min + "";
            timeLabel.text += " AM";
        }

        // refresh stars
        for(int s = 0; s < 3; s++){
            stars[s].isOn = s < med.Stars;
            stars[s].interactable = false;
        }

        // refresh weekdays
        for(int d = 0; d < 7; d++){
            weekdays[d].color = med.Weekdays[d] ? Color.white : new Color(0, 0, 0, 0.75F);
        }

    }

    // when object is clicked, open menu to edit
    public void ClickSelf(){

        list.EditMedication(id);

    }

}
