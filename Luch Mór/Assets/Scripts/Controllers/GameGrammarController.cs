using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;  // for stringbuilder
using UnityEngine;
using UnityEngine.Windows.Speech;   // grammar recogniser


/*
 *  Uses English US in the settings - Keyboard (on the taskbar), Region, Preferred Language and Speech in Settings
 */

public class GameGrammarController : MonoBehaviour
{
    private GrammarRecognizer gr;
    private GeneralMethods gm;

    private void Start()
    {
        gr = new GrammarRecognizer(Path.Combine(Application.streamingAssetsPath,
                                                "GameGrammar.xml"),
                                    ConfidenceLevel.Low);
        Debug.Log("Grammar loaded!");
        gr.OnPhraseRecognized += GR_OnPhraseRecognized;
        gr.Start();
        if (gr.IsRunning) Debug.Log("Recogniser running");

        gm = GetComponent<GeneralMethods>();

    }

    private void GR_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder message = new StringBuilder();
        SemanticMeaning[] meanings = args.semanticMeanings;
        string keyString;
        string valueString = "";

        foreach (SemanticMeaning meaning in meanings)
        {
            keyString = meaning.key.Trim();
            valueString = meaning.values[0].Trim();
            message.Append("Key: " + keyString + ", Value: " + valueString + " ");
        }
        Debug.Log(message);
        DecideAction(valueString);
    }

    private void DecideAction(string arg)
    {
        // Movement
        if (arg.ToLower().Equals("up"))
        {
            // Try to make this more readable...
            gm.SetDestination("up"); 
        }
        else if (arg.ToLower().Equals("down"))
        {
            gm.SetDestination("down"); 
        }
        else if (arg.ToLower().Equals("left"))
        {
            gm.SetDestination("left"); 
        }
        else if (arg.ToLower().Equals("right"))
        {
            gm.SetDestination("right"); 
        }
        // Path
        else if (arg.ToLower().Equals("go to home"))
        {
            gm.SetDestination("home");
        }
        else if (arg.ToLower().Equals("go to wizard"))
        {
            gm.SetDestination("wizard");
        }
        else if (arg.ToLower().Equals("go to cheese"))
        {
            gm.SetDestination("cheese");
        }
        // Action
        else if (arg.ToLower().Equals("attack cat"))
        {
            gm.SetDestination("cat");
        }
        else if (arg.ToLower().Equals("stop"))
        {
            gm.ClearDestination();
            Debug.Log("Stop");
        }
        // Wizard interaction
        else if (arg.ToLower().Equals("sleep"))
        {
            // if close - interact
            if (gm.GetDistance("wizard") < 2)
            {
                gm.WizardSpells("sleep");
            }
            else
            {
                Debug.Log("Out of range.");
            }

        }
        else if (arg.ToLower().Equals("evolve"))
        {
            // if close - interact
            if (gm.GetDistance("wizard") < 2)
            {
                gm.WizardSpells("evolve");
            }
            else
            {
                Debug.Log("Out of range.");
            }
        }
        // Cheese interaction
        else if (arg.ToLower().Equals("take cheese"))
        {
            if (gm.GetDistance("cheese") < 2)
            {
                gm.PlayerInteractions("cheese");
            }
            else
            {
                Debug.Log("Out of range.");
            }
        }
        else if (arg.ToLower().Equals("drop cheese"))
        {
            if (gm.GetDistance("home") < 2)
            {
                gm.PlayerInteractions("home");

            }
            else
            {
                Debug.Log("Out of range.");
            }
        }
        // Gameover menu
        else if (arg.ToLower().Equals("menu"))
        {
            if (!gm.Gameover())
            {
                Debug.Log("The game is not over.");
            }
            else
            {
                gm.MenuScene();
            }
        }
        else if (arg.ToLower().Equals("exit"))
        {
            if (!gm.Gameover())
            {
                Debug.Log("The game is not over.");
            }
            else
            {
                gm.QuitGame();
            }
        }
    }


    private void OnApplicationQuit()
    {
        if (gr != null && gr.IsRunning)
        {
            gr.OnPhraseRecognized -= GR_OnPhraseRecognized;
            gr.Stop();
        }
    }
}