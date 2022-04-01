using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MedicationPastItem : MonoBehaviour
{
    public Text nameLabel;
    public Text statusLabel;
    public Toggle[] stars;
    public MedicationIcon icon;

    private DBController database;
    private MedicationController controller;
    private int id;
    private DateTime date;
    private int status;

    public void Refresh(DBController database, Medication med, DateTime date){

        this.database = database;   // get db
        this.id = med.ID;           // get id
        this.date = date;           // get date

        // refresh name
        nameLabel.text = med.Name;

        // refresh stars
        for(int s = 0; s < 3; s++){
            stars[s].isOn = s < med.Stars;
            stars[s].interactable = false;
        }

        // refresh status label
        status = database.GetLogStatus(id, date);
        RefreshStatusLabel();

        // set icon (with shape and color)
        icon.Refresh(med);

    }

    private void RefreshStatusLabel(){

        switch(status){
            case 0: 
                statusLabel.text = "Status: Untaken.";
                break;
            case 1:
                statusLabel.text = "Status: Taken late.";
                break;
            case 2:
                statusLabel.text = "Status: Taken on time.";
                break;
        }

    }

    public void ClickSelf(){

        status = status > 2 ? 0 : status + 1;
        RefreshStatusLabel();
        database.ChangeLogStatus(id, date, status);

    }

}
