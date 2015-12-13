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
            rigidbody.AddForce(new Vector2(0,1));
        if (Input.GetKey(KeyCode.S))
            rigidbody.AddForce(new Vector2(0, -1));
        if (Input.GetKey(KeyCode.A))
            rigidbody.AddForce(new Vector2(-1, 0));
        if (Input.GetKey(KeyCode.D))
            rigidbody.AddForce(new Vector2(1, 0));
	}

    void OnTriggerEnter2D(Collider2D other)
    {
            if(other.gameObject.tag == "absorbable")
            {
                float weight = other.gameObject.GetComponent<Attractable>().Weight;
                rigidbody.mass += weight;
                transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            }
           if(other.gameObject.tag == "DirectionPlate")
           {
              // rigidbody.velocity = new velocity

           }
    }
}
