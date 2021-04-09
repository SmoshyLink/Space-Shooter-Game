using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script is attached to the main camera and its what spawns their prefabs with different frequency depending on which laser it is


public class spawn_lasers : MonoBehaviour
{
    public Sprite[] lasers;
    List<GameObject> laserObjects;
    public GameObject laserPrefab;
    private Vector2 bounds;

    public Text health;

    // Start is called before the first frame update
    void Start()
    {
        // Gets the size of the screen
        bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        // Runs function that starts coroutines
        Play();
    }

    public void Pause()
    {
        // Stops all running coroutines
        // This means that objects will stop spawning
        StopAllCoroutines();


        // **NOTE**
        /*for(int i = 0; i < laserObjects.Count; i++)
        {
            if(laserObjects[i] != null)
            {
                laserObjects[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }*/
    }

    public void Play()
    {
        // Starts the coroutine that will start the coroutines for spawning objects
        StartCoroutine(playAll());

        // **NOTE**
        /*for (int i = 0; i < laserObjects.Count; i++)
        {
            if (laserObjects[i] != null)
            {
                laserObjects[i].GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 0);
            }
        }*/
    }

    private IEnumerator playAll()
    {
        // Sets the frequency of spawn time for each laser type
        // The higher the number, the less the spawn frequency 
        float[] rarity = { 0.5f, 1.2f, 1.7f, 15f };

        // for loop that starts the coroutine function for each specific laser type and its specific spawn frequency
        for (int i = 0; i < rarity.Length; i++)
        {
            // Sets spawnTime to corresponding frequency in rarity array
            float spawnTime = rarity[i];
            // starts coroutine for specific object by calling spawn with the objects ID i and time spawnTime
            StartCoroutine(spawn(i, spawnTime));
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator spawn(int laser, float time)
    {
        // Keeps creating laser object every "time" amount of time
        while (true)
        {
            // When player dies (health == 0), runs Pause() which stops all coroutines
            if (health.text == "  Health: 0/100")
                Pause();

            // Instantiates public laser prefab set on Unity to GameObject g
            GameObject g = Instantiate(laserPrefab);

            //laserObjects.Add(g); // **NOTE**

            // Sets position of new laser on the X value away from the screen
            // Sets position of new laser on the Y value to a random point between the upper and lower bounds of the screen
            // Also adds or subtracts 0.5f so that lasers won't be created right on the bounds
            g.transform.position = new Vector2(bounds.x * 2, Random.Range(-bounds.y + 0.5f, bounds.y - 0.5f));
            
            // Gets the sprite of the current g and sets it to the corresponing sprite in the lasers array
            // laser and time sent from playAll() corresponds to one specific laser object
            g.GetComponent<SpriteRenderer>().sprite = lasers[laser];
            
            // Runs the coroutine of specific laser after time amount of seconds
            yield return new WaitForSeconds(time);
        }
    }

    // Commented out code (tagged with **NOTE**) does not work
    // It's supposed to stop/start the movement of all running coroutines
    // by adding them to list of GameObjects laserObjects then looping through the 
    // list and setting their rigid body velocities to zero
    // To start them again the same thing is done but velocities are set to -10 on the X

}
