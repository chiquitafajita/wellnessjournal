using UnityEngine;
using UnityEngine.UI;

public class WeekLabels : MonoBehaviour
{

    public Text date;
    public GameObject[] dayLabelsOff;
    public GameObject[] dayLabelsOn;

    // Start is called before the first frame update
    void Start()
    {
        
        date.text = TimeKeeper.GetMonth() + " " + TimeKeeper.GetDay();

        for(int d = 0; d < 7; d++){
            dayLabelsOff[d].SetActive(d != TimeKeeper.GetDayOfWeek());
            dayLabelsOn[d].SetActive(d == TimeKeeper.GetDayOfWeek());
        }

    }

}
