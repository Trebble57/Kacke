using UnityEngine;
using System.Collections;

public class ProjectileHitSound : MonoBehaviour {

    private AudioSource _aSource;
    private SoundAssets _sounds;

	// Use this for initialization
	void Start () {
        _aSource = GetComponent<AudioSource>();
        _sounds = GameObject.Find("LC").GetComponent<SoundAssets>();
	}
	
	public void OnCollisionEnter2D(Collision2D coll)
    {
        _aSource.clip = _sounds.ProjectileCollision;
        _aSource.loop = false;
        _aSource.Play();
    }
}
