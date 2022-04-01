using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MedicationEditor : MonoBehaviour
{
    public GameObject menu;
    public InputField medName;
    public InputField medHour;
    public InputField medMins;
    public GameObject amToggle;
    public GameObject pmToggle;
    public GameObject[] starOns;
    public Button[] shapes;
    public GameObject main;
    public Toggle[] weekdays;
    public bool ifPM;   // true if PM, false if AM

    public MedicationController controller;
    private Medication medication;

    // defaults to index = -1
    public void Open(){

        Open(-1);

    }

    // if index == -1, a new medication will be created
    // otherwise, the medication at the index will be accessed
    public void Open(int id){

        main.SetActive(false);
        menu.SetActive(true);
        
        // create new medication or get existing medication
        medication = id == -1 ? new Medication() : controller.GetMedication(id);

        // set if time is AM or PM
        ifPM = medication.NotifyTime.Hours > 11;

        // refresh GUI
        Refresh();
        

    }

    public void Refresh(){

        // set name
        medName.text = medication.Name;

        // get time (and convert from 24-hour clock to AM/PM)
        int hour = medication.NotifyTime.Hours;
        bool pm = hour > 11;
        if(pm) hour -= 12;
        if(hour == 0) hour = 12;
        int minutes = medication.NotifyTime.Minutes;

        // set label, accounting for 0 digit in front if single digit value
        medHour.text = hour < 10 ? "0" +  hour : hour + "";
        medMins.text = minutes < 10 ? "0" +  minutes : minutes + "";

        // set stars active
        for(int s = 0; s < 3; s++){
            starOns[s].SetActive(s < medication.Stars);
        }

        // set weekdays
        for(int d = 0; d < 7; d++){
            weekdays[d].isOn = medication.Weekdays[d];
        }

        for(int s = 0; s < shapes.Length; s++){
            shapes[s].interactable = medication.Shape != s;
            shapes[s].GetComponent<Image>().color = medication.Shape == s 
                ? PillColors.GetColor(medication.Color)
                : new Color(0, 0, 0, 0.5F);
        }

        // set initial AM/PM toggle
        amToggle.SetActive(!pm);
        pmToggle.SetActive(pm);

    }

    // update our medication object with new data
    public void UpdateInfo(){

        int hour = int.Parse(medHour.text); // get hour
        int min = int.Parse(medMins.text);  // get minute

        // reset hour if incorrect input
        if(hour < 1 || hour > 12){
            hour = medication.NotifyTime.Hours;
        }

        // reset minute if incorrect input
        if(min < 0 || min > 59){
            min = medication.NotifyTime.Minutes;
        }

        // we have to convert from regular AM/PM time to 24 hour time

        // first, if time is PM, add 12 to the time indicated
        if(ifPM && hour < 12)
            hour += 12;
        // 12 AM should become 0
        if(!ifPM && hour == 12)
            hour = 0;

        // get weekdays directly from toggles
        for(int d = 0; d < 7; d++){
            medication.Weekdays[d] = weekdays[d].isOn;
        }

        TimeSpan newNotify = new TimeSpan(hour, min, 0);
        medication.NotifyTime = newNotify;
        medication.Name = medName.text;
        Refresh();

    }

    // toggle time as being AM or PM
    public void ToggleAMPM(bool pm){
        ifPM = pm;
        UpdateInfo();
    }

    // close editor menu
    private void Close(){

        controller.Refresh();
        menu.SetActive(false);      // set our own menu inactive
        main.SetActive(true);       // set main menu active again

    }

    // save medication in database
    public void SaveMedication(){

        // if new medication, save as new object
        if(medication.ID == -1)
            controller.AddMedication(medication);

        // otherwise, update existing record
        else
            controller.EditMedication(medication);

        // close app
        Close();

    }

    // delete medication
    public void DeleteMedication(){

        // if medication is one that already exists, delete from database
        if(medication.ID != -1)
            controller.DeleteMedication(medication);

        // close menu
        Close();

    }

    // change medication priority
    public void ChangeStars(int stars){

        medication.Stars = stars;
        UpdateInfo();

    }

    // change medication shape
    public void ChangeShape(int shape){

        medication.Shape = shape;
        UpdateInfo();

    }

    // change medication color
    public void ChangeColor(int color){

        medication.Color = color;
        UpdateInfo();

    }

}
