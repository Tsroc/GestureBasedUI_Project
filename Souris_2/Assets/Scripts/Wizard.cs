using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    GameObject cat;
    private void Start()
    {
        cat = GameObject.FindGameObjectWithTag("Enemy");

    }

    public void CastSleep()
    {
        // The wizard puts the cat to sleep, when player interacts with the wizard.
        Debug.Log("The wizard has put the cat to sleep.");
        cat.GetComponent<Cat>().Sleep();

    }
}
