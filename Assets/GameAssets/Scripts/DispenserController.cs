using UnityEngine;
using System.Collections;

public class DispenserController : MonoBehaviour {
    [Tooltip("Collider that prevents projectiles from falling out of the dispenser")]
    public BoxCollider2D HatchColl;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Dispense()
    {
        HatchColl.enabled = false;
        StartCoroutine("closeHatch");
    }

    IEnumerator closeHatch()
    {
        yield return new WaitForSeconds(0.5f);
        HatchColl.enabled = true;
    }

}
