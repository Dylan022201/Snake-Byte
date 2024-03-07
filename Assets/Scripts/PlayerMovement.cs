using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Author: Dylan Janssen
 * Date: Dec 1st, 2021
 * Description: This script contains MANY controls. This includes: movement controls, stopping player when paused, adding body parts to the snake, storing gameObjects in a list, and finally,
 * determining distance using raycasts so that the snake will stop before hitting the wall.
 */

public class PlayerMovement : MonoBehaviour
{
    public GameObject bodyPref;
    private Vector3 dir; //used to determine which direction is okay to move
    private Vector3 VectorMove;
    private bool inputPressed;

    private float elapsed;
    
    public List<Transform> BodyParts = new List<Transform>(); // list of body parts
    private Transform currentBP;
    private Transform PrevBP;
    private int beginsize = 5;
    private Vector3 ForwardFollow = new Vector3(0f, 0f, -1f);
    public bool gameover;
    private RaycastHit hit;
    private bool playedAudio;

    public AudioSource gameOverSound;
    private Controller_UI uicntrl;

    // Start is called before the first frame update
    void Start()
    {
        playedAudio = false;
        gameover = false;
        inputPressed = false;
        BodyParts.Add(GameObject.Find("Head").transform); //get head of snack
        BodyParts.Add(GameObject.Find("Body").transform); //get first body part of snake
        elapsed = 0;

        uicntrl = GameObject.Find("Main Camera").GetComponent<Controller_UI>();

        dir = Vector3.forward; //initially move straight up
        VectorMove = new Vector3(0f, 0f, 1f); //initially move straight up
   
        //add extra body parts until 5 sections are on screen
        for (int i = 0; i < beginsize - 1; i++)
        {
            addBodyPart();
        }
        
    }

    void Update()
    {
        //call move function to move player if not paused
        if (uicntrl.pressed == false) { 
            Move(); 
        }
                
        if(gameover == true)
        {
            if (playedAudio == false)
            {
                gameOverSound.Play();
                playedAudio = true;
            }
        }
    }

    void Move()
    {
        if (inputPressed == false && gameover == false) //prevents multiple inputs from being spammed
        { 
            moveDir(); 
        }

        //timer for updating position every one second
        elapsed += Time.deltaTime;
        if (elapsed >= .2f)
        {
            //reset delay
            elapsed = elapsed % .2f;

            //get position of head and new position
            Vector3 Pos = BodyParts[0].position;
            Vector3 newPos = (Pos + VectorMove);


            //get raycast
            rayCastCheck();

            //if raycast detects that you are not at a wall or snake, move
            if (hit.distance != .5 || hit.transform.gameObject.tag == "Apple") {
                //move segments along with the head
                for (int i = BodyParts.Count - 1; i > 0; i--)
                {
                    BodyParts[i].position = BodyParts[i - 1].position;
                }

                //update head last so that it moves properly
                BodyParts[0].position = newPos;

                //allow more input
                inputPressed = false;

            } else if (hit.distance == .5 && hit.transform.gameObject.tag != "Apple")
            {
                //if you are facing the direction of a wall/section at the end of the move delay, game over
                gameover = true;
            }
        }

    }

    void rayCastCheck()
    {
        int layerMask = 1 << 8;

        layerMask = ~layerMask;

        // check raycast
        if (Physics.Raycast(BodyParts[0].position, transform.TransformDirection(VectorMove), out hit, Mathf.Infinity, layerMask))
        {
            //log it for debugging
            Debug.DrawRay(BodyParts[0].position, transform.TransformDirection(VectorMove) * hit.distance, Color.yellow);
            //Debug.Log(hit.distance);
        }
        else
        {
            //log it for debugging
            Debug.DrawRay(BodyParts[0].position, transform.TransformDirection(VectorMove) * 1000, Color.white);
            //Debug.Log("Did not Hit");
        }
    }
    
    //get movement direction
    void moveDir()
    {
        //left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (dir != Vector3.right)
            {
                dir = Vector3.left;
                VectorMove = new Vector3(-1f, 0f, 0f);
                inputPressed = true;
            }
        }
        //right
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (dir != Vector3.left)
            {
                dir = Vector3.right;
                VectorMove = new Vector3(1f, 0f, 0f);
                inputPressed = true;
            }
        }
        //up
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (dir != Vector3.back)
            {
                dir = Vector3.forward;
                VectorMove = new Vector3(0f, 0f, 1f);
                inputPressed = true;
            }
        }
        //down
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (dir != Vector3.forward)
            {
                dir = Vector3.back;
                VectorMove = new Vector3(0f, 0f, -1f);
                inputPressed = true;
            }
        }
    }
    
    //adds body part to list after instantiating it
    public void addBodyPart()
    {
        Transform newPart = (Instantiate(bodyPref, BodyParts[BodyParts.Count - 1].position, BodyParts[BodyParts.Count - 1].rotation) as GameObject).transform;
        newPart.SetParent(transform);
        BodyParts.Add(newPart);
    }
}
