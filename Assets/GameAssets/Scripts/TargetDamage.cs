using UnityEngine;
using System.Collections;

public class TargetDamage : MonoBehaviour {

	public int HitPoints = 2;					//	The amount of damage our target can take
	public Sprite DamagedSprite;				//	The reference to our "damaged" sprite
	public float DamageImpactSpeed;				//	The speed threshold of colliding objects before the target takes damage
    public AudioClip TargetHitSound;                 //  Sound that will be played when target object is hit
	
	
	private int currentHitPoints;				//	The current amount of health our target has taken
	private float damageImpactSpeedSqr;			//	The square value of Damage Impact Speed, for efficient calculation
	private SpriteRenderer spriteRenderer;		//	The reference to this GameObject's sprite renderer
    private AudioSource aSource;                //  AudioSource that will be used to play sounds
	
	void Start () {
		//	Get the SpriteRenderer component for the GameObject's Rigidbody
		spriteRenderer = GetComponent <SpriteRenderer> ();

        // Get the attached AudioSource compoenent
        aSource = GetComponent<AudioSource>();

		//	Initialize the Hit Points
		currentHitPoints = HitPoints;

		//	Calculate the Damage Impact Speed Squared from the Damage Impact Speed
		damageImpactSpeedSqr = DamageImpactSpeed * DamageImpactSpeed;
	}
	
	void OnCollisionEnter2D (Collision2D collision) {
		//	Check the colliding object's tag, and if it is not "Damager", exit this function
		if (collision.collider.tag != "Damager")
			return;
		
		//	Check the colliding object's velocity's Square Magnitude, and if it is less than the threshold, exit this function
		if (collision.relativeVelocity.sqrMagnitude < damageImpactSpeedSqr)
			return;

        aSource.PlayOneShot(TargetHitSound);

		//	We have taken damage, so change the sprite to the damaged sprite
		spriteRenderer.sprite = DamagedSprite;
		//	Decriment the Current Health of the target
		currentHitPoints--;

		//	If the Current Health is less than or equal to zero, call the Kill() function
		if(currentHitPoints <= 0)
			Kill ();
	}
	
	void Kill () {
		//	As the particle system is attached to this GameObject, when Killed, switch off all of the visible behaviours...
		spriteRenderer.enabled = false;
		GetComponent<Collider2D>().enabled = false;
		GetComponent<Rigidbody2D>().isKinematic = true;

		//	... and Play the particle system
		GetComponent<ParticleSystem>().Play();

        // Remove the object from the scene manager
        Destroy(gameObject, 1);
	}
}
