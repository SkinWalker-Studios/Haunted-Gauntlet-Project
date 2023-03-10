using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour
{
    // enemy stats
    public float speed = 3.0f;
    public int maxHealth = 100;
    int currentHealth;

    Rigidbody2D rigidbody2D;

    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {            
        Vector2 position = rigidbody2D.position;

        // finds the player's position
        float positX;
        positX = FindObjectOfType<PlayerController>().posX;
        float positY;
        positY = FindObjectOfType<PlayerController>().posX;

        // calculates the direction towards the player
        float distanceX = -position.x + positX;
        float distanceY = -position.y + positY;
        float distance = Mathf.Sqrt(Mathf.Pow(distanceX, 2) + Mathf.Pow(distanceY, 2));
        float directionX = distanceX / distance;
        float directionY = distanceY / distance;

        // changes the position
        position.y += Time.deltaTime * speed * directionY;
        position.x += Time.deltaTime * speed * directionX;

        // creates a new vector
        Vector2 move = new Vector2(directionX, directionY);

        // sets the direction
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }
        // adds animation to the direction
        animator.SetFloat("Move X", moveDirection.x);
        animator.SetFloat("Move Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        // updates position
        rigidbody2D.MovePosition(position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile")) // SET TAG
        {
            ChangeHealth(-50); // SET VALUE
            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // player contact
        if (other.gameObject.CompareTag("Player")) // SET TAG
        {
            ChangeHealth(-50); // SET VALUE
        }
    }

    void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth <= 0)
        {
            Destroy(GameObject);
            FindObjectOfType<PlayerController>().ChangeScore(100); // SET VALUE
        }
    }
}
