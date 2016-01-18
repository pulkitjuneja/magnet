using UnityEngine;
using System.Collections;

public class MagEnvInteraction : MonoBehaviour {

    Transform ParentTransform ;
    Rigidbody2D Parentrigidbody;

    public static float CurrentFieldRadius,InitialFieldRadius;

    Transform OtherBody; 
    bool inMagneticInfluence = false;
    int PullDirection ;

	void Start () 
    {
	  ParentTransform = transform.parent.transform;
      Parentrigidbody = transform.parent.gameObject.GetComponent<Rigidbody2D>();
      CurrentFieldRadius = InitialFieldRadius = GetComponent<CircleCollider2D>().radius;
	}

    void FixedUpdate()
    {
        if(inMagneticInfluence && OtherBody)
        {
            Vector2 direction = Vector3.Normalize(OtherBody.position - transform.position);
            float distance = Mathf.Min(Mathf.Abs(OtherBody.position.x - transform.position.x), CurrentFieldRadius);
            float MagStr = (CurrentFieldRadius / distance) * 15;
            direction.y = 0;
            Parentrigidbody.AddForce(direction * (MagStr * PullDirection), ForceMode2D.Force);
        }
    }
	

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "attractor")
        {
            OtherBody = other.transform;
            inMagneticInfluence = true;
            PullDirection = 1;
        }
        else if(other.gameObject.tag == "repulsor")
        {
            OtherBody = other.transform;
            inMagneticInfluence = true;
            PullDirection = -1;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        OtherBody = null;
        inMagneticInfluence = false; 
    }
}
