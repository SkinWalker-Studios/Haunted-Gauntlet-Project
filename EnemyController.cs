using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // enemy stats
    public float speed = 1.0f;
    public int maxHealth = 100;
    int currentHealth;

    PlayerController player = other.gameobject.GetComponent<PlayerController>;

    Rigidbody2D rigidbody2D;
 
    Animator animator;

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
        float posX = player.posX;
        float posY = player.posY;

        // calculates the direction towards the player
        float distanceX = position.x - posX;
        float distanceY = position.y - posY;
        float distance = sqrt(pow(distanceX, 2) + pow(distanceY, 2));
        float directionX = distanceX / distance;
        float directionY = distanceY / distance;

        // changes the position
        position.y += Time.deltaTime * speed * directionX;
        position.x += Time.deltaTime * speed * directionY;

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

    void OnCollisionEnter2D(Collision2D other)
    {
        // damage from projectile
        if (other.gameObject.CompareTag("")) // SET TAG
        {
            ChangeHealth(0); // SET VALUE
            Destroy(other.gameObject);
        }
    }

    void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth <= 0)
        {
            Destroy(GameObject)
            player.ChangeScore(0) // SET VALUE
        }
    }
}
