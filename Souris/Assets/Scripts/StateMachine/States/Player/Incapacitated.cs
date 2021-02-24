using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incapacitated :IState 
{
    private System.Action mobileCallback;

    float duration = 2.0f;

    public Incapacitated(System.Action mobileCallback)
    {
        this.mobileCallback = mobileCallback;

    }

    public void Enter()
    {

    }

    public void Execute()
    {
        if (duration > 0)
        {
            duration -= Time.deltaTime;
        }
        else
        {
            this.ResumeMovement();
        }
    }

    public void Exit()
    {

    }

    private void ResumeMovement()
    {
        this.mobileCallback();
    }


}
