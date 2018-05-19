using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

    string name;
    Animator anim;
    AudioSource audio;
	void Start ()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
	}
	
	void Update () 
    {
        transform.Translate(Vector3.up * GamePlay.gamespeed * Time.deltaTime,Space.World);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
             switch(gameObject.tag)
             {
                 case "invincibility" : other.gameObject.GetComponent<Magnet>().beInvincible(); break;
                 case "DownSize" : DownSize(other.gameObject); break;
                 case "ReduceTime": ReduceTime(); break;  
             }
             anim.SetBool("Fade", true);
             if (audio.clip != null)
                  AudioManager.play(audio, audio.clip);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Respawn")
        {
            StartCoroutine(DestroyAfterAudio());
        }
    }

    IEnumerator DestroyAfterAudio () {
        while(audio.isPlaying) {
            yield return null;
        }
        Destroy(this.gameObject);
    }

    void DownSize(GameObject player)
    {
        // var TargetScale = Mathf.Clamp (player.transform.localScale.x - 0.5f,1,500);
        // harmonicMotion.temp = Mathf.Clamp(harmonicMotion.temp - 12.5f, 0, 500);
        // player.transform.localScale = new Vector3(TargetScale, TargetScale,TargetScale);
        player.GetComponent<Magnet>().downScale();
    }

    void ReduceTime()
    {   
        GamePlay.gamespeed = Mathf.Clamp(GamePlay.gamespeed-7.0f, 5, 18.0f);
    }
}
