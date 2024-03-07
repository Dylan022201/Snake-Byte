using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*Author: Dylan Janssen
 * Date: Dec 1st, 2021
 * Description: This script handles snake collisions and score for the game.
 */


// this script also manages count and count text
public class CollisionController : MonoBehaviour
{
    public Text countText;
    private int count;

    void Start()
    {
        count = 0;
        countText.text = "Score: " + count.ToString();
    }


    void Update()
    {
        countText.text = "Score: " + count.ToString();

    }
    //colision handling
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Apple")
        {
            count++; // increase score by one
            for (int i = 0; i < 3; i++)
            { 
                //call function from parent object 3 times
                transform.parent.GetComponent<PlayerMovement>().addBodyPart();
            }
        }
    }
    
}
