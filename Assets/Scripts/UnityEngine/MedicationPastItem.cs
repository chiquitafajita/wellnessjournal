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

    private DBController database;
    private MedicationController controller;
    private int id;
    private DateTime date;
    private int status;

    public void Refresh(DBController database, Medication med, DateTime date){

        this.database = database;
        this.id = med.ID;
        this.date = date;

        nameLabel.text = med.Name;
        status = database.GetLogStatus(id, date);

        for(int s = 0; s < 3; s++){
            stars[s].isOn = s < med.Stars;
            stars[s].interactable = false;
        }

        RefreshStatusLabel();

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