using UnityEngine;


/// <summary>
/// Handles the dragging of the projectile by the user
/// </summary>
public class ProjectileDragging : MonoBehaviour {

    [Header("Required")]

    [Tooltip("LineRenderer attached the front part of the catapult")]
    public LineRenderer CatapultLineFront;

    [Tooltip("LineRenderer attached to the back part of the catapult")]
    public LineRenderer CatapultLineBack;


    [Header("Optional")]
    [Tooltip("Max radius the projectile can be dragged")]
    public float        MaxStretch = 3.0f;

    [Tooltip("Name of the Rubber band attached to the front catapult part")]
    public string       LayerNameFrontLine = "Foreground";

    [Tooltip("Name of the Rubber band attached to the back catapult part")]
    public string       LayerNameBackLine = "Foreground";
	

	private SpringJoint2D   _spring;            // The spring joint that will hold the projectile inside the specified radius
	private Transform       _catapult;          // Transform instance of the catapult
	private Ray             _rayToMouse;                 
	private Ray             _leftCatapultToProjectile;   
	private float           _maxStretchSqr;     // Max stretch radius squared
	private float           _circleRadius;      // Radius of the projectile collider
	private bool            _clickedOn;         // Indicates whether the projectile has been clicked on
	private Vector2         _prevVelocity;      // Velocity from the previous frame
    private SoundAssets     _sounds;
    private AudioSource     _aSource;           // AudioSource for this component

    /// <summary>
    /// Called when this object is active in the physics simulation
    /// </summary>
    void Awake () {

        // Get the catapult transform as soon as the physics simulation for this object starts
		_spring = GetComponent <SpringJoint2D> ();
		_catapult = _spring.connectedBody.transform;
        _sounds = GameObject.Find("LC").GetComponent<SoundAssets>();
    }
	
	void Start ()
    {
        _aSource = GameObject.Find("Dispenser").GetComponent<AudioSource>(); // Retrieve AudioSource of Dispenser

        LineRendererSetup (); // Call the function to setup the line renderers

		_rayToMouse = new Ray(_catapult.position, Vector3.zero); // Set default value for rayToMouse so it is not null
		_leftCatapultToProjectile = new Ray(CatapultLineFront.transform.position, Vector3.zero); // Set default value

		_maxStretchSqr = MaxStretch * MaxStretch; // Calc max stretch squared

        _circleRadius = 0.3f; // Offset from middle of the potato to the point where the rubber band is attached
	}
	
    /// <summary>
    /// Update. Called ~60 times per second
    /// </summary>
	void Update () {

        // If click is registered, handle dragging
		if (_clickedOn)
			Dragging ();
		
		if (_spring != null) { // executed when projectile is still attached to catapult
			if (!GetComponent<Rigidbody2D>().isKinematic && _prevVelocity.sqrMagnitude > GetComponent<Rigidbody2D>().velocity.sqrMagnitude) {
				Destroy (_spring); //  Destroy spring joint when proectile is released and therefore velocity is higher than in the previous frame
				GetComponent<Rigidbody2D>().velocity = _prevVelocity;
			}
			
			if (!_clickedOn) // set this only if projectile has been released by the player
				_prevVelocity = GetComponent<Rigidbody2D>().velocity;
			
			LineRendererUpdate ();
			
		} else {
			CatapultLineFront.enabled = false;
			CatapultLineBack.enabled = false;
		}
	}
	
    /// <summary>
    /// Handles the setup of the line renderers
    /// </summary>
	void LineRendererSetup () {

		CatapultLineFront.SetPosition(0, CatapultLineFront.transform.position);
		CatapultLineBack.SetPosition(0, CatapultLineBack.transform.position);
		
		CatapultLineFront.sortingLayerName = LayerNameFrontLine;
		CatapultLineBack.sortingLayerName = LayerNameBackLine;
		
		CatapultLineFront.sortingOrder = 3;
		CatapultLineBack.sortingOrder = 1;
	}
	
	void OnMouseDown () {
		_spring.enabled = false;
		_clickedOn = true;
	}
	
	void OnMouseUp () {
		_spring.enabled = true;
		GetComponent<Rigidbody2D>().isKinematic = false;
        GameObject.Find("LC").GetComponent<GameOverController>().enabled = true;
        _aSource.PlayOneShot(_sounds.ProjectileRelease);

        _aSource.clip = _sounds.ProjectileFlying;
        _aSource.PlayDelayed(0.05f);
        _clickedOn = false;
	}

	void Dragging () {
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 catapultToMouse = mouseWorldPoint - _catapult.position;
		
		if (catapultToMouse.sqrMagnitude > _maxStretchSqr) {
			_rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = _rayToMouse.GetPoint(MaxStretch);
		}
		
		mouseWorldPoint.z = 0f;
		transform.position = mouseWorldPoint;
	}

	void LineRendererUpdate () {
		Vector2 catapultToProjectile = transform.position - CatapultLineFront.transform.position;
		_leftCatapultToProjectile.direction = catapultToProjectile;
		Vector3 holdPoint = _leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + _circleRadius);
		CatapultLineFront.SetPosition(1, holdPoint);
		CatapultLineBack.SetPosition(1, holdPoint);
	}
}
