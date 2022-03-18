using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagWriter : MonoBehaviour
{

    public MedicationController controller;
    public Text output;
    public InputField inputField;
    public DBController database;

    public void Refresh(){

        string db = database.GetDayTags(TimeKeeper.GetDate());
        output.text = string.IsNullOrEmpty(db) ? "You have recorded no tags today." : db;
        inputField.text = db;

    }

    public void SetTags(){

        database.UpdateDayTags(TimeKeeper.GetDate(), inputField.text);
        controller.Refresh();

    }

}
