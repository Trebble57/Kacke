using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour {

    [Tooltip("Maximum time Minutes")]
    public int TotalGameMinutes = 2;
    [Tooltip("Maximum time Seconds")]
    public int TotalGameSeconds = 0;
    [Tooltip("Text to display time on")]
    public Text TimeDisplay;
    [Tooltip("Base string for text before time left")]
    public string BaseString = "Time left: ";
    public bool Enabled = true;

    public TimeSpan TotalGameTime;
    private GameOverController goC;

	// Use this for initialization
	public void Start () {
        TotalGameTime = new TimeSpan(0, 0, TotalGameMinutes, TotalGameSeconds);
        InvokeRepeating("countDown", 1, 1);   
	}

    // Counts Down one second and displays current time left on the specified text object
    void countDown ()
    {
        TimeDisplay.text = BaseString + " " + TotalGameTime.Minutes + ":" + TotalGameTime.Seconds;
        if (!Enabled)
            return;
        if (TotalGameTime > TimeSpan.Zero)
        {
            TotalGameTime = TotalGameTime.Subtract(new TimeSpan(0, 0, 0, 1, 0));
        }
        else
        {
            GameObject.Find("OrthoFollowResetCamera").GetComponent<GameOverController>().TimeOut();
        }
    }
}
