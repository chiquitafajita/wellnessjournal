using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a pool is a way for us to reuse objects that exist in memory
// without constantly destroying and creating new ones.
public class ObjectPool 
{
    
    // we have a storage of objects
    List<GameObject> objects = new List<GameObject>();

    // how many objects are currently in use
    int used;

    // the template object we clone
    GameObject template;

    // constructor for a pool
    public ObjectPool(GameObject template){

        this.template = template;
        used = -1;
        objects = new List<GameObject>();

    }

    // clear all objects from pool by setting 'active' to false
    public void Clear(){

        foreach(GameObject go in objects){
            go.SetActive(false);
        }

    }

    // retrieve an object from the pool
    public GameObject GetObject(){

        used++;

        // if all objects are being used, create a new one
        if(used >= objects.Count){
            objects.Add(GameObject.Instantiate(template));
        }

        // set 'active' to true and return object
        objects[used].SetActive(true);
        return objects[used];

    }

}
