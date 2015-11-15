using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameOverController : MonoBehaviour {
    
    [Header("Required")]
    [Tooltip("The projectile that will be monitored.")]
    public Rigidbody2D Projectile;          //	The rigidbody of the projectile
    [Tooltip("The Canvas that will be activated on \"Game Over\"")]
    public GameObject GameOverCanvas;           //  The Canvas that contains the GameOverScreen. Can NOT be null.
    public Text TextTimeScore;
    public Text TextAmmoScore;
    public Text TextTotalScore;
    [Header("Optional")]
    [Tooltip("The speed value below which the GameOver function will be called")]
    public float ResetSpeed = 0.025f;       //	The angular velocity threshold of the projectile, below which our game will reset
    [Tooltip("The Text that will display the loading progress of the next level.")]
    public Text LvlLoadText;                //  The text that displays the loading status. Can be null.
    [Tooltip("Amount of score points for every potato left")]
    public int ScorePerPotato = 10;
    [Tooltip("Amount of score points for every second left")]
    public int ScorePerSecond = 2;

    private float _resetSpeedSqr;            //	The square value of Reset Speed, for efficient calculation
    private SpringJoint2D _spring;           //	The SpringJoint2D component which is destroyed when the projectile is launched
    private AsyncOperation _lvlPreloadAction;//  Contains AsyncOperation object when loading of next level via Application.LoadLevelAsync is called
    private TimeCounter _timer;
    private SoundAssets _sounds;
    private AudioSource _aSource;

    void Start()
    {
        if (GameOverCanvas == null)
        {
            if (Debug.isDebugBuild)
                throw new GameOverCanvasNotSetException();
            else
                Application.Quit();
        }

        TextTimeScore = GameObject.Find("valTimeLeft").GetComponent<Text>();
        TextAmmoScore = GameObject.Find("valAmmoLeft").GetComponent<Text>();
        TextTotalScore = GameObject.Find("valTotalScore").GetComponent<Text>();
        _timer = GameObject.Find("LC").GetComponent<TimeCounter>();
        _sounds = GameObject.Find("LC").GetComponent<SoundAssets>();
        _aSource = GameObject.Find("Dispenser").GetComponent<AudioSource>();

        // Set current levelIndex, so the Player can continue the game when he returns.
        PlayerPrefs.SetInt("LastLevel", Application.loadedLevel);
        //	Calculate the Resset Speed Squared from the Reset Speed
        _resetSpeedSqr = ResetSpeed * ResetSpeed;



        //	Get the SpringJoint2D component through our reference to the GameObject's Rigidbody
        _spring = Projectile.GetComponent<SpringJoint2D>();

    }

    void Update()
    {
        //	If we hold down the "R" key...
        if (Input.GetKeyDown(KeyCode.R))
        {
            //	... call the Reset() function
            Reset();
        }

        if (Projectile == null)
            return;

        //	If the spring had been destroyed (indicating we have launched the projectile) and our projectile's velocity is below the threshold...
        if (_spring == null && Projectile.velocity.sqrMagnitude < _resetSpeedSqr)
        {
            //	remove projectiles and call the Reset() function
            Destroy(Projectile.gameObject, 0f);
            Reset();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // NOT required as we have seperate colliders at the edges
        ////	If the projectile leaves the Collider2D boundary...
        //if (other.GetComponent<Rigidbody2D>() == projectile)
        //{
        //    //	... call the Reset() function
        //    Reset();
        //}
    }

    void Reset()
    {
        
        //	The reset function will determine whether all enemies have been destroyed or not.
        //  In case there are enemies left, we reset the level - otherwise we display the GameOverCanvas
        var enemies = GameObject.FindGameObjectsWithTag("Target");
        if (enemies.Length > 0)
        {
            //  There are still enemies in the scene => no cookie for you (check for ammo left and reset)!

            var projectiles = GameObject.FindGameObjectsWithTag("Player");
            if (projectiles.Length-1 > 0)
            {
                var cam = GameObject.Find("OrthoFollowResetCamera");
                DestroyImmediate(cam.GetComponent<ProjectileFollow>());
                var pFollow = cam.AddComponent<ProjectileFollow>();
                pFollow.farLeft = GameObject.Find("MarkerLeft").transform;
                pFollow.farRight = GameObject.Find("MarkerRight").transform;
                pFollow.projectile = projectiles[projectiles.Length-1].transform;
                GameObject.Find("Dispenser").GetComponent<DispenserController>().Dispense();
                return;
            }
            _aSource.PlayOneShot(_sounds.LevelFailed);
            StartCoroutine("reloadLevel");
            return;
        }

        _aSource.PlayOneShot(_sounds.LevelSuccess);

        //  At this point there are no enemies in the scene. Let's check whether this is the last level.
        if (Application.levelCount > Application.loadedLevel + 1)
        {
            // If the total level count of this application is smaller than the number of the current level
            // there is still at least a level left. (We need to add 1 'cause the levelIndex is zero based)

            // Enable Canvas and Animator component so it will start playing the FadeIn animation
            var cv = GameOverCanvas.GetComponent<Canvas>();
            cv.enabled = true;
            var an = GameOverCanvas.GetComponent<Animation>();
            an.enabled = true;
            CountScore();
            GameObject.Find("btnNext").GetComponent<Button>().interactable = true;
            GameObject.Find("btnQuit").GetComponent<Button>().interactable = true;

            PlayerPrefs.SetInt("LastLevel", Application.loadedLevel + 1);
            return;
        }


        // This code section will only be reached when this is the last level as all  other blocks have return statements
        GameOverCanvas.GetComponent<Canvas>().enabled = true;
        GameOverCanvas.GetComponent<Animation>().enabled = true;

        CountScore();

        if (LvlLoadText != null) // If LvlLoadText has been set in the editor, we'll tell the player that he has beat the game
            LvlLoadText.text = "Congrats! You finished the game!";

        // This time we only enable the menu button as there are no other levels
        GameObject.Find("btnQuit").GetComponent<Button>().interactable = true;
        PlayerPrefs.DeleteKey("LastLevel"); // Delete stored lastLevel count as player beat all levels.
    }

    int timeScore = 0;

    void CountScore()
    {
        _timer.Enabled = false;
        _timer.TotalGameTime = _timer.TotalGameTime.Subtract(new TimeSpan(0, 0, 0, 1));
        timeScore += ScorePerSecond;
        int length = GameObject.FindGameObjectsWithTag("Player").Length;
        TextAmmoScore.text = "" + length * ScorePerPotato;
        TextTimeScore.text = timeScore.ToString();
        TextTotalScore.text = "" + length * ScorePerPotato * timeScore;
    }

    /// <summary>
    /// Coroutine for loading level asynchronously
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator loadNextLvl()
    {
        GameObject.Find("btnNext").GetComponent<Button>().interactable = false;
        GameObject.Find("btnQuit").GetComponent<Button>().interactable = false;
        _lvlPreloadAction = Application.LoadLevelAsync(Application.loadedLevel + 1); // Start the actual loading


        if (LvlLoadText != null) // If LvlLoadText has been set in the editor, we'll display the progress.
            LvlLoadText.text = "Loading next level... (" + _lvlPreloadAction.progress * 100 + "%)";
        yield return _lvlPreloadAction.isDone; // Yield to next frame if load has not finished 
    }

    IEnumerator reloadLevel()
    {
        yield return new WaitForSeconds(2f); // Wait 'till sound has played 
        Application.LoadLevel(Application.loadedLevel);
    }
    /// <summary>
    /// Function to be called by the Next-Level-Button on the GameOverCanvas
    /// </summary>
    public void OnNextLevelClick()
    {
        StartCoroutine("loadNextLvl");
    }

    /// <summary>
    /// Function to be called by the Main-Menu-Button on the GameOverCanvas
    /// </summary>
    public void OnQuitClick()
    {
        Application.LoadLevel(1); //MainMenu.scene
    }

    public void TimeOut()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}


/// <summary>
/// Custom exception derived class to abort execution and display a kind reminder.
/// </summary>
public class GameOverCanvasNotSetException : Exception
{
    public GameOverCanvasNotSetException()
        : base("The GameOverCanvas has not been set. Please be so kind and set it in the Unity editor, will ya?")
    {
        
    }
}
