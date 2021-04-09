using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script is attached to the spaceship and controls its movement controls and game logic for when objects collide

public class spaceship_control : MonoBehaviour
{

    public Rigidbody2D rb; 
    public float moveSpeed = 5;
    private Vector2 bounds;
    private float spaceShipHeight;

    private AudioSource hitAudio;
    public AudioClip laserHit;
    public AudioClip healthHit;

    private int health = 100;

    public Text healthDisplay;
    public Text gameOverDisplay;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        // Getting the size of the screen
        bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        // Getting the height of the spaceship through its sprite
        spaceShipHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        // Getting rigid body of the spaceship
        rb = GetComponent<Rigidbody2D>();
        // Getting audio source component of the spaceship
        hitAudio = GetComponent<AudioSource>();
        // Getting animator of the spaceship
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // This code prevents the spaceship from going outside the screen on the
        // It restricts the spaceship on the y bounds
        Vector3 viewPos = transform.position;
        viewPos.y = Mathf.Clamp(viewPos.y, -bounds.y + spaceShipHeight, bounds.y - spaceShipHeight);
        transform.position = viewPos;

        // Allows spaceship to be moved vertically with arrow keys
        float moveDirection = Input.GetAxisRaw("Vertical");

        // Sets the y (vertical) velocity of the rigid body to moveSpeed
        // Makes x velociy 0
        rb.velocity = new Vector2(0, moveDirection * moveSpeed);

        // Set the speed value of the animation to moveDirection 
        // Makes animation go from idle to moving when moveDirection > 0.01 
        // Use the abs value of moveDirection because we always want a positive value and when moving down moveDirection is negative 
        anim.SetFloat("speed", Mathf.Abs(moveDirection));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Gets the name of the sprite that collided with the spaceship
        string laserName = other.GetComponent<SpriteRenderer>().sprite.name;

        // Runs specifc code depending on what type of laser or health item collided with the spaceship
        // If its a laser, subtracts from health and sets audio source to laser hit audio clip
        // If its a health item, adds to the health and sets audio source to health restore audio clip
        if (laserName.Equals("laser1"))
        {
            health -= 5;
            hitAudio.clip = laserHit;
        }

        else if (laserName.Equals("laser2"))
        {
            health -= 10;
            hitAudio.clip = laserHit;
        }

        else if (laserName.Equals("laser3"))
        {
            health -= 20;
            hitAudio.clip = laserHit;
        }
        else if (laserName.Equals("health"))
        {
            // Makes sure health does not go above 100
            // If it goes above 100, resets it to 100
            if(health < 100)
                health += 10;
            if (health > 100)
                health = 100;
            hitAudio.clip = healthHit;
        }

        // Code for when player loses
        // Sets animator speed to zero (stops animation on spaceship)
        // Displays Game Over on the screen
        if (health <= 0)
        {
            health = 0;
            anim.speed = 0f;
            gameOverDisplay.text = "Game Over";
        }
        
        // Plays the audio source which is set to either laser hit or health
        hitAudio.Play();
        // Updates the health text on the screen
        healthDisplay.text = "  Health: " + health + "/100";
        // Destroys all objects that collided with spaceship
        Destroy(other.gameObject);
    }
}
