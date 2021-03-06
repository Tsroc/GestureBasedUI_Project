using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    private StateMachine stateMachine = new StateMachine();
    private Animator anim;
    private CircleCollider2D collider;
    private Transform alertRadius;
    private bool gameover = false;

    private float[] radius = {2.0f, 3.0f, 5.0f};

    private void Start()
    {
        anim = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();
        alertRadius = gameObject.transform.Find("alertRadius");
        Sleep();
    }

    private void Update()
    {
        if(!gameover)
            this.stateMachine.ExecuteStateUpdate();
    }

    public void Sleep()
    {
        // Collider radius changes, there is a slight delay. The change should happen after the animation updates as
        // the player will react to what they see and should not get unexpected, non-visible changes.
        anim.SetInteger("state", 0);
        this.stateMachine.ChangeState(new Sleeping(this.Wake));
        alertRadius.localScale = new Vector3(1.6f, 1.6f, 0.0f);
        collider.radius = radius[0];
    }

    private void Wake()
    {
        anim.SetInteger("state", 1);
        this.stateMachine.ChangeState(new Waking(this.Alert));
        alertRadius.localScale = new Vector3(2.3f, 2.3f, 0.0f);
        collider.radius = radius[1];
    }

    private void Alert()
    {
        anim.SetInteger("state", 2);
        this.stateMachine.ChangeState(new Alert());
        // Collider radius = 5; Player can no longer safely pass
        alertRadius.localScale = new Vector3(4.0f, 4.0f, 0.0f);
        collider.radius = radius[2];
    }

    public void Attack()
    {
        // Game over
        gameover = true;
        anim.SetInteger("state", 3);
    }

    public void Scared()
    {
        // Game over
        gameover = true;
        anim.SetInteger("state", 4);
    }


}
