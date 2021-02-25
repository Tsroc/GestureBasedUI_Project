using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private StateMachine stateMachine = new StateMachine();
    private Animator anim;
    [SerializeField] GameObject canvas;

    [SerializeField] private float movement = 0.8f;
    private float vClamp = 4.5f;
    private float hClamp = 8.0f;
    private bool hasCheese = false;
    private int cheeseCount = 0;
    [SerializeField] private int evolveRequirement = 1;
    private bool evolved = false;
    private bool gameover = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        Mobile();
    }

    private void Update()
    {
        if(!gameover)
            this.stateMachine.ExecuteStateUpdate();
    }

    private void Mobile()
    {
        this.stateMachine.ChangeState(new Mobile(this.gameObject, movement, vClamp, hClamp,this.Incapacitated));
    }

    private void Incapacitated()
    {
        this.stateMachine.ChangeState(new Incapacitated(this.Mobile));

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            InteractWithCat();
        }
        else if (collision.tag == "Wizard")
        {
            InteractWithWizard();
        }
        else if (collision.tag == "PickUp")
        {
            Debug.Log("Looting the chsses.");
            InteractWithCheese();
        }
        else if (collision.tag == "Home")
        {
            Debug.Log("Depositing the cheese.");
            InteractWithHome();
        }
    }

    // == Methods: movement ==
    private void GoToHome()
    {
        Transform home = GameObject.FindGameObjectWithTag("Home").transform;
        // Enter moveToTarget state

    }

    private void GoToWizard()
    {
        Transform wizard = GameObject.FindGameObjectWithTag("Wizard").transform;
        // Enter moveToTarget state
    }

    private void GoToCheese()
    {
        Transform cheese = GameObject.FindGameObjectWithTag("PickUp").transform;
        // Enter moveToTarget state
    }

    // == Methods: interaction ==
    private void InteractWithHome()
    {
        // If we have cheese, should deposit it
        if (hasCheese && !evolved)
        {
            Incapacitated();
            hasCheese = false;
            cheeseCount++;
            Debug.Log("Cheese collected: " + cheeseCount);
        }
    }

    private void InteractWithWizard()
    {
        // Ask the wizard to put the cat to sleep
        if (cheeseCount >= evolveRequirement)
        {
                evolved = true;
                anim.SetInteger("state", 1);
        }
    }

    private void InteractWithCheese()
    {
        // Pick up cheese, if we do not have cheese
        if (!hasCheese && !evolved)
        {
            Incapacitated();
            hasCheese = true;
        }
    }

    private void InteractWithCat()
    {
        if (evolved)
        {
            // You win
            Gameover("You win!");
            Debug.Log("You killed the cat!");
            GameObject cat = GameObject.FindGameObjectWithTag("Enemy");
            cat.GetComponent<Cat>().Scared();
        }
        else
        {
            // You lose
            Gameover("You lose!");
            GameObject cat = GameObject.FindGameObjectWithTag("Enemy");
            cat.GetComponent<Cat>().Attack();
            Debug.Log("The cat found you!");
        }
    }

    private void Gameover(string status)
    {
        // Freeze the gamefeedbackText
        gameover = true;
        // Display canvas
        Text[] text = canvas.GetComponentsInChildren<Text>();
        text[0].text = status;
        text[1].text = status;
        canvas.SetActive(true);

    }

}
