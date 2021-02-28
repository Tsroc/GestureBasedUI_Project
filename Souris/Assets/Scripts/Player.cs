using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Animator anim;
    [SerializeField] GameObject canvas;

    [SerializeField] private float movement = 3f;
    [SerializeField] private int evolveRequirement = 3;
    private float vClamp = 4.5f;
    private float hClamp = 8.0f;
    private int cheeseCount = 0;
    private bool hasCheese = false;
    private bool evolved = false;
    private bool gameover = false;
    private bool hasDestination = false;
    private Vector3 destination;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!gameover)
        {
            // Will have a target if a path 
            if (hasDestination)
            {
                // move to target
                float step = 3f * Time.deltaTime;

                if (gameObject.transform.position.x < destination.x)
                    RotateRight();
                else
                    RotateLeft();

                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, destination, step);
                ClampMovement();

            }

            // manual moveemnt will break path movement
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                ClearDestination();
                MoveRight();
            }
            if (Input.GetAxisRaw("Horizontal") == -1)
            {
                ClearDestination();
                MoveLeft();
            }
            if (Input.GetAxisRaw("Vertical") == 1)
            {
                ClearDestination();
                MoveUp();
            }
            if (Input.GetAxisRaw("Vertical") == -1)
            {
                ClearDestination();
                MoveDown();
            }
        }
    }

    public void SetDestination(string direction)
    {
        switch (direction)
        {
            case "up":
                destination = gameObject.transform.position + new Vector3(0, 10, 0);
                break;
            case "down":
                destination = gameObject.transform.position + new Vector3(0, -10, 0);
                break;
            case "left":
                destination = gameObject.transform.position + new Vector3(-20, 0, 0);
                break;
            case "right":
                destination = gameObject.transform.position + new Vector3(20, 0, 0);
                break;
        }

        this.hasDestination = true;
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        this.hasDestination = true;
    }

    public void ClearDestination()
    {
        this.hasDestination = false;
    }

    private void MoveUp()
    {
        gameObject.transform.Translate(0, this.movement * Time.deltaTime, 0);
        ClampMovement();
    }

    private void MoveDown()
    {
        gameObject.transform.Translate(0, -this.movement * Time.deltaTime, 0);
        ClampMovement();
    }

    private void MoveLeft()
    {
        RotateLeft();
        gameObject.transform.Translate(-this.movement * Time.deltaTime, 0, 0);
        ClampMovement();
    }

    private void MoveRight()
    {
        RotateRight();
        gameObject.transform.Translate(-this.movement * Time.deltaTime, 0, 0);
        ClampMovement();
    }

    private void RotateLeft()
    {
        gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }

    private void RotateRight()
    {
        gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
    }

    private void ClampMovement()
    {
        Vector3 clampedPosition = gameObject.transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -vClamp, vClamp-1);
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -hClamp, hClamp);
        gameObject.transform.position = clampedPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            InteractWithCat();
        }
    }

    // == Methods: interaction ==
    public void InteractWithHome()
    {
        // If we have cheese, should deposit it
        if (hasCheese && !evolved)
        {
            //Incapacitated();
            hasCheese = false;
            cheeseCount++;
            Debug.Log("Depositing the cheese, cheese collected: " + cheeseCount);
        }
    }

    public void InteractWithCheese()
    {
        // Pick up cheese, if we do not have cheese
        if (!hasCheese && !evolved)
        {
            Debug.Log("Taking the cheese...");
            //Incapacitated();
            hasCheese = true;
        }
        else
        {
            Debug.Log("Rat cannot has cheese.");
        }
    }

    private void InteractWithCat()
    {
        if (evolved)
        {
            // You win
            Gameover("You win!");
            GameObject cat = GameObject.FindGameObjectWithTag("Enemy");
            cat.GetComponent<Cat>().Scared();
            Debug.Log("You defeated the cat!");
        }
        else
        {
            // You lose
            Gameover("You lose!");
            GameObject cat = GameObject.FindGameObjectWithTag("Enemy");
            cat.GetComponent<Cat>().Attack();
            Debug.Log("The cat has found you!");
        }
    }

    private void Gameover(string status)
    {
        gameover = true;
        // Display canvas
        Text[] text = canvas.GetComponentsInChildren<Text>();
        text[0].text = status;
        text[1].text = status;
        canvas.SetActive(true);
    }

    public bool IsGameover()
    {
        return gameover;
    }

    public void Evolve()
    {
        // Ask the wizard to put the cat to sleep
        if (cheeseCount >= evolveRequirement)
        {
                evolved = true;
                anim.SetInteger("state", 1);
        }
        else
        {
            Debug.Log("You have not collected enough cheese.");
        }
    }
}
