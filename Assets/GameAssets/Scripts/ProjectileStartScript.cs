using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProjectileStartScript : MonoBehaviour {

    public AudioClip LevelAnouncement;

    private DispenserController dispenser;

    // Use this for initialization
    void Start () {

        // Find the level text object if it exists and update it with the current level count.
        // Remember: Scenes "0" and "1" are SplashScreen and MainMenu so the first lvl has the index "2"!
        var lvlText = GameObject.Find("txtLevel").GetComponent<Text>();
        lvlText.text = "Level " + (Application.loadedLevel - 1);

        var audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(LevelAnouncement);

        dispenser = GameObject.Find("Dispenser").GetComponent<DispenserController>();

        // Dispense the first potato
        dispenser.Dispense();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
