using UnityEngine;

public class ProjectileDragging : MonoBehaviour {
    [Header("Required")]
    [Tooltip("LineRenderer attached the front part of the catapult")]
    public LineRenderer catapultLineFront;
    [Tooltip("LineRenderer attached to the back part of the catapult")]
    public LineRenderer catapultLineBack;
    [Header("Optional")]
    [Tooltip("Max radius the projectile can be dragged")]
    public float maxStretch = 3.0f;
    [Tooltip("Name of the Rubber band attached to the front catapult part")]
    public string LayerNameFrontLine = "Foreground";
    [Tooltip("Name of the Rubber band attached to the back catapult part")]
    public string LayerNameBackLine = "Foreground";
	
	private SpringJoint2D spring;           // The spring joint that will hold the projectile inside the specified radius
	private Transform catapult;             // Transform instance of the catapult
	private Ray rayToMouse;                 
	private Ray leftCatapultToProjectile;   
	private float maxStretchSqr;            // Max stretch radius squared
	private float circleRadius;             // Radius of the projectile collider
	private bool clickedOn;                 // Indicates whether the projectile has been clicked on
	private Vector2 prevVelocity;           // Velocity from the previous frame


    void Awake () {
        // Get the catapult transform as soon as the physics simulation for this object starts
		spring = GetComponent <SpringJoint2D> ();
		catapult = spring.connectedBody.transform;
    }
	
	void Start ()
    {
        LineRendererSetup ();
		rayToMouse = new Ray(catapult.position, Vector3.zero);
		leftCatapultToProjectile = new Ray(catapultLineFront.transform.position, Vector3.zero);
		maxStretchSqr = maxStretch * maxStretch;
		PolygonCollider2D circle = GetComponent<Collider2D>() as PolygonCollider2D;

        circleRadius = 0.3f;
	}
	
	void Update () {
		if (clickedOn)
			Dragging ();
		
		if (spring != null) {
			if (!GetComponent<Rigidbody2D>().isKinematic && prevVelocity.sqrMagnitude > GetComponent<Rigidbody2D>().velocity.sqrMagnitude) {
				Destroy (spring);
				GetComponent<Rigidbody2D>().velocity = prevVelocity;
			}
			
			if (!clickedOn)
				prevVelocity = GetComponent<Rigidbody2D>().velocity;
			
			LineRendererUpdate ();
			
		} else {
			catapultLineFront.enabled = false;
			catapultLineBack.enabled = false;
		}
	}
	
	void LineRendererSetup () {
		catapultLineFront.SetPosition(0, catapultLineFront.transform.position);
		catapultLineBack.SetPosition(0, catapultLineBack.transform.position);
		
		catapultLineFront.sortingLayerName = LayerNameFrontLine;
		catapultLineBack.sortingLayerName = LayerNameBackLine;
		
		catapultLineFront.sortingOrder = 3;
		catapultLineBack.sortingOrder = 1;
	}
	
	void OnMouseDown () {
		spring.enabled = false;
		clickedOn = true;
	}
	
	void OnMouseUp () {
		spring.enabled = true;
		GetComponent<Rigidbody2D>().isKinematic = false;
        GameObject.Find("OrthoFollowResetCamera").GetComponent<GameOverController>().enabled = true;
        clickedOn = false;
	}

	void Dragging () {
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 catapultToMouse = mouseWorldPoint - catapult.position;
		
		if (catapultToMouse.sqrMagnitude > maxStretchSqr) {
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
		}
		
		mouseWorldPoint.z = 0f;
		transform.position = mouseWorldPoint;
	}

	void LineRendererUpdate () {
		Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
		leftCatapultToProjectile.direction = catapultToProjectile;
		Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + circleRadius);
		catapultLineFront.SetPosition(1, holdPoint);
		catapultLineBack.SetPosition(1, holdPoint);
	}
}
