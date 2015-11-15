using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelStartController : MonoBehaviour {

    public float                LevelStartTime = 3f;

    private DispenserController _dispenser;
    private SoundAssets         _sounds;
    private TimeCounter         _timeCounter;
    private float               _secToStart;
    private Text                _lvlText;

    // Use this for initialization
    void Start()
    {
        _sounds = GameObject.Find("LC").GetComponent<SoundAssets>();
        _dispenser = GameObject.Find("Dispenser").GetComponent<DispenserController>();
        _timeCounter = GetComponent<TimeCounter>();
        var audioSource = _dispenser.gameObject.GetComponent<AudioSource>();
        _lvlText = GameObject.Find("txtLevel").GetComponent<Text>();
        // Find the level text object if it exists and update it with the current level count.
        // Remember: Scenes "0" and "1" are SplashScreen and MainMenu so the first lvl has the index "2"!
        _lvlText.text = "Level " + (Application.loadedLevel - 1);
        audioSource.PlayOneShot(_sounds.LevelStart);

        _secToStart = LevelStartTime;
        StartCoroutine("WaitOne");

    }

    IEnumerator WaitOne()
    {
        yield return new WaitForSeconds(1.0f);
        InvokeRepeating("StartLevel", 0, 1.0f);
    }

    void StartLevel()
    {

        if (_secToStart == 0)
        {
            _timeCounter.enabled = true;
            // Dispense the first potato
            _dispenser.Dispense();
            _lvlText.enabled = false;
            _secToStart = -1f;
        }
        if (_secToStart > 0)
        {
            _lvlText.text = _secToStart.ToString();
            _secToStart -= 1;
        }
    }
}
