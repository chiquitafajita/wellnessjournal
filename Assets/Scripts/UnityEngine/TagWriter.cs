using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TagWriter : MonoBehaviour
{

    public MedicationController controller;
    public Text output;
    public InputField inputField;
    public DBController database;

    private DateTime date;

    public void Refresh(DateTime date){

        this.date = date;
        string db = database.GetDayTags(date);
        output.text = string.IsNullOrEmpty(db) ? "You have recorded no tags today." : db;
        inputField.text = db;

    }

    public void SetTags(){

        database.UpdateDayTags(date, inputField.text);
        Refresh(date);
        controller.Refresh();

    }

}
