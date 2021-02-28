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
    // Change to destination...
    private bool hasTarget = false;
    private Vector3 target;
    private System.Action incapacitatedCallback;

    GameObject wizard;
    private Vector3 homePosition;
    private Vector3 cheesePosition;
    private Vector3 wizardPosition;
    private Vector3 catPosition;

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
        // This is not ideal, Start and Awake methods return wrong positions.
        wizard =  GameObject.FindGameObjectWithTag("Wizard");
        homePosition = GameObject.FindGameObjectWithTag("Home").transform.position;
        cheesePosition = GameObject.FindGameObjectWithTag("PickUp").transform.position;
        wizardPosition = GameObject.FindGameObjectWithTag("Wizard").transform.position;
        catPosition = GameObject.FindGameObjectWithTag("Enemy").transform.position;

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
            SetDestination(ownerGameObject.transform.position + new Vector3(0, 10, 0)); 
        }
        else if (arg.ToLower().Equals("down"))
        {
            SetDestination(ownerGameObject.transform.position + new Vector3(0, -10, 0)); 
        }
        else if (arg.ToLower().Equals("left"))
        {
            SetDestination(ownerGameObject.transform.position + new Vector3(-20, 0, 0)); 
        }
        else if (arg.ToLower().Equals("right"))
        {
            SetDestination(ownerGameObject.transform.position + new Vector3(20, 0, 0)); 
        }
        // Path
        else if (arg.ToLower().Equals("go to home"))
        {
            SetDestination(homePosition);
        }
        else if (arg.ToLower().Equals("go to wizard"))
        {
            SetDestination(wizardPosition);
        }
        else if (arg.ToLower().Equals("go to cheese"))
        {
            SetDestination(cheesePosition);
        }
        // Action
        else if (arg.ToLower().Equals("attack cat"))
        {
            SetDestination(catPosition);
        }
        else if (arg.ToLower().Equals("stop"))
        {
            ClearTarget();
            Debug.Log("Stop");
        }
        else if (arg.ToLower().Equals("sleep"))
        {
            // if close - interact
            if (this.GetDistance("Wizard") < 2)
            {
                wizard.GetComponent<Wizard>().CastSleep(); ;
            }
            else
            {
                Debug.Log("Out of range.");
            }

        }
        else if (arg.ToLower().Equals("evolve"))
        {
            // if close - interact
            if (this.GetDistance("Wizard") < 2)
            {
                ownerGameObject.GetComponent<Player>().Evolve(); ;

            }
            else
            {
                Debug.Log("Out of range.");
            }
        }
        else if (arg.ToLower().Equals("take cheese"))
        {
            if (this.GetDistance("PickUp") < 2)
            {
                ownerGameObject.GetComponent<Player>().InteractWithCheese(); ;

            }
            else
            {
                Debug.Log("Out of range.");
            }
        }
        else if (arg.ToLower().Equals("drop cheese"))
        {
            if (this.GetDistance("Home") < 2)
            {
                ownerGameObject.GetComponent<Player>().InteractWithHome(); ;

            }
            else
            {
                Debug.Log("Out of range.");
            }
        }
    }


    public void Execute()
    {
        // Will have a target if a path 
        if (hasTarget)
        {
            // move to target
            float step = 3f * Time.deltaTime;

            if (ownerGameObject.transform.position.x < target.x)
                RotateRight();
            else
                RotateLeft();

            ownerGameObject.transform.position = Vector3.MoveTowards(ownerGameObject.transform.position, target, step);
            ClampMovement();

        }

        // manual moveemnt will break path movement
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
        this.hasTarget = false;
    }

    private void SetDestination(Vector3 destination)
    {
        this.target = destination; 
        this.hasTarget = true;
    }
    
    private float GetDistance(string t)
    {
        // Check distance from wizard,
        GameObject target  = GameObject.FindGameObjectWithTag(t);
        float distance = Vector3.Distance(ownerGameObject.transform.position,
            target.transform.position);

        return distance;

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
        RotateLeft();

        ownerGameObject.transform.Translate(-this.movement * Time.deltaTime, 0, 0);
        ClampMovement();
    }

    private void MoveRight()
    {
        RotateRight();

        ownerGameObject.transform.Translate(-this.movement * Time.deltaTime, 0, 0);
        ClampMovement();
    }

    /*
     * Faces the player to the left.
     */
    private void RotateLeft()
    {
        ownerGameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }

    /*
     * Faces the player to the right.
     */
    private void RotateRight()
    {
        ownerGameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
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

