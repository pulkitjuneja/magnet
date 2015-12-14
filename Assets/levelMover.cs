using UnityEngine;
using System.Collections;

public class levelMover : MonoBehaviour {

    Rigidbody2D rigidbody;
    float limiter = 0.1f;
	void Start ()
    {
        rigidbody = GetComponent<Rigidbody2D>();
	}
	
	void Update() 
    {
       // rigidbody.MovePosition(transform.position + new Vector3(0,generator.gamespeed,0));
	}
}
