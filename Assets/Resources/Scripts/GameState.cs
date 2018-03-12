using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameState : MonoBehaviour {

    public List<Building> buildings = new List<Building>();
    public Player player;
    public GameObject Message;
    public Text ScoreText;

    private Text MessageText;
    private State state = State.Start;
    private int Score;
    private float lastUpdate;
    private float lastSpawn;

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

        if (Time.time - lastSpawn >= 1.7f)
        { 
            Debug.Log(Time.time - lastUpdate);
            float x = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, 0.0f)).y + 6f;
            float y = Random.Range(-2.5f, -5.3f);

            GameObject NewBuilding = Instantiate(Resources.Load("Prefabs/Buildings"), new Vector3(x, y, 0), Quaternion.identity) as GameObject;
            buildings.Add(NewBuilding.GetComponent<Building>());
            lastSpawn = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame("Game Paused, Click to continue...");

        if (Time.time - lastUpdate >= 1f)
        {
            Score += 1;
            lastUpdate = Time.time;
        }

        ScoreText.text = Score.ToString();
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
        if (buildings.Count > 0)
        {
            foreach (Building building in buildings)
            {
                if (building != null) 
                    building.SetPaused(Paused);
            }

        }
    }
}
