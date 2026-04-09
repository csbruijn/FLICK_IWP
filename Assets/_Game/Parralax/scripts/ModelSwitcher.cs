using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ModelSwitcher : MonoBehaviour
{
    private List<GameObject> modelsToSwitchOut;
    private int Index = 1;

    private void Start()
    {
        modelsToSwitchOut = new List<GameObject>(); // create new list

        foreach (Transform child in transform) // for every child that is in this object 
        {
            modelsToSwitchOut.Add(child.gameObject); // it adds a new one thus making a list
        }

        switchOut(); // make sure to show at least one child at start
    }

    public void switchOut() // custom event
    {
        foreach (GameObject model in modelsToSwitchOut) // look at every item in list
        {
            model.SetActive(false); // hide everything
        }
        int IndexForCode = Index - 1;
        modelsToSwitchOut[IndexForCode].SetActive(true); // reveal the only object we wanna see

        if (Index < modelsToSwitchOut.Count) // in case there is another model to show,-1 to account for starting at 0
        {
            Index++; // move to next one
        }
        else // in case all the models have already been shown
        {
            Index = 1; // move back to first one
        }
    }
}
