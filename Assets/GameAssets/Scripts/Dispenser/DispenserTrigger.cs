using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class DispenserTrigger : MonoBehaviour {
    public DispenserController DispenserController;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        DispenserController.OnDispensed(other);
    }
}
