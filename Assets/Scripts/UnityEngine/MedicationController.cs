using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicationController : MonoBehaviour
{
    
    public DBController database;
    public MedicationChecklist checklist;
    public MedicationList listAll;
    public bool showTakenDoses = false;

    private void Awake() {
        checklist.Setup();
        listAll.Setup();
    }

    public void Refresh(){
        checklist.Refresh();
        listAll.Refresh();
    }

    public void AddMedication(Medication medication){

        int id = database.InsertMedication(medication);

        // if the notify time was designated prior to when it was created
        // count as taken so that the user doesn't get points off
        if(medication.GetTimePosition() == 2)
            database.ChangeLogStatus(id, TimeKeeper.GetDate(), 2);

        Refresh();

    }

    public void EditMedication(Medication medication){
        database.UpdateMedication(medication);
        Refresh();
    }

    public void TakeMedication(int id){
        int status = database.GetMedication(id).GetTimePosition() == 2 ?    // equals 2 if late
                    1 : 2;  // if late, status is 1; otherwise it is 2
                            // this way we can score late medications as half-worth
        database.ChangeLogStatus(id, TimeKeeper.GetDate(), status);
        Refresh();
    }

    public void DeleteMedication(Medication medication){
        database.DeleteMedication(medication);
        Refresh();
    }

    public Medication GetMedicationByID(int id){

        return database.GetMedication(id);

    }

    public List<Medication> GetAllMedications(){

        return database.GetAllMedications();

    }

    public List<Medication> GetMedicationsScheduledToday(){

        return database.GetMedicationsForDay(TimeKeeper.GetDate(), showTakenDoses);

    }

    public bool HasDoseBeenTakenToday(int id){

        // if status is > 0, then it has not been taken or taken late
        return database.GetLogStatus(id, TimeKeeper.GetDate()) > 0;

    }

}
