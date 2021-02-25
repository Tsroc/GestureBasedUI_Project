using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : IState
{

    public void Enter()
    {
        // Increase detection range.
        Debug.Log("The cat has become alert.");
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
