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
    private List<GameObject> items;
    private List<Medication> meds;

    public void Setup(){
        items = new List<GameObject>();
        Refresh();
    }

    public void Refresh(){

        meds = controller.GetAllMedications();

        foreach(GameObject go in items)
            go.SetActive(false);

        // for each dose:
        for(int i = 0; i < meds.Count; i++){

            // if we have run out of pooled objects, create a new one
            if(i >= items.Count){
                items.Add(GameObject.Instantiate(itemTemplate));
                items[i].transform.SetParent(contentWindow);
                items[i].transform.localScale = Vector3.one;
            }

            // set the object active and refresh it with data from controller
            items[i].SetActive(true);
            MedicationRecord mi = items[i].GetComponent<MedicationRecord>();
            mi.Refresh(this, meds[i]);

        }

    }

    public Medication GetMedication(int index){

        return meds[index];

    }

}
