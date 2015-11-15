using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class DispenserController : MonoBehaviour {
    [Tooltip("Collider that prevents projectiles from falling out of the dispenser")]
    public BoxCollider2D HatchColl;

    [Tooltip("LineRenderer for the front part of the catapult")]
    public LineRenderer CatapultFront;
    [Tooltip("LineRenderer for the back part of the catapult")]
    public LineRenderer CatapultBack;

    private GameObject levelCtrl;

    // Use this for initialization
    void Start () {
        levelCtrl = GameObject.Find("LC");
	}
	
	// Update is called once per frame
	void Update () {
	}

    /// <summary>
    /// Dispenses a projectile
    /// </summary>
    public void Dispense()
    {
        HatchColl.enabled = false;
        StartCoroutine("closeHatch"); // close dispenser hatch
    }

    /// <summary>
    /// Called by DispenserTrigger.cs when Potato is in firing position.
    /// </summary>
    public void OnDispensed(Collider2D potatoCollider)
    {
        var potato = potatoCollider.gameObject; // Get the GameObject the collider is attached to which happens to be one of the potatoes

        potato.GetComponent<Rigidbody2D>().isKinematic = true; // Set Rigidbody to kinematic so it stays in catapult position
        potato.GetComponent<SpringJoint2D>().enabled = true; // Enable spring joint for dragging

        var pDragging = potato.AddComponent<ProjectileDragging>();  // Get DraggingScript attached to that potato

        pDragging.enabled = false; 
        pDragging.CatapultLineBack = CatapultBack; // Attach catapult line renderers
        pDragging.CatapultLineFront = CatapultFront;
        pDragging.MaxStretch = 3f; 
        pDragging.enabled = true; // Enable script

        GameObject.Find("OrthoFollowResetCamera").GetComponent<ProjectileFollow>().projectile = potato.transform; // Set follow target transform to current potato

        Destroy(levelCtrl.GetComponent<GameOverController>()); // Destroy old GameOverController...
        var goCtrl = levelCtrl.AddComponent<GameOverController>(); // ...and assign a new one.

        // Check for level load status text and assign it if it exists
        try
        {
            goCtrl.LvlLoadText = GameObject.Find("LvlLoadingText").GetComponent<Text>();
        }
        catch (Exception ex)
        {
            // If there is none, print exception to console
            Debug.LogWarning(ex);
        }
        goCtrl.GameOverCanvas = GameObject.Find("CanvasGameOver"); // Assign GameOverCanvas to controller
        goCtrl.Projectile = potato.GetComponent<Rigidbody2D>(); // Assign current potato as target projectile

        // Assign click receivers to UI Buttons
        GameObject.Find("btnNext").GetComponent<Button>().onClick.AddListener(goCtrl.OnNextLevelClick); ;
        GameObject.Find("btnQuit").GetComponent<Button>().onClick.AddListener(goCtrl.OnQuitClick); ;

        // Assign potato transform to catapult parts and enable those
        CatapultBack.probeAnchor = potato.transform;
        CatapultFront.probeAnchor = potato.transform;
        CatapultBack.enabled = true;
        CatapultFront.enabled = true;
    }

    /// <summary>
    /// Enables the collider that holds projectiles in the dispenser in 0.5 seconds
    /// </summary>
    IEnumerator closeHatch()
    {
        yield return new WaitForSeconds(0.5f);
        HatchColl.enabled = true;
    }

}
