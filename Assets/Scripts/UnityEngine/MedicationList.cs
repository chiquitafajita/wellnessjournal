using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicationList : MonoBehaviour
{

    public GameObject menu;
    public MedicationController controller;
    public MedicationEditor editor;
    public GameObject itemTemplate;
    public Transform contentWindow;
    private List<Medication> meds;
    private ObjectPool items;

    public void Setup(){
        items = new ObjectPool(itemTemplate);
        Refresh();
    }

    public void Refresh(){

        meds = controller.GetAllMedications();

        items.Clear();

        // for each dose:
        GameObject go;
        for(int i = 0; i < meds.Count; i++){

            // get object from pool
            go = items.CreateNew();
            go.transform.SetParent(contentWindow);
            go.transform.SetAsLastSibling();
            go.transform.localScale = Vector3.one;
            MedicationRecord mi = go.GetComponent<MedicationRecord>();
            mi.Refresh(this, meds[i]);

        }

    }

    public Medication GetMedication(int index){

        return meds[index];

    }

    public void EditMedication(int id){

        editor.Open(id);

    }

}
