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
    private Vector3 target;

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
        Debug.Log(message);

        DecideAction(valueString);
    }

    private void DecideAction(string arg)
    {

        // Movement
        if (arg.ToLower().Equals("up"))
        {
            target = new Vector3(ownerGameObject.transform.position.x, ownerGameObject.transform.position.y + 10, ownerGameObject.transform.position.z);
            hasTarget = true;
            Debug.Log("Home");
        }
        else if (arg.ToLower().Equals("down"))
        {
            target = new Vector3(ownerGameObject.transform.position.x, ownerGameObject.transform.position.y - 10, ownerGameObject.transform.position.z);
            hasTarget = true;
            Debug.Log("Down");
        }
        else if (arg.ToLower().Equals("left"))
        {
            target = new Vector3(ownerGameObject.transform.position.x - 20, ownerGameObject.transform.position.y, ownerGameObject.transform.position.z);
            hasTarget = true;
            Debug.Log("Left");
        }
        else if (arg.ToLower().Equals("right"))
        {
            target = new Vector3(ownerGameObject.transform.position.x + 20, ownerGameObject.transform.position.y, ownerGameObject.transform.position.z);
            hasTarget = true;
            Debug.Log("Right");
        }
        // Path
        else if (arg.ToLower().Equals("go to home"))
        {
            target = GameObject.FindGameObjectWithTag("Home").transform.position;

            hasTarget = true;
            Debug.Log("Home");
        }
        else if (arg.ToLower().Equals("go to wizard"))
        {
            target = GameObject.FindGameObjectWithTag("Wizard").transform.position;

            hasTarget = true;
            Debug.Log("Wizard");
        }
        else if (arg.ToLower().Equals("go to cheese"))
        {
            target = GameObject.FindGameObjectWithTag("PickUp").transform.position;

            hasTarget = true;
            Debug.Log("Cheese");
        }
        // Action
        else if (arg.ToLower().Equals("attack cat"))
        {
            target = GameObject.FindGameObjectWithTag("Enemy").transform.position;

            hasTarget = true;
            Debug.Log("Cat");
        }
        else if (arg.ToLower().Equals("stop"))
        {
            ClearTarget();
            Debug.Log("Stop");
        }
        else if (arg.ToLower().Equals("cast sleep"))
        {
            // if close - interact
            if (this.GetDistance("Wizard") < 2)
            {
                GameObject.FindGameObjectWithTag("Wizard").GetComponent<Wizard>().CastSleep(); ;
            }
            else
            {
                Debug.Log("Out of range.");
            }

        }
        else if (arg.ToLower().Equals("cast evolve"))
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
        else if (arg.ToLower().Equals("deposit cheese"))
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
        hasTarget = false;
        //target = null;
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
