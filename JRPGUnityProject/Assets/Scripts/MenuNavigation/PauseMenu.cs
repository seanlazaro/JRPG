using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	// Basic array of objects loaded.
	GameObject player;
	GameObject[] pauseObjects;

	bool paused = true;

	// To prevent pausing.
	// Make sure to reset!
	bool talkingEnabled = true;

	public void ToggleTalking()
	{
		talkingEnabled = !talkingEnabled;
	}

	// Use this for initialization
	void Awake () {
		Debug.Log ("Start called");
		player = GameObject.FindGameObjectWithTag ("Player");
		pauseObjects = GameObject.FindGameObjectsWithTag ("Pause Menu");
		TogglePauseMenu ();

		// Sets onclick for the 
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (talkingEnabled) {
				TogglePauseMenu ();
			}
		}
		if (EventSystem.current.currentSelectedGameObject == null && paused)
			EventSystem.current.SetSelectedGameObject (EventSystem.current.firstSelectedGameObject);
	}

	// Takes all gameobjects in the pauseobjects array, 
	// and sets their to the bool passed to the method's.
	public void TogglePauseMenu () {
		paused = !paused;
		Debug.Log (pauseObjects.Length);
		foreach (GameObject i in pauseObjects) {
			i.SetActive (paused);
		}
		if (paused)
			StartCoroutine ("SelectProperButton");
		player.GetComponent<PlayerMovementController> ().EnableMovement (!paused);
	}

	IEnumerator SelectProperButton()
	{
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
		yield break;
	}

	public void ExitGame(){
		SceneManager.LoadScene ("TitleMenu");
	}
}
