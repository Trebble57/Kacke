using UnityEngine;
using System.Collections;

public class TargetDamage : MonoBehaviour {

	public int              HitPoints = 2;				//	The amount of damage our target can take
	public Sprite           DamagedSprite;              //	The reference to our "damaged" sprite
    public Sprite           ExplosionSprite;				//	The reference to our "damaged" sprite
	public float            DamageImpactSpeed;			//	The speed threshold of colliding objects before the target takes damage
	
	
	private int             _currentHitPoints;			//	The current amount of health our target has taken
	private float           _damageImpactSpeedSqr;		//	The square value of Damage Impact Speed, for efficient calculation
	private SpriteRenderer  _spriteRenderer;	        //	The reference to this GameObject's sprite renderer
    private AudioSource     _aSource;                   //  AudioSource that will be used to play sounds
    private SoundAssets     _sounds;
	
	void Start () {
        // Get the SoundEffect storage
        _sounds = GameObject.Find("LC").GetComponent<SoundAssets>();

		//	Get the SpriteRenderer component for the GameObject's Rigidbody
		_spriteRenderer = GetComponent <SpriteRenderer> ();

        // Get the attached AudioSource compoenent
        _aSource = GetComponent<AudioSource>();

		//	Initialize the Hit Points
		_currentHitPoints = HitPoints;

		//	Calculate the Damage Impact Speed Squared from the Damage Impact Speed
		_damageImpactSpeedSqr = DamageImpactSpeed * DamageImpactSpeed;
	}
	
	void OnCollisionEnter2D (Collision2D collision) {
		//	Check the colliding object's tag, and if it is not "Damager", exit this function
		if (collision.collider.tag != "Damager")
			return;
		
		//	Check the colliding object's velocity's Square Magnitude, and if it is less than the threshold, exit this function
		if (collision.relativeVelocity.sqrMagnitude < _damageImpactSpeedSqr || collision.collider.transform.position.y < transform.position.y)
			return;

		//	We have taken damage, so change the sprite to the damaged sprite
		_spriteRenderer.sprite = DamagedSprite;
		//	Decriment the Current Health of the target
		_currentHitPoints--;

        //	If the Current Health is less than or equal to zero, call the Kill() function
        if (_currentHitPoints <= 0)
        {
            GameObject.Find("Dispenser").GetComponent<AudioSource>().PlayOneShot(_sounds.TargetExplode); // Sound needs to be played on an audioSource that wil not be destroyed
            Kill();
        }
        else if (_currentHitPoints >= 1)
            _aSource.PlayOneShot(_sounds.TargetHit);
    }
	
	void Kill () {
		//	As the particle system is attached to this GameObject, when Killed, switch off all of the visible behaviours...
		_spriteRenderer.enabled = false;
		GetComponent<Collider2D>().enabled = false;
		GetComponent<Rigidbody2D>().isKinematic = true;

        _spriteRenderer.sprite = ExplosionSprite;
        //	... and Play the particle system
        GetComponent<ParticleSystem>().Play();

        // Remove the object from the scene manager
        Destroy(gameObject,0);
	}
}
