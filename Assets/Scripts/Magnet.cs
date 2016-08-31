using UnityEngine;
using System;
using System.Collections;


public class Magnet : MonoBehaviour {
	
	public Rigidbody2D rigidbody;
	public AudioClip absorb;
    bool Toclamp = false;
	public AudioClip hit;
    CircleCollider2D collider;
    CircleCollider2D magField;
	SpriteRenderer boost;
    bool ControlsDisabled = false;
    AudioSource audio;
    public event Action GameOverEvent;
	Animator animator;

    float TouchSeperator;
	
	void Start () 
	{
		animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        collider = GetComponent<CircleCollider2D>();
        TouchSeperator = Screen.width / 2;
        magField = GetComponentsInChildren<CircleCollider2D>()[1];
		boost = GetComponentsInChildren<SpriteRenderer> () [1] ;
		rigidbody = GetComponent<Rigidbody2D>();
		boost.enabled = false;
	}
	void Update () 
    {
        if (!ControlsDisabled)
        {
             #if  UNITY_STANDALONE || UNITY_WEBPLAYER
            if (Input.GetKey(KeyCode.A))
                rigidbody.AddForce(new Vector2(-50, 0));
            if (Input.GetKey(KeyCode.D))
                rigidbody.AddForce(new Vector2(50, 0));
            #else
            if(Input.touchCount>0)
            {
                Touch touch = Input.touches[0];
                if (touch.phase == TouchPhase.Stationary)
                {
                    if (touch.position.x < TouchSeperator)
                        rigidbody.AddForce(new Vector2(-2000*Time.deltaTime, 0));
                    else if (touch.position.x >= TouchSeperator)
                        rigidbody.AddForce(new Vector2(2000* Time.deltaTime, 0));
                }
            }
            #endif 
        }
        if(Toclamp)
         transform.position = new Vector3(Mathf.Clamp(transform.position.x, -5.0f, 5.0f), transform.position.y);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
            if(other.gameObject.tag == "absorbable")
			{	
				animator.SetInteger("state",1);
                Destroy(other.gameObject);
                rigidbody.mass += 0.02f;
                transform.localScale += new Vector3(0.02f, 0.02f,0f);
                MagEnvInteraction.CurrentFieldRadius = MagEnvInteraction.InitialFieldRadius * transform.localScale.x;
				harmonicMotion.temp += 0.5f;
				audio.clip = absorb;	
				audio.Play ();
            }
            else if(other.gameObject.tag == "Respawn")
            {
                Debug.Log("error");
            }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
		animator.SetInteger ("state", 2);
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
        float originalspeed = GamePlay.gamespeed;
        GamePlay.gamespeed = 1;
        ControlsDisabled = true;
        rigidbody.velocity = Vector2.zero;
        float time1 = Time.realtimeSinceStartup + 1.0f;
        while (Time.realtimeSinceStartup < time1)
        {
            yield return null;
        }
        rigidbody.velocity=Vector2.zero;
        collider.enabled = false;
        magField.enabled = false;
		boost.enabled = true;
        GamePlay.gamespeed = 23.0f;
        float time = Time.realtimeSinceStartup + 3.0f;
        while(Time.realtimeSinceStartup<time)
        {
            yield return null ;
        }
        ControlsDisabled = false;
        GamePlay.gamespeed = originalspeed;
        Toclamp = true;
        float recovertime = Time.realtimeSinceStartup + 2.0f;
        while(Time.realtimeSinceStartup<recovertime)
        {
            yield return null;
        }
        collider.enabled = true; 
        magField.enabled = true;
        Toclamp = false;
		boost.enabled = false;
    }

    void OnDestroy()
    {
        if(GameOverEvent!=null)
           GameOverEvent();
    }
}
