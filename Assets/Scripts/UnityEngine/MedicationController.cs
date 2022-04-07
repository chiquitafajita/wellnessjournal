using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MedicationController : MonoBehaviour
{
    
    public DBController database;
    public MedicationChecklist checklist;
    public MedicationList listAll;
    public DayRater dayRater;
    public TagWriter tags;
    public CalendarController calendar;
    public MicrojournalController microjournal;
    public bool showTakenDoses = false;

    // set up all GUI elements and refresh them
    private void Awake() {
        calendar.Setup();
        checklist.Setup();
        listAll.Setup();
        dayRater.Refresh(TimeKeeper.GetDate());
        tags.Refresh(TimeKeeper.GetDate());
        microjournal.Refresh();
    }

    // refresh all GUI elements
    public void Refresh(){
        calendar.Refresh();
        checklist.Refresh();
        listAll.Refresh();
        dayRater.Refresh(TimeKeeper.GetDate());
        tags.Refresh(TimeKeeper.GetDate());
        microjournal.Refresh();
    }

    // add medication to database
    public void AddMedication(Medication medication){

        // returns id
        int id = database.InsertMedication(medication);

        // if the notify time was designated prior to when it was created
        // count as taken so that the user doesn't get points off
        if(medication.GetTimePosition() == 2)
            database.ChangeLogStatus(id, TimeKeeper.GetDate(), 2);

        // refresh all GUI
        Refresh();

    }

    // edit medication and refresh all GUI
    public void EditMedication(Medication medication){
        database.UpdateMedication(medication);
        Refresh();
    }

    // take medication and refresh all GUI
    public void TakeMedication(int id){
        int status = database.GetMedication(id).GetTimePosition() == 2 ?    // equals 2 if late
                    1 : 2;  // if late, status is 1; otherwise it is 2
                            // this way we can score late medications as half-worth
        database.ChangeLogStatus(id, TimeKeeper.GetDate(), status);
        Refresh();
    }

    // delete medication and refresh all GUI
    public void DeleteMedication(Medication medication){
        database.DeleteMedication(medication);
        Refresh();
    }

    // get medication from database by ID
    public Medication GetMedication(int id){

        return database.GetMedication(id);

    }

    // get all medications
    public List<Medication> GetAllMedications(){

        return database.GetAllMedications();

    }

    // get doses scheduled for a given day (i.e. doses that have a log for that date)
    public List<Medication> GetMedicationsScheduledForDate(DateTime date){

        // 'showTakenDoses' toggles if doses with non-0 logs (i.e. those taken) are included
        return database.GetDosesForDay(date, showTakenDoses);

    }

    // returns true if dose has been taken today
    public bool HasDoseBeenTakenToday(int id){

        // if status is > 0, then it has not been taken or taken late
        return database.GetLogStatus(id, TimeKeeper.GetDate()) > 0;

    }

}
