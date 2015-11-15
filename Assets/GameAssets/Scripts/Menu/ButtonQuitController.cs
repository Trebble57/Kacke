using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonQuitController : MonoBehaviour {
    private Button _b;
    private AudioSource _aSource;
    private MenuSounds _sounds;

    // Use this for initialization
    void Start () {
        _sounds = GameObject.Find("LC").GetComponent<MenuSounds>();
        _aSource = GameObject.Find("LC").GetComponent<AudioSource>();
        _b = GetComponent<Button>();
        _b.onClick.AddListener(() => { Application.Quit(); });
	}

    public void OnHover()
    {
        if (_b.interactable)
            _aSource.PlayOneShot(_sounds.ButtonQuit);
    }
}
