using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    /*
        LoaderCallback is used to display the loading scene while waiting the called scene to load.
        The load scene will display when Update is called.

            Callback info: https://www.youtube.com/watch?v=3I5d2rUJ0pE
    */

    private bool isFirstUpdate = true;

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            SceneController.LoaderCallback();
        }
    }

}
