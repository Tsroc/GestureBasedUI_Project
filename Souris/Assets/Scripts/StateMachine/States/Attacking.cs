using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : IState
{
    private float duration = 10.0f;

    public void Enter()
    {
        Debug.Log("The cat is attacking...");
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
