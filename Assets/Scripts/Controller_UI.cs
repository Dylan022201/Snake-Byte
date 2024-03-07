using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/*
 * Author: Dylan Janssen
 * Date: Dec 1st, 2021
 * Description: This script contains UI controls, such as controlling text, hiding UI, and pausing the game.
 */
public class Controller_UI : MonoBehaviour
{

    public Button restartButton;
    public Button ExitGame;
    private bool gameover;
    public Text winText;
    private PlayerMovement Plyr;
    public bool pressed;
    public Text PauseText;

    void Start()
    {
        pressed = false;
        PauseText.gameObject.SetActive(false);
        ExitGame.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        winText.text = "";
        Plyr = GameObject.Find("Snake").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if(Plyr.gameover == true)
        {
            //just in case this causes an issue, hide pause text
            PauseText.gameObject.SetActive(false);
            ExitGame.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            winText.text = "Game Over!";
        }

        //pause button
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //make sure you havent won the game or lost already, to avoid conflict
            if (winText.text != "Game Over!")
            {
                //check if you have already pressed ESC or not, then hide or show buttons
                if (pressed == false)
                {
                    ExitGame.gameObject.SetActive(true);
                    restartButton.gameObject.SetActive(true);
                    PauseText.gameObject.SetActive(true);
                    pressed = true;
                }
                else
                {
                    ExitGame.gameObject.SetActive(false);
                    restartButton.gameObject.SetActive(false);
                    PauseText.gameObject.SetActive(false);
                    pressed = false;
                }
            }
        }

    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene"); //restart the game
    }

    public void Exit()
    {
        Application.Quit(); //exit application
    }
}
