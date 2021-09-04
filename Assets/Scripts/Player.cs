using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Variable Jump Height, speed and maxSpeed - could be overridden in Unity or via other classes.
    public float jumpHeight, speed, maxSpeed, pointerRadius, jumpCooldown, degreesUnableToJump;
    public GameObject arrow;
    private Rigidbody2D rb;
    // Handles if entity is on ground and moving.
    private bool grounded, moving, attachedToWall, fireHeld = false;
    private Quaternion aimAngle;
    private Vector2 moveVal, lookVal;
    private Transform pivot;
    private float lastJump;
    private AudioSource[] audioData;

    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponents<AudioSource>();
        PlayerPrefs.SetString("ControlScheme", "Keyboard&Mouse");
        // PlayerPrefs.SetString("ControlScheme", "Gamepad");
        Debug.Log(PlayerPrefs.GetString("ControlScheme"));
        GetComponent<PlayerInput>().SwitchCurrentControlScheme(PlayerPrefs.GetString("ControlScheme"));
        rb = GetComponent<Rigidbody2D>();

        pivot = transform.Find("PlayerContainer").transform;
        arrow.transform.parent = pivot;
        arrow.transform.position += Vector3.up * pointerRadius;
    }

    // Update is called once per frame
    void Update() {
        SyncContainer();
        HorizontalMovementController();
        PointerController();
        AttackController();
        GravityController();
    }

    void GravityController() {
        if (attachedToWall) {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
        else {
            rb.gravityScale = 1;
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        // If the entity is touching the Landscape object, it is grounded.
        // Vector2 direction = collision.GetContact(0).normal;
        // if( direction.x == 1 ) print("right");
        // if( direction.x == -1 ) print("left");
        // if( direction.y == 1 ) print("up");
        // if( direction.y == -1 ) print("down");
        if ((collision.gameObject.CompareTag("Landscape") || collision.gameObject.CompareTag("UnstickableLandscape")) && collision.GetContact(0).normal.y == 1) {
            grounded = true;
        }
        else if (collision.gameObject.CompareTag("Landscape") && collision.GetContact(0).normal.x != 0) {
            attachedToWall = true;
        }
        // If the entity is touching the Enemy Test object, destroy the player entity.
        if (collision.gameObject.name == "Enemy Test") {
            // Destroy(GameObject.Find("Player"));
        }
    }
    
    void OnCollisionExit2D(Collision2D collision) {
        // If the entity is not touching the Landscape object, it is not grounded.
        if (collision.gameObject.CompareTag("Landscape") || collision.gameObject.CompareTag("UnstickableLandscape")) {
            grounded = false;
            attachedToWall = false;
        }
    }

    void SyncContainer () {
        transform.Find("PlayerContainer").transform.position = transform.position;
    }

    void HorizontalMovementController () {
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
    }

    void PointerController () {
        Vector3 cursorWorldPoint;
        if (GetComponent<PlayerInput>().currentControlScheme == "Gamepad") {
            Vector3 gamepadScreenSpace = transform.position + new Vector3(lookVal.x,lookVal.y,0);
            cursorWorldPoint = Camera.main.WorldToScreenPoint(gamepadScreenSpace);
        }
        else {
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
    }

    void AttackController () {
        if (fireHeld) {
            DrawLine(transform.position,transform.position + aimAngle * Vector3.up * 5, Color.green);
        }
    }

    public void OnMove(InputValue value) {
        moveVal = value.Get<Vector2>();
    }

    public void OnJump() {
        // If the entity is on the ground and the Jump key is pressed, add force equal to the jump height.
        float time = Time.time;
        if (time - lastJump > jumpCooldown) {
            if(grounded || attachedToWall) {
                audioData[0].Play(0);
                float jumpAngle = aimAngle.eulerAngles.z+90;
                if (jumpAngle > 360) {
                    jumpAngle -= 360;
                }
                if(grounded) {
                    float exclusionDegree = jumpAngle+degreesUnableToJump/2;
                    if (exclusionDegree > (360-degreesUnableToJump)) {
                        jumpAngle = 90;
                    }
                }
                else if (attachedToWall) {
                    //TODO add wall jump
                }
                float forceModifier = Mathf.Abs((jumpAngle - 90)/45);
                float jumpForce = jumpHeight * Mathf.Clamp(forceModifier,1,5);
                // BUG not moving in the direction properly. Its kinda like a shove left/right then up?
                rb.AddForce(new Vector2(Mathf.Cos(jumpAngle * Mathf.Deg2Rad), Mathf.Sin(jumpAngle * Mathf.Deg2Rad)) * jumpForce);
                grounded = false;
                attachedToWall = false;
                lastJump = time;
            }
        }
    }

    public void OnLook(InputValue value) {
        lookVal = value.Get<Vector2>();
    }

    public void OnFire() {
        fireHeld = !fireHeld;
        if (fireHeld) {
            // audioData[1].Play(0);
        }
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
