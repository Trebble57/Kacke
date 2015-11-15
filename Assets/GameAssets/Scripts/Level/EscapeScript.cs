using UnityEngine;
using System.Collections;

public class EscapeScript : MonoBehaviour {

    public KeyCode Key;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(Key))
        {
            Application.LoadLevel(1);
        }
	}
}
