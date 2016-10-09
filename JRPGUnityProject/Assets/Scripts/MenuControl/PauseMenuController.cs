using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenuController : MonoBehaviour {

	// Basic array of objects loaded.
	GameObject player;
	GameObject instructionButton;
	GameObject[] pauseObjects;
	GameObject[] instructionMenu;
	GameObject instructionReturnButton;

	bool paused = true;
	bool instructionsDisplayed = false;

	// Prevents talking.
	bool talkingEnabled = true;

	public void ToggleTalking()
	{
		talkingEnabled = !talkingEnabled;
	}

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player");
		pauseObjects = GameObject.FindGameObjectsWithTag ("Pause Menu");
		instructionButton = GameObject.Find("Pause Menu/Instructions");
		instructionMenu = GameObject.FindGameObjectsWithTag ("Instruction Menu");
		instructionReturnButton = GameObject.FindGameObjectWithTag ("Instruction Button");

        // The pause menu must start as active, because one cannot find the Game Object
        // if it is inactive.
	    TogglePauseMenu ();        
	}

	void Start() {

        if (SceneTransitionManager.Instance.PreviousScene != "TitleMenu")
        {
            foreach(GameObject i in instructionMenu)
                i.SetActive (false);
            instructionReturnButton.SetActive (false);
        }
        else
        {
            Debug.Log("show close button instead of back button");
        }
        
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (talkingEnabled && !instructionsDisplayed) {
				TogglePauseMenu ();
			}
		}
		if (EventSystem.current.currentSelectedGameObject == null && paused)
			SelectProperButton();
	}

	// Takes all gameobjects in the pauseobjects array, 
	// and sets their to the bool passed to the method's.
	public void TogglePauseMenu () {
		paused = !paused;

		foreach (GameObject i in pauseObjects) {
			i.SetActive (paused);
		}

		if (paused)
			SelectProperButton();

		player.GetComponent<PlayerSpriteController> ().EnableMovement (!paused);
	}

	public void ToggleInstructionsMenu () {
		instructionsDisplayed = !instructionsDisplayed;

        foreach (GameObject i in instructionMenu)
        {
            i.SetActive(instructionsDisplayed);
        }
			
		instructionReturnButton.SetActive (instructionsDisplayed);

        foreach (GameObject i in pauseObjects)
        {
            i.SetActive(!instructionsDisplayed);
        }

        if (!instructionsDisplayed)
            EventSystem.current.SetSelectedGameObject(instructionButton);
        else

        SelectProperButton();
	}

	void SelectProperButton()
	{
		if (paused) {
			EventSystem.current.SetSelectedGameObject (null);
			if (instructionsDisplayed)
				EventSystem.current.SetSelectedGameObject (instructionReturnButton);
			else
				EventSystem.current.SetSelectedGameObject (EventSystem.current.firstSelectedGameObject);
		}
	}

	public void ExitGame(){
        SceneTransitionManager.Instance.ReturnToTitleScreen();
	}
}
