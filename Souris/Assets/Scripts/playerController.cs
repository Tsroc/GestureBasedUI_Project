using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    private Rigidbody2D rb;
    // These numbers will be changed once voice is implemented.
    private float vMovement = 0.05f;
    private float hMovement = 0.05f;
    private float vClamp = 4.5f;
    private float hClamp = 8.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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

    private void MoveUp()
    {
        transform.Translate(0, vMovement, 0);
        ClampMovement();
    }

    private void MoveDown()
    {
        transform.Translate(0, -vMovement, 0);
        ClampMovement();
    }

    private void MoveLeft()
    {
        transform.rotation = new Quaternion(0f, 180f, 0f, 0f);

        transform.Translate(-hMovement, 0, 0);
        ClampMovement();
    }

    private void MoveRight()
    {
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        transform.Translate(-hMovement, 0, 0);
        ClampMovement();
    }

    private void ClampMovement()
    {
        // https://answers.unity.com/questions/925199/restricting-movement-with-mathfclamp.html

        // initially, the temporary vector should equal the player's position
        Vector3 clampedPosition = transform.position;
        // Now we can manipulte it to clamp the y element
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -vClamp, vClamp);
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -hClamp, hClamp);
        // re-assigning the transform's position will clamp it
        transform.position = clampedPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Trigger: Enemy.");
        }
        else if (collision.tag == "Wizard")
        {
            Debug.Log("Trigger: Wizard.");
        }
        else if (collision.tag == "PickUp")
        {
            Debug.Log("Trigger: PickUp.");
        }
        else if (collision.tag == "Home")
        {
            Debug.Log("Trigger: Home.");
        }
    }

}
