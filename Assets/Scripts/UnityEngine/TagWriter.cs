using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagWriter : MonoBehaviour
{

    public Text input;
    public Text output;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTagDisplay(){

        string tags = input.text;
        if(string.IsNullOrEmpty(tags))
            output.text = "You have recorded no tags today.";
        else
            output.text = tags;

    }

}
