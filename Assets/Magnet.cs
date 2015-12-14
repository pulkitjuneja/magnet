using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour {

    public Rigidbody2D rigidbody;


	void Start () 
    {
        rigidbody = GetComponent<Rigidbody2D>();
	}
	void Update () 
    {
       // if (Input.GetKey(KeyCode.W))
        //    rigidbody.AddForce(new Vector2(0,20));
       // if (Input.GetKey(KeyCode.S))
         //   rigidbody.AddForce(new Vector2(0, -20));
        if (Input.GetKey(KeyCode.A))
            rigidbody.AddForce(new Vector2(-20, 0));
        if (Input.GetKey(KeyCode.D))
            rigidbody.AddForce(new Vector2(20, 0));
	}

    void OnTriggerEnter2D(Collider2D other)
    {
            if(other.gameObject.tag == "absorbable")
            {
                float weight = other.gameObject.GetComponent<Attractable>().Weight;
                Destroy(other.gameObject);
                rigidbody.mass += 0.1f;
                transform.localScale += new Vector3(0.01f, 0.01f,0f);
                MagEnvInteraction.CurrentFieldRadius = MagEnvInteraction.InitialFieldRadius * transform.localScale.x;
                Debug.Log(MagEnvInteraction.CurrentFieldRadius);
            }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name);
        rigidbody.isKinematic = true;
        transform.parent = other.gameObject.transform;
    }
}
