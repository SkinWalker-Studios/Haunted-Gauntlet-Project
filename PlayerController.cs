using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // player stats
    public float speed = 1.0f;
    public int maxHealth = 2000;
    float count = 0;
    int currentHealth;
    int currentScore;
    float currentSpeed;

    // collectables
    public int keyAmount;
    public int potionAmount;

    // tracking player movement & position
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    public float posX;
    public float posY;
    
    // audio
    public AudioSource musicsource;
    AudioSource audioSource;
    // INSERT CLIPS

    // animation
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    public GameObject projectilePrefab; // SET PROJECTILE

    void Start()
    {
    
        // set base variables
        currentHealth = maxHealth;
        currentSpeed = speed;
        currentScore = 0;
        keyAmount = 0;
        potionAmount = 0; 
        posX = 0; // SET VALUE
        posY = 0; // SET VALUE
        
        // background music
        audioSource = GetComponent<AudioSource>();
        musicSource.clip = backgroundMusic; // SET BACKGROUND MUSIC
        musicSource.Play();
     
    }
    
    void Update()
    {
        // movement inputs
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // animation states
        Vector2 move = new Vector2(horizontal, vertical);
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // firing projectiles
        if (Input.GetKeyDown(KeyCode.X))
        {
            Launch();
            currentSpeed = 0;
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            currentSpeed = speed;
        }

        // esc to exit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // health decay
        if (count >= 60)
        {
            count = 0;
            ChangeHealth(-1); // SET VALUE
        }
        else
        {
            count *= Time.deltaTime;
        }
    }
    void FixedUpdate()
    {
        // movements
        Vector2 position = rigidbody2d.position;
        position.x += currentSpeed * horizontal * Time.deltaTime;
        position.y += currentSpeed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(position);
        posX = position.x;
        posY = position.y;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // key collectable
        if (other.gameObject.CompareTag("Key")) // SET TAG
        {
            ChangeKeys(1);
            Destroy(other.gameObject);
        }
        // health collectable
        if (other.gameObject.CompareTag("Health")) // SET TAG
        {
            ChangeHealth(150); // SET VALUE
            Destroy(other.gameObject);
        }
        // potion collectable
        if (other.gameObject.CompareTag("Potion")) // SET TAG
        {
            ChangePotions(1);
            Destroy(other.gameObject);
        }
        // treasure collectable
        if (other.gameObject.CompareTag("Treasure")) // SET TAG
        {
            ChangeScore(200); // SET VALUE
            Destroy(other.gameObject);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        // enemy contact
        if (other.gameObject.CompareTag("Enemy")) // SET TAG
        {
            ChangeHealth(-50); // SET VALUE
        }
    }
    void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth <= 0)
        {
            currentSpeed = 0;
        }
    }
    public void ChangeScore(int amount)
    {
        currentScore += amount;
    }
    void ChangeKeys(int amount)
    {
        keyAmount += amount;
    }
    void ChangePotions(int amount)
    {
        potionAmount += amount;
    }
    
    // fires a projectile
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        animator.SetTrigger("Launch");
        PlaySound(throwSound); // SET LAUNCH CLIP
    }
    
    // plays a clip
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
