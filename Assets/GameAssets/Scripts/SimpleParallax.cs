using UnityEngine;
using System.Collections;

public class SimpleParallax : MonoBehaviour {

    [Header("Values")]
    [Tooltip("Amount of unitss the background will be displaced from the camera position")]
    public float DisplacementAmount;
    [Tooltip("The position of the target camera")]
    public Transform Camera;

    private float prevCamPosX;
    private Vector3 startPos;
    private Vector3 camStartPos;

	// Use this for initialization
	void Start () {
        prevCamPosX = Camera.position.x;
        camStartPos = Camera.position;
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Diff:" + (Camera.position.x - prevCamPosX));
        if ((Camera.position.x - prevCamPosX) > 0.02)
        {
            Debug.Log("Called");
            transform.position = transform.position - new Vector3(Camera.position.x * DisplacementAmount, 0, 0);
        }

        if (Camera.position == camStartPos)
        {
            transform.position = startPos;
        }

        prevCamPosX = Camera.position.x;
	}
}
