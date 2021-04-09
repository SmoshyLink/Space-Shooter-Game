using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is attached to all 4 lasers and its what sets their direction, speed, and destroys them

public class laser_script : MonoBehaviour
{

    public float moveSpeed = 10;
    public Rigidbody2D rb;
    private Vector2 bounds;

    // Start is called before the first frame update
    void Start()
    {
        // Gets rigid body of laser object
        rb = GetComponent<Rigidbody2D>();
        // Sets velocity of rigid body to -moveSpeed on the X direction
        // because it's moving left (left is negative X, right is positive X)
        rb.velocity = new Vector2(-moveSpeed, 0);
        // Gets the size of the screen 
        bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        // Destroys objects when they leave the screen
        // If the position of the object is less than the left bounds of X,
        // Destroy it
        if (transform.position.x < -bounds.x)
            Destroy(gameObject);
    }
}
