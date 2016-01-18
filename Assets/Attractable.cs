using UnityEngine;
using System.Collections;

public class Attractable : MonoBehaviour {
    string FieldObjectName = "magnetic field";
    public float Weight ;
    bool inMagnetRange = false;
    Transform parent;
    public Rigidbody2D rigidbody;
    Transform MagnetLocation;
    public GameObject Magnet; 


	void Start () 
	{
		rigidbody = GetComponent<Rigidbody2D>();
        parent = transform.parent;
	}
	void Update () {
	
	}

    void FixedUpdate()
    {
        if(inMagnetRange && MagnetLocation)
        {
            Vector3 direction = MagnetLocation.position - transform.position;
            float magnetDistance = Vector3.Distance(MagnetLocation.position, transform.position);
            float magnetstr = (MagEnvInteraction.CurrentFieldRadius/magnetDistance)*500;
            rigidbody.AddForce(direction * magnetstr, ForceMode2D.Force);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == FieldObjectName)
        {
            transform.parent = null ;
            rigidbody.velocity = new Vector2(0, 0);
            rigidbody.isKinematic = false;
            MagnetLocation = other.transform;
            inMagnetRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == FieldObjectName)
        {
            rigidbody.isKinematic = true;
            transform.parent = parent;
            inMagnetRange = false;
            MagnetLocation = null;
        }
    }
 }