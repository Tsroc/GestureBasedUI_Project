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

    private void Start()
    {
        gr = new GrammarRecognizer(Path.Combine(Application.streamingAssetsPath,
                                                "GameGrammar.xml"),
                                    ConfidenceLevel.Low);
        Debug.Log("Grammar loaded!");
        gr.OnPhraseRecognized += GR_OnPhraseRecognized;
        gr.Start();
        if (gr.IsRunning) Debug.Log("Recogniser running");
    }

    private void GR_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder message = new StringBuilder();
        Debug.Log("Recognised a phrase");
        // read the semantic meanings from the args passed in.
        SemanticMeaning[] meanings = args.semanticMeanings;
        string keyString;
        string valueString = "";

        foreach (SemanticMeaning meaning in meanings)
        {
            keyString = meaning.key.Trim();
            valueString = meaning.values[0].Trim();
            message.Append("Key: " + keyString + ", Value: " + valueString + " ");
        }
        // use a string builder to create the string and out put to the user
        Debug.Log(message);

        TempMethod(valueString);
    }

    private void TempMethod(string arg)
    {
        if (arg.ToLower().Equals("up"))
        {
            Debug.Log("Up");
        }
        else if (arg.ToLower().Equals("down"))
        {
            Debug.Log("Down");
        }
        else if (arg.ToLower().Equals("left"))
        {
            Debug.Log("Left");
        }
        else if (arg.ToLower().Equals("right"))
        {
            Debug.Log("Right");
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