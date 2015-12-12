using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour {

    public Rigidbody2D rigidbody;
	void Start () 
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(0,-10f);
	}
	void Update () {
	
	}
}
