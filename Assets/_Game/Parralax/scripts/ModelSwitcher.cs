using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ModelSwitcher : MonoBehaviour
{
    private List<GameObject> modelsToSwitchOut;
    private int Index = 0;

    private void Start()
    {
        modelsToSwitchOut = new List<GameObject>(); // create new list

        foreach (Transform child in transform) // for every child that is in this object 
        {
            modelsToSwitchOut.Add(child.gameObject); // it adds a new one
            child.gameObject.SetActive(false); // make disappear
        }
    }

    public void switchOut() // custom event
    {
        GameObject g = modelsToSwitchOut[Index];

        foreach (GameObject child in modelsToSwitchOut) // look at every item in list
        {

        }

    }
}
