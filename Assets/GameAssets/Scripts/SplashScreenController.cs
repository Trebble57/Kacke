using UnityEngine;
using System.Collections;

public class SplashScreenController : MonoBehaviour {
    [Header("Settings")]
    [Tooltip("The time in seconds that must pass before the next scene is loaded")]
    public int WaitTime = 5;
    [Tooltip("The length of the FadeOut animation")]
    public int WaitForAnimTime = 1;
    [Tooltip("The Name of the Level that should be loaded afterwards.")]
    public string NextLevel;

    private float timer;                // Will be decremented each Frame
    private bool done = false;          // Indicates whether load of MenuScene has finished
    private AsyncOperation async;       // The AsyncOperation when loading the MenuScene
    private bool loadStarted = false;   // Indicates whether LoadLevelAsync has started yet

	// Use this for initialization
	void Start () {
        timer = WaitTime; // Set initial value for timer
	}
	
	// Update is called once per frame
	void Update () {
        if (!loadStarted)
            StartCoroutine("loadStuff"); // If it has not been done already, start loading of the MainMenuScene
        timer -= Time.deltaTime; // Decrease timer by one

        if (timer > 0)
            return; // Skip the rest if timer is not zero, as the timer is the minimum wait time

        if (done)
        {
            GameObject.Find("SplashImage").GetComponent<Animator>().Play("FadeOutImage"); //Fade out SplashImage
            StartCoroutine("waitForAnim"); //Wait for WaitForAnimTime
        }
	}

    IEnumerator loadStuff()
    {
        loadStarted = true; // Indicate loading has started
        async = Application.LoadLevelAsync(NextLevel); // start the  actual loading
        async.allowSceneActivation = false; // Prevent Unity from showing the scene when the loading has finished
        yield return async.isDone; // yield if loading is in progress
        done = true;    // otherwise set done to true
    }

    IEnumerator waitForAnim()
    {
        yield return new WaitForSeconds(WaitForAnimTime); // yield if time has not run out
        async.allowSceneActivation = true;  // else display menuScene
    }
}
