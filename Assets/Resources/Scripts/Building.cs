using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    public bool top = false;
    public float speed = 0.1f;
    public Camera cam;

    private bool Paused = false;

    

	void Update () {

        if (!Paused)
        {
            transform.position = new Vector2(transform.position.x - speed, transform.position.y);
        }

    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
       // transform.position = new Vector2(Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, 0.0f)).y + 6f, Random.Range(-2.5f, -5.3f));
    }

    public void SetPaused(bool paused)
    {
        Paused = paused;
    }

}

