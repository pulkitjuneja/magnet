using UnityEngine;
using System.Collections;

public class ChangeSpeed : MonoBehaviour {

    Rigidbody2D rb;
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
	
	}

    public void changeSpeed(float speed)
    {
        rb.velocity = new Vector2(0, speed);
    }
}
