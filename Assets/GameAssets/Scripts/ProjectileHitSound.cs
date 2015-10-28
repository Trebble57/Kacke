using UnityEngine;
using System.Collections;

public class ProjectileHitSound : MonoBehaviour {
    public AudioClip HitSound;

    private AudioSource aSource;

	// Use this for initialization
	void Start () {
        aSource = GetComponent<AudioSource>();
	}
	
	public void OnCollisionEnter2D(Collision2D coll)
    {
        aSource.clip = HitSound;
        aSource.loop = false;
        aSource.Play();
    }
}
