using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicationController : MonoBehaviour
{
    
    public DBController database;
    public MedicationChecklist checklist;
    public int MaxIndex { get { return medications.Count - 1; } }
    private List<Medication> medications;

    private void Start() {
        
        medications = database.GetMedications(TimeKeeper.GetDate());
        checklist.RefreshChecklist();

    }

    public void AddMedication(Medication medication){
        int id = database.InsertMedication(medication);

        // if the notify time was designated prior to when it was created
        // count as taken so that the user doesn't get points off
        if(medication.GetTimePosition() == 2)
            database.ChangeLogStatus(id, TimeKeeper.GetDate(), 1);

        medications = database.GetMedications(TimeKeeper.GetDate());
    }

    public void EditMedication(int index, Medication medication){
        Debug.LogError("UNIMPLEMENTED.");
    }

    public Medication GetMedication(int index){
        return medications[index];
    }

    public void TakeMedication(int index){
        int status = medications[index].GetTimePosition() == 2 ?    // equals 2 if late
                    2 : 1;  // if late, status is 2; otherwise it is 1
        database.ChangeLogStatus(medications[index].ID, TimeKeeper.GetDate(), status);
    }

    public void DeleteMedication(int index){
        Debug.LogError("UNIMPLEMENTED.");
    }

    public bool HasDoseBeenTakenToday(int index){

        // if status is > 0, then it has not been taken or taken late
        return database.GetLogStatus(medications[index].ID, TimeKeeper.GetDate()) > 0;

    }

}
