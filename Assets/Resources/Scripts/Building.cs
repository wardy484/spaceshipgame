using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    public bool top = false;
    public float speed = 0.05f;

    private bool Paused = false;

	void Update () {

        if (!Paused)
        {
            transform.position = new Vector2(transform.position.x - speed, transform.position.y);
        }

    }

    void OnBecameInvisible()
    {
        if (top)
        {
            transform.position = new Vector2(11, Random.Range(3.25f, 6));
        }
        else
        {
            transform.position = new Vector2(11, Random.Range(-6, -3));
        }
    }

    public void SetPaused(bool paused)
    {
        Paused = paused;
    }

}

