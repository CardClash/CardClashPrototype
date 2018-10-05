using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour {
    public EventSystem eventSystem;
    public GameObject selectedObject;

    private bool buttonSelected;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        //If you move vertically, select the first button
		if(Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
	}
    //de-select when you leave the menu
    private void OnDisable()
    {
        buttonSelected = false;
    }
   
}
