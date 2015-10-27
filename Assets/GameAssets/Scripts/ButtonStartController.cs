using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonStartController : MonoBehaviour {
    Button b;

    void Start()
    {
        b = GetComponent<Button>();
        b.onClick = new Button.ButtonClickedEvent();
        b.onClick.AddListener(() => { NewGame(); });
    }

    public void NewGame()
    {
        Application.LoadLevel("Level 1");
    }
   
}
