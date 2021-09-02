using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public float lineOfSight, speed, maxSpeed, attackRange;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        rb = GetComponent<Rigidbody2D>();
    }

    public void ChasePlayer() {
        Vector3 playerPosition = GameObject.Find("PlayerSprite").transform.position;
        Vector3 relativePlayerPosition = transform.InverseTransformPoint(playerPosition);
        RaycastHit2D vision = Physics2D.Raycast(transform.position, relativePlayerPosition, lineOfSight);
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
