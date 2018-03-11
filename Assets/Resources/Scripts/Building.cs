using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    //Rigidbody2D rb;
    public bool top = false;
    public float speed = 0.05f;

	// Use this for initialization
	void Start () {
      //  rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector2(transform.position.x - speed, transform.position.y);
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

}

