using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameState : MonoBehaviour {

    public List<Building> buildings = new List<Building>();
    public Player player;
    public GameObject TheCanvas;
    public Text ScoreText;

    private ScoreHolder scoreHolder;
    private State state = State.Start;
    private int Score;
    private float lastUpdate;
    private float lastSpawn;
    private GameObject Menu;
    private float timePaused;
    
	// Use this for initialization
	void Start () {

        GameObject obj = GameObject.FindGameObjectWithTag("GameController");

        if (obj == null)
        {
            obj = Instantiate(Resources.Load("Prefabs/ScoreHolder"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        }

        scoreHolder = obj.GetComponent<ScoreHolder>();

        ScoreText.text = "High Score: " + scoreHolder.HighScore.ToString();

        if (scoreHolder.hasPlayed == false)
        {
            PauseGame(State.Start);
        }
        else
        {
            state = State.InGame;
            player.Resume();
            SetBuildingState(false);
            Time.timeScale = 1;
        }
        
    }

    private void CreateMenu(Object menu)
    {
        Menu = Instantiate(menu, new Vector3(-1.907349e-06f, 0, 0), Quaternion.identity) as GameObject;
        Menu.transform.SetParent(TheCanvas.transform, false);

        Button[] btns = Menu.GetComponentsInChildren<Button>();
        Text text;

        if (state == State.Dead)
        {
            Text[] texts = Menu.GetComponentsInChildren<Text>();

            foreach(Text txt in texts)
            {
                if (txt.name == "ScoreValue")
                    txt.text = Score.ToString();
            }

        }

        foreach (Button btn in btns)
        {
            switch (btn.name)
            {
                case "Resume":
                    btn.onClick.AddListener(UnPause);
                    break;
                case "RestartBtn":
                    if (state == State.Dead || state == State.Paused)
                    {
                        text = btn.GetComponentInChildren<Text>();
                        text.text = "Restart";
                        btn.onClick.AddListener(RestartGame);
                    }
                    else
                    {
                        btn.onClick.AddListener(UnPause);

                    }

                    break;
                case "ExitBtn":
                    btn.onClick.AddListener(Quit);
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update () {

        // Restart Game on any key if the player fucked up.
        switch(state)
        {
            case State.InGame:
                InGame();
                break;
        }

     }

    private void InGame()
    {
        if (CheckForDeath())
            return;

        if (Time.time - lastSpawn >= 1.7f)
        {
            float x = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, 0.0f)).y + 6f;
            float y = Random.Range(-2.5f, -5.3f);

            GameObject NewBuilding = Instantiate(Resources.Load("Prefabs/Buildings"), new Vector3(x, y, 0), Quaternion.identity) as GameObject;
            buildings.Add(NewBuilding.GetComponent<Building>());
            lastSpawn = Time.time;
            timePaused = 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame(State.Paused);

        if (Time.time - lastUpdate >= 1f)
        {
            Score += 1;
            lastUpdate = Time.time;
        }

        ScoreText.text = Score.ToString();
    }

    private void RestartGame()
    {
        scoreHolder.hasPlayed = true;
        SceneManager.LoadScene(SceneManager.GetSceneByName("Main").name);
    }

    private bool CheckForDeath()
    {
        if (player.dead)
        {
            state = State.Dead;
            SetBuildingState(true);
            CreateMenu(Resources.Load("Prefabs/DeadMenu"));
            ScoreText.text = "";

            if (Score > scoreHolder.HighScore)
            {
                scoreHolder.HighScore = Score;
            }

            Debug.Log(scoreHolder.HighScore);
            ScoreText.text = "High Score: " + scoreHolder.HighScore;
        }

        return player.dead;
    }

    private void PauseGame(State newState)
    {
        state = newState;
        Time.timeScale = 0;

        if (Menu == null)
        {
            if (state == State.Paused)
                CreateMenu(Resources.Load("Prefabs/PauseMenu"));
            else
                CreateMenu(Resources.Load("Prefabs/StartMenu"));
        }

        player.Pause();
        SetBuildingState(true);
    }

    public void UnPause()
    {
        Destroy(Menu);
        state = State.InGame;
        player.Resume();
        SetBuildingState(false);
        Time.timeScale = 1;
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        audio.Play(44100);

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

    public void Quit()
    {
        Application.Quit();
    }

}
