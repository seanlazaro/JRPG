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
	GameObject instructionScrollButton;

	GameObject[] instructionText;
	int currentInstructionText;

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
		instructionScrollButton = GameObject.FindGameObjectWithTag ("Instruction Scroll Button");

		instructionText = new GameObject[3];
        
		instructionText[0] = GameObject.Find ("Instructions Text A");
		instructionText[1] = GameObject.Find ("Instructions Text B");
		instructionText[2] = GameObject.Find ("Instructions Text C");

		// The pause menu must start as active, 
		//because you can't find an inactive game object.
	    TogglePauseMenu ();        
	}

	IEnumerator Start() {
		
        if (SceneTransitionManager.Instance.PreviousScene != "TitleMenu")
		{
            foreach (GameObject i in instructionMenu)
                i.SetActive(false);
            instructionReturnButton.SetActive(false);
            instructionScrollButton.SetActive(false);

            yield break;
        }
        else
        {
            yield return new WaitForEndOfFrame();
            
            TogglePauseMenu ();
            ToggleInstructionsMenu ();

            player.GetComponent<PlayerSpriteController> ().EnableMovement(false);
            SelectProperButton ();
        }
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (talkingEnabled && !instructionsDisplayed) {
				TogglePauseMenu ();
			}
		}
		if (EventSystem.current.currentSelectedGameObject == null && (paused || instructionsDisplayed))
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
		if (instructionsDisplayed) {
			TogglePauseMenu ();
		}

		// Undoes toggle pause menu movement change, and allows player to move after closing instructions.
		player.GetComponent<PlayerSpriteController> ().EnableMovement (!instructionsDisplayed);

		foreach (GameObject i in instructionMenu) {
			i.SetActive (instructionsDisplayed);
		}

		foreach (GameObject i in instructionText) {
			i.SetActive (false);
		}

		if (instructionsDisplayed) {
			instructionText [0].SetActive (true);
		}
		instructionReturnButton.SetActive (instructionsDisplayed);
		instructionScrollButton.SetActive (instructionsDisplayed);

		SelectProperButton();
	}

	public void ScrollInstructionsMenu() {
		// hides current instruction text.
		instructionText [currentInstructionText].SetActive (false);

		// Cycles current instruction text, then enables the next one.
		if (currentInstructionText != instructionText.Length - 1)
			currentInstructionText++;
		else
			currentInstructionText = 0;

		// shows new one.
		instructionText [currentInstructionText].SetActive (true);
	}

	void SelectProperButton()
	{
		if (paused || instructionScrollButton) {
			EventSystem.current.SetSelectedGameObject (null);
			if (instructionsDisplayed) {
				EventSystem.current.SetSelectedGameObject (instructionReturnButton);
			}
			else
				EventSystem.current.SetSelectedGameObject (EventSystem.current.firstSelectedGameObject);
		}
	}

	public void ExitGame(){
        SceneTransitionManager.Instance.ReturnToTitleScreen();
	}
}
