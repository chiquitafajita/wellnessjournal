using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MedicationIcon : MonoBehaviour {

    public Image[] shapes;

    public void Refresh(Medication med){

        // for each possible shape object
        for(int s = 0; s < shapes.Length; s++){
            shapes[s].gameObject.SetActive(s == med.Shape);     // set object active if right shape
            shapes[s].color = PillColors.GetColor(med.Color);   // change color of shape as defined
        }

    }

}