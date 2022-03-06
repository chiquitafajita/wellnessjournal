using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicationChecklist : MonoBehaviour
{

    public MedicationController controller;
    public GameObject itemTemplate;
    public Transform contentWindow;
    private List<GameObject> items;
    public bool showTaken = false;

    private void Start() {
        items = new List<GameObject>();
    }
    
    public void RefreshChecklist(){

        // we are pooling checklist items as game objects
        // that way, we are not constantly destroying or creating objects

        // set all existing items inactive
        for(int i = 0; i < items.Count; i++){
            items[i].SetActive(false);
        }

        // for each dose:
        for(int i = 0; i <= controller.MaxIndex; i++){

            // if we have run out of pooled objects, create a new one
            if(i >= items.Count){
                items.Add(GameObject.Instantiate(itemTemplate));
                items[i].transform.SetParent(contentWindow);
            }

            // set the object active and refresh it with data from controller
            items[i].SetActive(true);
            MedicationItem mi = items[i].GetComponent<MedicationItem>();
            mi.Refresh(controller, i);

        }

    }

}
