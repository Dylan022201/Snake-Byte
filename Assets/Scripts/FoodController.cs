using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Dylan Janssen
 * Date: Dec 1st, 2021
 * Description: This script contains controls related to the apple, such as: changing location, checking new locations, collision handling, etc.
 */
public class FoodController : MonoBehaviour
{
    public AudioSource eatSound;
    private bool spawned;
    private Vector3 headPos;
    public Transform snakeHead;

    void Start()
    {
        RandomSpawn(); //initial random spawn
    }

    private void RandomSpawn()
    {
        spawned = false;
        do
        {
            
            float x = Random.Range(-10f, 10f);
            float z = Random.Range(-10f, 10f);
            float y = .5f; //ensures it spawns above the floor
            

            //the following if statements round it to the nearest whole number, then add .5 to that to make sure it always is at
            //the center of a cell. I used Random.Range to determind the center 4 cells
            if (x < 0f)
            {
                x = Mathf.Round(x) + .5f;
            }
            if (x > 0f)
            {
                x = Mathf.Round(x) - .5f;
            }
            if (z > 0f)
            {
                z = Mathf.Round(z) - .5f;
            }
            if (z < 0f)
            {
                z = Mathf.Round(z) + .5f;
            }
            if (x == 0f)
            {
                int i = Random.Range(1, 2);
                switch (i)
                {
                    case 1:
                        x += .5f;
                        break;
                    case 2:
                        x -= .5f;
                        break;
                }
            }
            if (z == 0f)
            {
                int i = Random.Range(1, 2);
                switch (i)
                {
                    case 1:
                        z += .5f;
                        break;
                    case 2:
                        z -= .5f;
                        break;
                }
            }


            //this following bit took me a REALLY long time to figure out why .magnitude was returning absurd numbers, and it was because I was using this.transform.position instead of transform.position
            //that being said, I have tried many (5 games) times to get an apple to appear within 5 units of the player, and have not succeeded in doing so.
            //new location

            //Debug.Log("Head Position: " + snakeHead.position); 
            Vector3 newPos = new Vector3(x, y, z);
            float m = (newPos - snakeHead.position).magnitude;

            if (m < 5f)
            {
                continue; //redo loop if distance < 5
            } else if (m >= 5f)
            {
                transform.position = newPos;
                //Debug.Log(m);
                //Debug.Log("Apple Position: " + transform.position);
                spawned = true;
                //if distance >= 5 "spawn" there
            }
            
        } while (!spawned);
    }



    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Snake")
        {
            //if you eat the apple play sound then change location
            eatSound.Play(); 
            RandomSpawn();
        }
        else if (col.gameObject.tag == "Body")
        {
            //change location if it spawns inside of the body
            RandomSpawn(); 
        }
    }
}
