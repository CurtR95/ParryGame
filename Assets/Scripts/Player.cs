using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Variable Jump Height, speed and maxSpeed - could be overridden in Unity or via other classes.
    public float jumpHeight, speed, maxSpeed, pointerRadius;

    public GameObject arrow;

    private Rigidbody2D rb;
    // Handles if entity is on ground and moving.
    private bool grounded, moving, fireHeld = false;
    private Quaternion aimAngle;
    private Vector2 moveVal, lookVal;
    private Transform pivot;

    // Start is called before the first frame update
    void Start()
    {
        
        PlayerPrefs.SetString("ControlScheme", "Gamepad");
        Debug.Log(PlayerPrefs.GetString("ControlScheme"));
        GetComponent<PlayerInput>().SwitchCurrentControlScheme(PlayerPrefs.GetString("ControlScheme"));
        rb = GetComponent<Rigidbody2D>();

        pivot = transform.Find("PlayerContainer").transform;
        arrow.transform.parent = pivot;
        arrow.transform.position += Vector3.up * pointerRadius;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Find("PlayerContainer").transform.position = transform.position;

        Vector3 horizontal = new Vector2(moveVal.x, 0);
        // Gets direction of the input, if the velocity isn't at max speed yet, increase it in the correct direction.
        if (rb.velocity.x > -maxSpeed && rb.velocity.x < maxSpeed) {
            rb.AddForce(transform.forward + horizontal * speed);
        }

        // Determine if the entity is actually moving, this is defined by the Input not having horizontal movement and the entity being grounded
        if (moveVal.x == 0 && grounded == true) {
            moving = false;
        }
        else {
            moving = true;
        }

        // If the entity isn't moving reduce its x velocity by half every frame, if it is moving but in the air reduce it by a hundredth every frame. Gravity will deal with the vertical velocity.
        if (moving == false) {
            rb.velocity = new Vector2 (rb.velocity.x * 0.5f, rb.velocity.y);
        }
        else if (moving == true && grounded == false) {
            rb.velocity = new Vector2 (rb.velocity.x * 0.99f, rb.velocity.y);
        }

        Vector3 cursorWorldPoint;
        if (GetComponent<PlayerInput>().currentControlScheme == "Gamepad") {
            // Debug.DrawRay(transform.position, new Vector3(lookVal.x,lookVal.y,0), Color.green);
            Vector3 gamepadScreenSpace = transform.position + new Vector3(lookVal.x,lookVal.y,0);
            Debug.DrawRay(transform.position, gamepadScreenSpace - transform.position, Color.green);
            cursorWorldPoint = Camera.main.WorldToScreenPoint(gamepadScreenSpace);
        }
        else {
            Debug.DrawRay(transform.position, Camera.main.ScreenToWorldPoint(lookVal)-transform.position, Color.green);
            cursorWorldPoint = lookVal;
        }
        if (lookVal != new Vector2(0,0)) {
            arrow.GetComponent<Renderer>().enabled = true;
            Vector3 playerVector = Camera.main.WorldToScreenPoint(transform.position);
            playerVector = new Vector3(cursorWorldPoint.x,cursorWorldPoint.y,0) - playerVector;
            float angle = Mathf.Atan2(playerVector.y, playerVector.x) * Mathf.Rad2Deg;
            aimAngle = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            pivot.SetPositionAndRotation(transform.position, aimAngle);
        }
        else {
            arrow.GetComponent<Renderer>().enabled = false;
            aimAngle = new Quaternion(0,0,0,0);
        }

        if (fireHeld) {
            DrawLine(transform.position,transform.position + aimAngle * Vector3.up * 5, Color.green);
        }

    }
    void OnCollisionEnter2D(Collision2D collision) {
        // If the entity is touching the Landscape object, it is grounded.
        if (collision.gameObject.CompareTag("Landscape")) {
            grounded = true;
        }
        // If the entity is touching the Enemy Test object, destroy the player entity.
        if (collision.gameObject.name == "Enemy Test") {
            // Destroy(GameObject.Find("Player"));
        }
    }
    
    void OnCollisionExit2D(Collision2D collision) {
        // If the entity is not touching the Landscape object, it is not grounded.
        if (collision.gameObject.CompareTag("Landscape")) {
            grounded = false;
        }
    }

    public void OnMove(InputValue value) {
        moveVal = value.Get<Vector2>();
    }

    public void OnJump() {
        // If the entity is on the ground and the Jump key is pressed, add force equal to the jump height.
        if(grounded)
        {
            rb.AddForce(new Vector2(0, jumpHeight));
            grounded = false;
        }
    }

    public void OnLook(InputValue value) {
        lookVal = value.Get<Vector2>();
    }

    public void OnFire() {
        fireHeld = !fireHeld;
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f) {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
}
