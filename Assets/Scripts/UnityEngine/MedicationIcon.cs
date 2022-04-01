using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MedicationIcon : MonoBehaviour {

    public Image[] shapes;

    public void Refresh(Medication med){

        for(int s = 0; s < shapes.Length; s++){
            shapes[s].gameObject.SetActive(s == med.Shape);
            shapes[s].color = PillColors.GetColor(med.Color);
        }

    }

}