using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public bool CanSpawn = true;
    public GameObject ToSpawn = null;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "level" && other.gameObject.tag!="absorbable")
        {
            CanSpawn = false;
        }
    }
    void OnTriggerExit2D(Collider2D other )
    {
        if (other.gameObject.tag != "level" && other.gameObject.tag != "absorbable")
        {
            CanSpawn = true;
            if (ToSpawn != null)
            {
                Spawn(ToSpawn);
                ToSpawn = null;
            }
        }
    }

    public void Spawn(GameObject spawn)
    {
        if (CanSpawn)
        {
            var pos = this.transform.position;
            GameObject pickup = Instantiate(spawn, pos, Quaternion.identity) as GameObject;
        }
        else
        {
            ToSpawn = spawn; 
        }
    }
}
