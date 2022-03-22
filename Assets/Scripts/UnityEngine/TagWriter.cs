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
        RefreshAppearance(database.GetDayTags(date));

    }

    private void RefreshAppearance(String tags){

        output.text = string.IsNullOrEmpty(tags) ? "You have recorded no tags today." : tags;
        inputField.text = tags;

    }

    public void SetTags(){

        RefreshAppearance(inputField.text);
        database.UpdateDayTags(date, inputField.text);
        //controller.Refresh();

    }

}
