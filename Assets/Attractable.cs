using UnityEngine;
using System.Collections;

public class Attractable : MonoBehaviour {

    string FieldObjectName = "magnetic field";
    public float Weight ;
    public Rigidbody2D rigidbody;
    public GameObject Magnet; 
	void Start () 
    {
        rigidbody = GetComponent<Rigidbody2D>();
	}
	void Update () {
	
	}
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == FieldObjectName)
        {
            Vector3 direction = Vector3.Normalize(Magnet.transform.position - transform.position);
            rigidbody.AddForce(direction*500, ForceMode2D.Force);
        }
    }
}
