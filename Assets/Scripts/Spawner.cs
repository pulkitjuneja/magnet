using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public bool CanSpawn = true;
    public GameObject ToSpawn = null;
	void Start () {
	
	}

	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        CanSpawn = false; 
    }
    void OnTriggerExit2D(Collider2D other)
    {
        CanSpawn = true;
        if(ToSpawn != null)
        {
            Spawn(ToSpawn);
            ToSpawn = null;
        }
    }

    public void Spawn(GameObject spawn)
    {
        if (CanSpawn)
        {
            var pos = this.transform.position;
            GameObject pickup = Instantiate(spawn, pos, Quaternion.identity) as GameObject;
            pickup.GetComponent<Rigidbody2D>().velocity = new Vector2(0, generator.gamespeed);
        }
        else
        {
            ToSpawn = spawn; 
        }
    }
}
