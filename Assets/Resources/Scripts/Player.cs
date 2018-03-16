using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float thrust;
    public Rigidbody2D ship;
    public SpriteRenderer Explosion;

    bool paused = false;
    public bool dead = false;
    private float lastY;

	// Use this for initialization
	void Start () {
        ship = GetComponent<Rigidbody2D>();
        Explosion.sortingOrder = -1;
    }

    // Update is called once per frame
    void Update () {

		if (Input.GetMouseButtonDown(0) && !paused)
        {
            ship.velocity = Vector2.zero;
            ship.AddForce(new Vector2(0, thrust));
        }

        if (ship.position.y < lastY)
        {
            ship.gameObject.transform.rotation = 
                Quaternion.RotateTowards(ship.gameObject.transform.rotation, Quaternion.Euler(0, 0, -61f), 1.5f);
        }
        else if (ship.position.y > lastY)
        {
            ship.gameObject.transform.rotation =
                Quaternion.RotateTowards(ship.gameObject.transform.rotation, Quaternion.Euler(0, 0, 6.86f), 10f);
        }

        lastY = ship.position.y;

    }

    void OnBecameInvisible()
    {
        Die();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Die();
    }

    public void Die()
    {
        Explosion.sortingOrder = 9;
        Pause();
        dead = true;
        PlayExplosion();

    }

    private void PlayExplosion()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        audio.Play(44100);
    }

    public void Revive()
    {
        Explosion.sortingOrder = -1;
        dead = false;
        Resume();
    }

    public void Pause()
    {
        paused = true;
        ship.bodyType = RigidbodyType2D.Static;       
    }

    public void Resume()
    {
        paused = false;
        ship.bodyType = RigidbodyType2D.Dynamic;
    }
}
