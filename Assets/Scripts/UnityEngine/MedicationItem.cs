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

        this.controller = controller;
        this.id = med.ID;

        nameLabel.text = med.Name;
        TimeSpan timeUntil = med.GetTimeUntil();
        int status = controller.HasDoseBeenTakenToday(med.ID) ? 3 : med.GetTimePosition();
        for(int s = 0; s < 3; s++){
            stars[s].isOn = s < med.Stars;
            stars[s].interactable = false;
        }
        switch(status){
            case 0: 
                statusLabelRegular.gameObject.SetActive(true);
                statusLabelLate.gameObject.SetActive(false);
                takeButton.interactable = true;
                if(timeUntil.Hours > 0)
                    statusLabelRegular.text = "Due in " + timeUntil.Hours + " hours.";
                else
                    statusLabelRegular.text = "Due in " + timeUntil.Minutes + " minutes.";
                break;
            case 1:
                statusLabelRegular.gameObject.SetActive(true);
                statusLabelLate.gameObject.SetActive(false);
                statusLabelRegular.text = "Due now.";
                takeButton.interactable = true;
                break;
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
            default:
                statusLabelRegular.gameObject.SetActive(true);
                statusLabelLate.gameObject.SetActive(false);
                statusLabelRegular.text = "Taken.";
                takeButton.interactable = false;
                break;
        }

    }

    public void Take(){

        controller.TakeMedication(id);

    }

}
