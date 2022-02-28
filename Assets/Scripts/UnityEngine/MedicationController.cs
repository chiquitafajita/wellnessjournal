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
        
        medications = new List<Medication>();

    }

    public int AddMedication(Medication medication){
        medications.Add(medication);
        Debug.Log(medication.GetSqlValues());
        database.InsertMedication(medication);
        return MaxIndex;
    }

    public void EditMedication(int index, Medication medication){
        medications[index] = medication;
        Debug.Log(medication.Name + " (" + medication.Stars + " stars) will take place at " + medication.NotifyTime);
    }

    public Medication GetMedication(int index){
        return medications[index];
    }

    public void SortMedications(){

        medications.Sort();

    }

    public void TakeMedication(int index){
        medications[index].Take();
    }

    public void DeleteMedication(int index){
        medications.RemoveAt(index);
    }

}
