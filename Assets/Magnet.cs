using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour {

    public Rigidbody2D rigidbody;
    public Rigidbody2D camera; 
	void Start () 
    {
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Camera.main.GetComponent<Rigidbody2D>();
	}
	void Update () 
    {
        if (Input.GetKey(KeyCode.W))
            rigidbody.AddForce(new Vector2(0,100));
        if (Input.GetKey(KeyCode.S))
            rigidbody.AddForce(new Vector2(0, -100));
        if (Input.GetKey(KeyCode.A))
            rigidbody.AddForce(new Vector2(-100, 0));
        if (Input.GetKey(KeyCode.D))
            rigidbody.AddForce(new Vector2(100, 0));
	}

    void OnCollisionEnter2D(Collision2D other)
    {
            if(other.gameObject.tag == "absorbable")
            {
                float weight = other.gameObject.GetComponent<Attractable>().Weight;
                Destroy(other.gameObject);
                rigidbody.mass += weight;
                transform.localScale += new Vector3(0.01f, 0.01f,0f);
            }
    }
}
