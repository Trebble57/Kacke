using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonStartController : MonoBehaviour {
    Button _b;
    private AudioSource _aSource;
    private MenuSounds _sounds;

    void Start()
    {
        _sounds = GameObject.Find("LC").GetComponent<MenuSounds>();
        _aSource = GameObject.Find("LC").GetComponent<AudioSource>();
        _b = GetComponent<Button>();
        _b.onClick = new Button.ButtonClickedEvent();
        _b.onClick.AddListener(() => { NewGame(); });
    }

    public void NewGame()
    {
        Application.LoadLevel("Level 1");
    }

    public void OnHover()
    {
        if (_b.interactable)
            _aSource.PlayOneShot(_sounds.ButtonNew);
    }

}
