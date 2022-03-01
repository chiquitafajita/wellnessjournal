using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MedicationEditor : MonoBehaviour
{

    public InputField medName;
    public InputField medHour;
    public InputField medMins;
    public GameObject amToggle;
    public GameObject pmToggle;
    public GameObject[] starOns;
    public Toggle[] weekdays;
    public bool ifPM;   // true if PM, false if AM

    public MedicationController controller;
    private Medication medication;
    private int medIndex = -1;  // equals -1 if creating a new medication

    // if index == -1, a new medication will be created
    // otherwise, the medication at the index will be accessed
    public void EditMedication(int index){

        // set our index to the method being called
        medIndex = index;

        // create new medication or get existing medication
        medication = medIndex == -1 ? new Medication() : controller.GetMedication(medIndex);

        // set if time is AM or PM
        ifPM = medication.NotifyTime.Hours > 11;

        // refresh GUI
        RefreshGUI();

    }

    public void RefreshGUI(){

        medName.text = medication.Name;
        int hour = medication.NotifyTime.Hours;
        bool pm = hour > 11;
        if(pm) hour -= 12;
        if(hour == 0) hour = 12;
        int minutes = medication.NotifyTime.Minutes;

        medHour.text = hour < 10 ? "0" +  hour : hour + "";
        medMins.text = minutes < 10 ? "0" +  minutes : minutes + "";

        for(int s = 0; s < 3; s++){
            starOns[s].SetActive(s < medication.Stars);
        }

        for(int d = 0; d < 7; d++){
            weekdays[d].isOn = medication.Weekdays[d];
        }

        amToggle.SetActive(!pm);
        pmToggle.SetActive(pm);

    }

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
        RefreshGUI();

    }

    public void ToggleAMPM(bool pm){
        ifPM = pm;
        UpdateInfo();
    }

    private void Close(){

        controller.checklist.RefreshChecklist();

    }

    public void SaveMedication(){

        if(medIndex == -1)
            controller.AddMedication(medication);
        else
            controller.EditMedication(medIndex, medication);
        Close();

    }

    public void DeleteMedication(){

        if(medIndex != =1)
            controller.DeleteMedication(medIndex);
        Close();

    }

    public void ChangeStars(int stars){

        medication.Stars = stars;
        UpdateInfo();

    }

}
