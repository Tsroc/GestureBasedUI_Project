using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waking : IState
{
    private float duration = 7.0f;

    private System.Action alertCallback;

    public Waking(System.Action wakeCallback)
    {
        this.alertCallback = wakeCallback;

    }

    public void Enter()
    {
        // Lower detection range.
        Debug.Log("The cat is begining to wake...");
    }

    public void Execute()
    {
        if (duration > 0)
        {
            duration -= Time.deltaTime;
        }
        else
        {
            this.BecomeAlert();
        }
    }

    public void Exit()
    {
    }

    private void BecomeAlert()
    {
        this.alertCallback();
    }
}
