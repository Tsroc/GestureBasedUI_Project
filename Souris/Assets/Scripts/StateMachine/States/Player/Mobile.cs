using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Mobile : IState 
{
    private GrammarRecognizer gr;
    GameObject ownerGameObject;
    private float movement;
    private float hClamp, vClamp;
    private bool hasTarget = false;
    private System.Action incapacitatedCallback;
    private Transform target;

    public Mobile(GameObject ownerGameObject, float movement,
            float vClamp, float hClamp, System.Action incapacitatedCallback)
    {
        this.ownerGameObject = ownerGameObject;
        this.movement = movement;
        this.vClamp = vClamp;
        this.hClamp = hClamp;
        this.incapacitatedCallback = incapacitatedCallback;

    }

    public void Enter()
    {
        gr = new GrammarRecognizer(Path.Combine(Application.streamingAssetsPath,
                                                "GameGrammar.xml"),
                                    ConfidenceLevel.Low);
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
            MoveUp();
            Debug.Log("Up");
        }
        else if (arg.ToLower().Equals("down"))
        {
            MoveDown();
            Debug.Log("Down");
        }
        else if (arg.ToLower().Equals("left"))
        {
            MoveLeft();
            Debug.Log("Left");
        }
        else if (arg.ToLower().Equals("right"))
        {
            MoveRight();
            Debug.Log("Right");
        }
        else if (arg.ToLower().Equals("go to home"))
        {
            target = GameObject.FindGameObjectWithTag("Home").transform;

            hasTarget = true;
            Debug.Log("Home");
        }
        else if (arg.ToLower().Equals("go to wizard"))
        {
            target = GameObject.FindGameObjectWithTag("Wizard").transform;

            hasTarget = true;
            Debug.Log("Wizard");
        }
        else if (arg.ToLower().Equals("go to cheese"))
        {
            target = GameObject.FindGameObjectWithTag("PickUp").transform;

            hasTarget = true;
            Debug.Log("Cheese");
        }
        else if (arg.ToLower().Equals("attack cat"))
        {
            target = GameObject.FindGameObjectWithTag("Enemy").transform;

            hasTarget = true;
            Debug.Log("Cat");
        }
        else if (arg.ToLower().Equals("cast sleep"))
        {
            // Check distance from wizard,
            GameObject wizard = GameObject.FindGameObjectWithTag("Wizard");
            float distance = Vector3.Distance(ownerGameObject.transform.position,
                wizard.transform.position);

            // if close - interact
            if (distance < 2)
            {
                wizard.GetComponent<Wizard>().CastSleep();
            }
            else
            {
                Debug.Log("Out of range.");
            }

        }
        else if (arg.ToLower().Equals("stop"))
        {
            target = null;

            hasTarget = false;
            Debug.Log("Stop");
        }
    }


    public void Execute()
    {
        if (hasTarget)
        {
            // move to target
            float step = 3f * Time.deltaTime;
            
            if(ownerGameObject.transform.position.x < target.transform.position.x)
                ownerGameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            else
                ownerGameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

            ownerGameObject.transform.position = Vector3.MoveTowards(ownerGameObject.transform.position, target.position, step);

        }
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            ClearTarget();
            MoveRight();
        }
        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            ClearTarget();
            MoveLeft();
        }
        if (Input.GetAxisRaw("Vertical") == 1)
        {
            ClearTarget();
            MoveUp();
        }
        if (Input.GetAxisRaw("Vertical") == -1)
        {
            ClearTarget();
            MoveDown();
        }
    }

    private void ClearTarget()
    {
        hasTarget = false;
        target = null;
    }

    public void Exit()
    {

        if (gr != null && gr.IsRunning)
        {
            gr.OnPhraseRecognized -= GR_OnPhraseRecognized;
            gr.Stop();
        }

    }
    private void MoveUp()
    {
        ownerGameObject.transform.Translate(0, this.movement * Time.deltaTime, 0);
        ClampMovement();
    }

    private void MoveDown()
    {
        ownerGameObject.transform.Translate(0, -this.movement * Time.deltaTime, 0);
        ClampMovement();
    }

    private void MoveLeft()
    {
        ownerGameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        ownerGameObject.transform.Translate(-this.movement * Time.deltaTime, 0, 0);
        ClampMovement();
    }

    private void MoveRight()
    {
        ownerGameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);

        ownerGameObject.transform.Translate(-this.movement * Time.deltaTime, 0, 0);
        ClampMovement();
    }

    private void ClampMovement()
    {
        // https://answers.unity.com/questions/925199/restricting-movement-with-mathfclamp.html

        // initially, the temporary vector should equal the player's position
        Vector3 clampedPosition = ownerGameObject.transform.position;
        // Now we can manipulte it to clamp the y element
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -vClamp, vClamp-1);
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -hClamp, hClamp);
        // re-assigning the transform's position will clamp it
        ownerGameObject.transform.position = clampedPosition;
    }
}
