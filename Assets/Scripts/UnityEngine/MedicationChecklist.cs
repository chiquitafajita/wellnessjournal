using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicationChecklist : MonoBehaviour
{

    public MedicationController controller;
    public GameObject itemTemplate;
    public Transform contentWindow;
    private ObjectPool items;
    public bool showTaken = false;

    public void Setup() {
        items = new ObjectPool(itemTemplate);
        Refresh();
    }
    
    public void Refresh(){

        // we are pooling checklist items as game objects
        // that way, we are not constantly destroying or creating objects

        items.Clear();

        List<Medication> meds = controller.GetMedicationsScheduledForDate(TimeKeeper.GetDate());

        // for each dose:
        GameObject go;
        for(int i = 0; i < meds.Count; i++){

            // retrieve pool object and refresh it with data from controller
            go = items.CreateNew();
            go.transform.SetParent(contentWindow);
            go.transform.SetAsLastSibling();
            MedicationItem mi = go.GetComponent<MedicationItem>();
            mi.Refresh(controller, meds[i]);

        }

    }

}
