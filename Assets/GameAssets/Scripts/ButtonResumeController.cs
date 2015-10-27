using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonResumeController : MonoBehaviour {
    private Button btn;

	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        btn.interactable = PlayerPrefs.HasKey("LastLevel");
        btn.onClick.AddListener(() =>
        {
            Application.LoadLevel(PlayerPrefs.GetInt("LastLevel"));
        });
    }
}
