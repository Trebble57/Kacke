﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class DispenserTrigger : MonoBehaviour {
    public LineRenderer CatapultFront;
    public LineRenderer CatapultBack;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        var potato = other.gameObject;
        potato.GetComponent<Rigidbody2D>().isKinematic = true;
        potato.GetComponent<SpringJoint2D>().enabled = true;
        var pDragging = potato.AddComponent<ProjectileDragging>();
        pDragging.enabled = false;
        pDragging.catapultLineBack = CatapultBack;
        pDragging.catapultLineFront = CatapultFront;
        pDragging.maxStretch = 3f;
        pDragging.enabled = true;
        var cam = GameObject.Find("OrthoFollowResetCamera");
        cam.GetComponent<ProjectileFollow>().projectile = potato.transform;
        //cam.GetComponent<GameOverController>().Projectile = potato.GetComponent<Rigidbody2D>();
        Destroy(cam.GetComponent<GameOverController>());
        var goCtrl = cam.AddComponent<GameOverController>();
        try {
            goCtrl.LvlLoadText = GameObject.Find("LvlLoadingText").GetComponent<Text>();
        }
        catch (Exception ex)
        {

        }
        goCtrl.GameOverCanvas = GameObject.Find("CanvasGameOver").GetComponent<Canvas>();
        goCtrl.Projectile = potato.GetComponent<Rigidbody2D>();
        CatapultBack.probeAnchor = potato.transform;
        CatapultFront.probeAnchor = potato.transform;
        CatapultBack.enabled = true;
        CatapultFront.enabled = true;
    }
}