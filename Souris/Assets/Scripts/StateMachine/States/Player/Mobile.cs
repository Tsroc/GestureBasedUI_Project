using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobile : IState 
{
    GameObject ownerGameObject;
    float movement;
    float hClamp, vClamp;
    private System.Action incapacitatedCallback;

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

    }

    public void Execute()
    {
        if (Input.GetAxisRaw("Horizontal") == 1)
            MoveLeft();
        if (Input.GetAxisRaw("Horizontal") == -1)
            MoveRight();
        if (Input.GetAxisRaw("Vertical") == 1)
            MoveUp();
        if (Input.GetAxisRaw("Vertical") == -1)
            MoveDown();
    }

    public void Exit()
    {

    }
    private void MoveUp()
    {
        ownerGameObject.transform.Translate(0, movement, 0);
        ClampMovement();
    }

    private void MoveDown()
    {
        ownerGameObject.transform.Translate(0, -movement, 0);
        ClampMovement();
    }

    private void MoveLeft()
    {
        ownerGameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);

        ownerGameObject.transform.Translate(-movement, 0, 0);
        ClampMovement();
    }

    private void MoveRight()
    {
        ownerGameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        ownerGameObject.transform.Translate(-movement, 0, 0);
        ClampMovement();
    }

    private void ClampMovement()
    {
        // https://answers.unity.com/questions/925199/restricting-movement-with-mathfclamp.html

        // initially, the temporary vector should equal the player's position
        Vector3 clampedPosition = ownerGameObject.transform.position;
        // Now we can manipulte it to clamp the y element
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -vClamp, vClamp);
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -hClamp, hClamp);
        // re-assigning the transform's position will clamp it
        ownerGameObject.transform.position = clampedPosition;
    }
}
