using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // player stats
    public float speed = 1.0f;
    public int maxHealth = 2000;
    int currentHealth;
    int currentScore;

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

    // world variables
    bool gameOver = false;

    // animation
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    
    public GameObject projectilePrefab; // SET PROJECTILE

    void Start()
    {
        // set base variables
        currentHealth = maxHealth;
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
        }

        // esc to exit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        // movements
        Vector2 position = rigidbody2d.position;
        position.x += speed * horizontal * Time.deltaTime;
        position.y += speed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(position);
        posX = position.x;
        posY = position.y;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // key collectable
        if (other.gameObject.CompareTag("")) // SET TAG
        {
            ChangeKeys(1);
            Destroy(other.gameObject);
        }

        // health collectable
        if (other.gameObject.CompareTag("")) // SET TAG
        {
            ChangeHealth(0); // SET VALUE
            Destroy(other.gameObject);
        }

        // potion collectable
        if (other.gameObject.CompareTag("")) // SET TAG
        {
            ChangePotions(1);
            Destroy(other.gameObject);
        }
        
        // treasure collectable
        if (other.gameObject.CompareTag("")) // SET TAG
        {
            ChangeScore(0); // SET VALUE
            Destroy(other.gameObject);
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        // enemy contact
        if (other.gameObject.CompareTag("")) // SET TAG
        {
            ChangeHealth(0); // SET VALUE
    }

    void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth <= 0 && again == true)
        {
            speed = 0;
            gameOver = true;
        }
    }

    void ChangeScore(int amount)
    {
        currentScore += amount;
    }

    void ChangeKeys(int amount)
    {
        keyAmount += amount;
    }

    void ChangePotion(int amount)
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
