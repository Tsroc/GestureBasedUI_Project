using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : IState
{
    public void Enter()
    {
        Debug.Log("The cat has found you!");
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
