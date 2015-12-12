using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

    string FieldObjectName = "magnetic field";
    public GameObject Magnet; 
	void Start () {
	}
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == FieldObjectName)
        {
            Debug.Log(other.gameObject.transform.position);
        }
    }
}
