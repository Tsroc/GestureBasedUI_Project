using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Utilities;

public class SceneController : MonoBehaviour
{
    /*
        SceneController manages transitions between scenes.
    */

    // Allows for the loadscene to be shown while the scene assigned to this Action loads.
    // Callback info: https://www.youtube.com/watch?v=3I5d2rUJ0pE
    private static Action onLoaderCallback;
    //[SerializeField] Text feedbackText;

    /*
        Will call the gamescene, loadscene will load on the next update and remain until the gamescene is loaded.
    */
    public void LaunchGame()
    {
        // Set the loader callback action to load the game scene
        onLoaderCallback = () => {
            SceneManager.LoadSceneAsync(SceneNames.GAMESCENE);
        };

        SceneManager.LoadSceneAsync(SceneNames.LOADING);
    }


    /*
        Will call the menuscene, loadscene will load on the next update and remain until the menuscene is loaded.
    */
    public void LaunchMainMenu()
    {
        // Set the loader callback action to load the game scene
        onLoaderCallback = () => {
            SceneManager.LoadSceneAsync(SceneNames.MAINMENU);
        };

        SceneManager.LoadSceneAsync(SceneNames.LOADING);
    }

    public static void LoaderCallback()
    {
        // Triggered after the first Update which lets the scene refresh
        // Execute the loader callback action which will load the game scene
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }

    /*
        Exits the application.
    */
    public void QuitGame()
    {
        Application.Quit();
    }
}
