using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour {
    List<Button> AllButtons = new List<Button> ();
    List<Button> ActiveButtons = new List<Button> ();

    private void Awake () {
        RegisterAllChildButtons ();
    }

    void Start () { }

    Button GetFirsActiveButton () {
        if (ActiveButtons.Count > 0)
            return (ActiveButtons[0]);
        return null;
    }

    void RegisterAllChildButtons () {

        int count = transform.childCount;
        for (int i = 0; i < count; i++) {
            if (transform.GetChild (i).gameObject.GetComponent<Button> () != null) {
                RegisterButton (transform.GetChild (i).gameObject.GetComponent<Button> ());
            }
        }
    }

    void RegisterButton (Button btn) {
        AllButtons.Add (btn);
    }

    public void OnClickAnyButton (Button b) {

        ActiveButtons.ForEach (b => b.interactable = true);
        b.interactable = false;
        ActiveButtons.Add (b);

    }

    public void SetButtonActive (int index) { // here active means non interactable

        OnClickAnyButton(AllButtons[index]);
        
    }
}