using UnityEngine;
using System.Collections;

public class CatSpeaker : MonoBehaviour {

    private AudioClip[] happyClips;
    private AudioClip[] angryClips;

	// Use this for initialization
	void Start () {
        happyClips = Resources.LoadAll<AudioClip>("catsounds/happy");
        angryClips = Resources.LoadAll<AudioClip>("catsounds/angry");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void PlayHappyCat()
    {
        PlayRandom(happyClips);
    }

    void PlayAngryCat()
    {
        PlayRandom(angryClips);
    }

    void PlayRandom(AudioClip[] clips)
    {
        int i = Random.Range(0, clips.Length);
        AudioSource.PlayClipAtPoint(clips[i], transform.position);
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.GetComponent<Goal>() != null)
        {
            PlayAngryCat();
        }
        if (c.gameObject.GetComponent<Paddle>() != null)
        {
            PlayHappyCat();
        }
        if (c.gameObject.GetComponent<WallController>() != null)
        {
            PlayHappyCat();
        }
    }
}
