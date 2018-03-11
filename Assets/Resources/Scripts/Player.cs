using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float thrust = 20;
    public Rigidbody2D ship;
    public SpriteRenderer Explosion;

    bool dead = false;

    GameObject DeadText;

	// Use this for initialization
	void Start () {
        ship = GetComponent<Rigidbody2D>();
        Explosion.sortingOrder = -1;

        DeadText = GameObject.Find("DeadText");
        DeadText.SetActive(false);

    }

    // Update is called once per frame
    void FixedUpdate () {
        if (dead)
        {
             if(Input.anyKeyDown)
             {
                Application.LoadLevel(Application.loadedLevel);
                 
             }

            return;
        }

		if (Input.GetMouseButtonDown(0) && thrust < 24)
        {
            thrust = thrust + 1;
            ship.AddForce(transform.up * thrust);
        }

        if (thrust > 20)
            thrust = thrust - 5;
	}

    void OnBecameInvisible()
    {
        Die();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Die();
    }

    private void Die()
    {
        Explosion.sortingOrder = 9;
        dead = true;
        thrust = 0;
        ship.bodyType = RigidbodyType2D.Static;

        var buildings = GameObject.FindGameObjectsWithTag("Buidling");

        foreach (var building in buildings)
        {
            Building build = building.GetComponent<Building>();
            build.speed = 0;
        }

        DeadText.SetActive(true);
    }
}
