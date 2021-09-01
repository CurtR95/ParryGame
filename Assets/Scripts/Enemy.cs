using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float lineOfSight, speed, maxSpeed, attackRange;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 playerPosition = GameObject.Find("PlayerSprite").transform.position;
        
        Vector3 relativePlayerPosition = transform.InverseTransformPoint(playerPosition);
        
        RaycastHit2D vision = Physics2D.Raycast(transform.position, relativePlayerPosition, lineOfSight);
        Debug.DrawRay(transform.position, new Vector3(-lineOfSight,0,0), Color.green);
        Debug.DrawRay(transform.position, new Vector3(lineOfSight,0,0), Color.green);
        Debug.DrawRay(transform.position, new Vector3(-attackRange,0,0), Color.red);
        Debug.DrawRay(transform.position, new Vector3(attackRange,0,0), Color.red);

        if (vision.collider != null && vision.collider.name == "PlayerSprite") {
            if (vision.distance < attackRange) {
                rb.velocity = new Vector2 (rb.velocity.x * 0.5f, rb.velocity.y);
            }
            else if (rb.velocity.x > -maxSpeed && rb.velocity.x < maxSpeed) {
                rb.AddForce(transform.forward + new Vector3 (Mathf.Clamp(relativePlayerPosition.x,-1,1),0,0) * speed);
            }
        }
        else {
            rb.velocity = new Vector2 (rb.velocity.x * 0.5f, rb.velocity.y);
        }
    }
}
