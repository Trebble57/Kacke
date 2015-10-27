using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonQuitController : MonoBehaviour {
    private Button btn;

	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => { Application.Quit(); });
	}
}
