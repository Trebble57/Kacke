using UnityEngine;
using System.Collections;

/// <summary>
/// Script for a simple Parallax background scrollling effect.
/// Scrolls the GameObject it is attached to based on Camera position
/// </summary>
public class SimpleParallax : MonoBehaviour {

    [Header("Values")]
    [Tooltip("Amount of unitss the background will be displaced from the camera position")]
    public float DisplacementAmount;
    [Tooltip("The position of the target camera")]
    public Transform Camera;

    private float prevCamPosX;      // Camera's position on x-axis in last frame
    private Vector3 startPos;       // Background position on start
    private Vector3 camStartPos;    // Camera's start pos 

	// Use this for initialization
	void Start () {
        prevCamPosX = Camera.position.x;    // Variable mit aktueller position initialisieren
        camStartPos = Camera.position;      // Save camera start position
        startPos = transform.position;      // Save background image start position
	}
	
	// Update is called once per frame
	void Update () {

        // Check if camera position has changed at all
        if ((Camera.position.x - prevCamPosX) > 0.02)
        {
            // If so, shift this GameObject by the specified DisplacementAmount
            transform.position = transform.position - new Vector3(Camera.position.x * DisplacementAmount, 0, 0);
        }

        // If Camera is reset to start position, reset this GameObject also to its initial position
        if (Camera.position == camStartPos)
        {
            transform.position = startPos;
        }

        prevCamPosX = Camera.position.x; // Save current camera pos on x-axis for next frame
	}
}
