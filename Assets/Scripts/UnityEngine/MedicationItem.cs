using UnityEngine;
using UnityEngine.UI;
using System;

public class MedicationItem : MonoBehaviour
{
    
    public Text nameLabel;
    public Text statusLabelRegular;
    public Text statusLabelLate;
    public Button takeButton;
    public Toggle[] stars;

    private MedicationController controller;
    private int id;

    public void Refresh(MedicationController controller, Medication med){

        // set controller and ID
        this.controller = controller;
        this.id = med.ID;

        // set name label
        nameLabel.text = med.Name;

        // status is 3 if not taken
        // other statuses are defined in switch-case logic
        int status = controller.HasDoseBeenTakenToday(med.ID) ? 3 : med.GetTimePosition();
        TimeSpan timeUntil = med.GetTimeUntil();

        switch(status){

            // case 0 means time has not come
            case 0: 
                statusLabelRegular.gameObject.SetActive(true);
                statusLabelLate.gameObject.SetActive(false);
                takeButton.interactable = true;
                if(timeUntil.Hours > 0)
                    statusLabelRegular.text = "Due in " + timeUntil.Hours + " hours.";
                else
                    statusLabelRegular.text = "Due in " + timeUntil.Minutes + " minutes.";
                break;
            
            // case 1 means we are in window
            case 1:
                statusLabelRegular.gameObject.SetActive(true);
                statusLabelLate.gameObject.SetActive(false);
                statusLabelRegular.text = "Due now.";
                takeButton.interactable = true;
                break;
            
            // case 2 means late
            case 2:
                statusLabelRegular.gameObject.SetActive(false);
                statusLabelLate.gameObject.SetActive(true);
                timeUntil = timeUntil.Negate();
                takeButton.interactable = true;
                if(timeUntil.Hours > 0)
                    statusLabelLate.text = "Due " + timeUntil.Hours + " hours ago (Late).";
                else
                    statusLabelLate.text = "Due " + timeUntil.Minutes + " minutes ago (Late).";
                break;

            // case 3 means it has been taken
            default:
                statusLabelRegular.gameObject.SetActive(true);
                statusLabelLate.gameObject.SetActive(false);
                statusLabelRegular.text = "Taken.";
                takeButton.interactable = false;
                break;
        }

        // set stars
        for(int s = 0; s < 3; s++){
            stars[s].isOn = s < med.Stars;
            stars[s].interactable = false;
        }

    }

    public void Take(){

        controller.TakeMedication(id);

    }

}
