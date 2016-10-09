using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class InstructionsScreenController : MonoBehaviour {

	GameObject backButton;
	GameObject nextButton;

	GameObject[] instructionText;
	int currentInstructionText;

	void Awake() {
		backButton = GameObject.Find ("Main Menu Button"); 
		nextButton = GameObject.Find ("Next Page Button");
		instructionText = new GameObject[3];
	}

	// Use this for initialization
	void Start () { 
		backButton.GetComponent<Button> ().onClick.AddListener(() => ReturnToTitle());
		nextButton.GetComponent<Button> ().onClick.AddListener(() => ScrollInstructions());

		instructionText[0] = GameObject.Find ("Instructions Text 1");
		instructionText[1] = GameObject.Find ("Instructions Text 2");
		instructionText[2] = GameObject.Find ("Instructions Text 3");

		foreach (GameObject i in instructionText) {
			Debug.Log (i);
			i.SetActive (false);
		}

		currentInstructionText = 0;
		instructionText [0].SetActive (true);
	}

	void ReturnToTitle() {
		SceneManager.LoadScene ("TitleMenu");
	}

	void ScrollInstructions(){
		// hides current instruction text.
		instructionText [currentInstructionText].SetActive (false);

		// Cycles current instruction text, then enables the next one.
		if (currentInstructionText != instructionText.Length - 1)
			currentInstructionText++;
		else
			currentInstructionText = 0;

		// shows new one.
		instructionText [currentInstructionText].SetActive (true);
		Debug.Log(instructionText[currentInstructionText].GetComponent<Text>().fontSize);
	}

	// Update is called once per frame
	void Update () {
		if (EventSystem.current.currentSelectedGameObject == null) {
			EventSystem.current.SetSelectedGameObject (null);
			EventSystem.current.SetSelectedGameObject (backButton);
		}
	}
}
