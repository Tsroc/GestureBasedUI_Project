using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleeping : IState
{
    private float duration = 5.0f;

    private System.Action wakeCallback;

    public Sleeping(System.Action wakeCallback)
    {
        this.wakeCallback = wakeCallback;

    }

    public void Enter()
    {
        // Lower detection range.
        Debug.Log("The cat is sleeping...");
    }

    public void Execute()
    {
        if (duration > 0)
        {
            duration -= Time.deltaTime;
        }
        else
        {
            this.WakeUp();
        }
    }

    public void Exit()
    {
        Debug.Log("The cat has finished sleeping..."); 
    }

    private void WakeUp()
    {
        this.wakeCallback();
    }
}
