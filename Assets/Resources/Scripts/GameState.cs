using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameState : MonoBehaviour {

    public Building[] buildings;
    public Player player;
    public GameObject Message;
    private Text MessageText;

    private State state = State.Start; 

	// Use this for initialization
	void Start () {
        MessageText = Message.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {

        // Restart Game on any key if the player fucked up.
        switch(state)
        {
            case State.Start:
                GameStart();
                break;
            case State.InGame:
                InGame();
                break;
            case State.Paused:
                Paused();
                break;
            case State.Dead:
                Dead();
                break;             
        }

     }

    private void GameStart()
    {
        PauseGame("Click To Start!");
        Paused();
    }

    private void InGame()
    {
        CheckForDeath();

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame("Game Paused, Click to continue...");
    }

    private void Paused()
    {
        if (Input.anyKeyDown)
        {
            UnPause();
        }
    }

    private void Dead()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void CheckForDeath()
    {
        if (player.dead)
        {
            state = State.Dead;
            SetBuildingState(true);
            Message.SetActive(true);
            MessageText.text = "Get Reckt Mate! Press Owt To Start Again...";
        }
    }

    private void PauseGame(string message)
    {
        state = State.Paused;
        player.Pause();
        SetBuildingState(true);
        MessageText.text = message;
        Message.SetActive(true);
    }

    private void UnPause()
    {
        state = State.InGame;
        player.Resume();
        SetBuildingState(false);
        Message.SetActive(false);
    }

    private void SetBuildingState(bool Paused)
    {
        foreach (Building building in buildings)
        {
            building.SetPaused(Paused);
        }
    }
}
