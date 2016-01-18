using UnityEngine;
using System.Collections;
public class Magnet : MonoBehaviour {
	
	public Rigidbody2D rigidbody;
	public AudioClip absorb;
	public AudioClip hit;
    CircleCollider2D collider;
    CircleCollider2D magField;
    bool ControlsDisabled = false;
    AudioSource audio;
	Animator animator;
	
	void Start () 
	{
		animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        collider = GetComponent<CircleCollider2D>();
        magField = GetComponentsInChildren<CircleCollider2D>()[1];
        Debug.Log(magField.radius);
		rigidbody = GetComponent<Rigidbody2D>();
	}
	void Update () 
    {
        if (!ControlsDisabled)
        {
            if (Input.GetKey(KeyCode.A))
                rigidbody.AddForce(new Vector2(-50, 0));
            if (Input.GetKey(KeyCode.D))
                rigidbody.AddForce(new Vector2(50, 0));
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
            if(other.gameObject.tag == "absorbable")
			{	
				animator.SetInteger("state",1);
                float weight = other.gameObject.GetComponent<Attractable>().Weight;
                Destroy(other.gameObject);
                rigidbody.mass += 0.02f;
                transform.localScale += new Vector3(0.02f, 0.02f,0f);
                MagEnvInteraction.CurrentFieldRadius = MagEnvInteraction.InitialFieldRadius * transform.localScale.x;
				harmonicMotion.temp += 0.5f;
				//Debug.Log(MagEnvInteraction.CurrentFieldRadius);
				audio.clip = absorb;	
				audio.Play ();
            }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
		animator.SetInteger ("state", 2);
       // Debug.Log(other.gameObject.name);
        rigidbody.isKinematic = true;
        transform.parent = other.gameObject.transform;
		audio.clip = hit;
		audio.Play ();
	}

    public void beInvincible()
    {
        StartCoroutine(Invincibility());
    }

    IEnumerator Invincibility()
    {
        rigidbody.velocity=Vector2.zero;
        collider.enabled = false;
        magField.enabled = false;
        float originalTimeScale = Time.timeScale;
        ControlsDisabled = true;
        Time.timeScale = 6.5f;
        float time = Time.realtimeSinceStartup + 3.0f;
        while(Time.realtimeSinceStartup<time)
        {
            yield return null ;
        }
        ControlsDisabled = false;
        Time.timeScale = originalTimeScale;
        float recovertime = Time.realtimeSinceStartup + 2.0f;
        while(Time.realtimeSinceStartup<recovertime)
        {
            yield return null;
        }
        collider.enabled = true;
        magField.enabled = true;
    }
}
