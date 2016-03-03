using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

    string tag;
    Animator anim;
    AudioSource audio;
	void Start ()
    {
        tag = gameObject.tag;
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
	}
	
	void Update () 
    {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
             switch(tag)
             {
                 case "invincibility" : other.gameObject.GetComponent<Magnet>().beInvincible(); break;
                 case "DownSize" : DownSize(other.gameObject); break;
                 case "ReduceTime": ReduceTime(); break;  
             }
             anim.SetBool("Fade", true);
             if (audio.clip != null)
                 audio.Play();
        }
        else if(other.tag=="Respawn")
        {
            Destroy(this.gameObject);
        }
    }

    void DownSize(GameObject player)
    {
        var TargetScale = Mathf.Clamp (player.transform.localScale.x - 0.5f,1,500);
        player.transform.localScale = new Vector3(TargetScale, TargetScale);
       // harmonicMotion.temp = Mathf.Clamp(harmonicMotion.temp - 12.5f, 0, 500);
        Debug.Log(harmonicMotion.temp);
    }

    void ReduceTime()
    {
        Time.timeScale = Mathf.Clamp(Time.timeScale - 2.0f, 1, 5.0f);
    }
}
