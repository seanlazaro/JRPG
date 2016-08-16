using UnityEngine;
using System.Collections;

public class DialogueController : MonoBehaviour {

	// Only used to enable and disable player movement.
	GameObject player;

	// Used to disable pausing.
	GameObject pauseMenu;

	// Stores npc image, it should be a square.
	public Texture image;
	public string npcName;

	// If a variable is set, then the

	//Stores text style info for dialogue text, will be edited in the inspector.
	public GUISkin textStyle;

	// Stores the entire dialogue tree.
	[Header("Up to [8], [9] - null, [10] - closer")]
	public string[] dialogueArray;

	public string[] choices;
	public string[] branchA;
	public string[] branchB;
	public string[] branchC;
	public string[] branchD;

	public string[][] brallsls;

	[Header("Dont change.")]
	public bool triggerOtherScripts;

	// Stores the current line to be displayed.
	string currentText;
	int currentTextIndex = 0;
	/*
	The main branch array must folow this format, unless there is no closing line:
		0-8   Primary dialogue branch
		9     Must be empty
		10    Closing line
	(If there is no closing line, the array can hold up to 9 lines (0-8))
	(Branches may be as long as desired, as long as the last element is empty)
	The choices array will be hard coded to use a certain array format:
 		0   NPC context text for dialogue branches
 		1-4 Choices
	*/

	// Used to control what is happening.
	enum dialogueState{
		MainBranch=1,
		PlayerChoice,
		BranchOne,
		BranchTwo,
		BranchThree,
		BranchFour,
		Closer,
		End
	}
	int currentDialogueState;


	bool talking;
	bool playerChoice = false;
	// Value is changed to true when user advances dialogue.
	// It will be set back to false after dialogue state changes.
	bool advance = false;
	// NOTE: This functions very well because the WaitUntil function is evaluated right after the update function.


	// Used when there are multiple dialogue branches.
	bool dialogueHasBranches;

	// Used to control the size of main dialogue box.
	float width;
	float height;
	float locationX;
	float locationY;

	float buttonWidth;
	float buttonHeight;
	Rect button1Location;
	Rect button2Location;
	Rect button3Location;
	Rect button4Location;

	// Use this for initialization
	void Start () {
		// Used to disable movement.
		player = GameObject.FindWithTag("Player");
		// Used to disable pausing
		pauseMenu = GameObject.FindWithTag("Pause Menu Canvas");

		// If the choices array length is over 0, there must be multiple dialogue branches.
		if (choices.Length > 0) {
			dialogueHasBranches = true;
		}
		// Triggered when the choices array has 0 elements, meaning there are no dialogue branches.
		else {
			dialogueHasBranches = false;
		}

		// Sets dimensions of the main dialogue box.
		width = Screen.width / 20 * 19;
		height = Screen.height / 5 * 2;
		locationX = (Screen.width - width) / 2;
		locationY = Screen.height - height - 10;
		//Dimensions of player choice buttons.
		buttonWidth = width / 4 - 20;
		buttonHeight = height / 4 - 10;
		button1Location = new Rect (0, 0, buttonWidth, buttonHeight);
		button2Location = new Rect (buttonWidth + 10, 0, buttonWidth, buttonHeight);
		button3Location = new Rect (buttonWidth + 10 * 2, 0, buttonWidth, buttonHeight);
		button4Location = new Rect (buttonWidth + 10 * 3, 0, buttonWidth, buttonHeight);

		// Makes sure that the other scripts aren't triggered before dialogue
		triggerOtherScripts = false;
	}

	public IEnumerator StartDialogue(){
		player.GetComponent<PlayerMovementController> ().EnableMovement (false);
		pauseMenu.GetComponent<PauseMenu> ().ToggleTalking ();
		talking = true;
		currentDialogueState = (int)dialogueState.MainBranch;
		currentTextIndex = 0;

		// Gives player 0.2 seconds to let go of space key.
		yield return new WaitForSeconds (0.2f);


		pauseMenu.GetComponent<PauseMenu> ().ToggleTalking ();
		player.GetComponent<PlayerMovementController> ().EnableMovement (true);

		// Other Scripts
		Debug.Log("Other Scripts Triggered");
		triggerOtherScripts = true;


		yield break;
	}

	void OnGUI(){
		if (talking) {
		
			// Edit the GUI Skin to change the text's style.
			GUI.skin = textStyle;

			// Changing the divisor will change the font size ratio,
			GUI.skin.label.fontSize = Screen.width / 20;

			GUI.BeginGroup(new Rect(locationX, locationY, width, height));
				// The big box that contains the dialogue and the 
				GUI.Box (new Rect(0, 0, width, height), "");
		
				// Displays box with same dimensions as picture to give border.
				GUI.Box(new Rect(8, 8, height-16, height-16), "");
				// Displays image ten pixels away from the top, bottom, and left sides, scaling the image to fit.
				GUI.DrawTexture(new Rect(10, 10, height-20, height-20),  image, ScaleMode.StretchToFill, true, 1);
	
				// Displays text 10 pixels away from the top, bottom, and 50 pixels to the right of the picture.
				// The width is set to width - height + 30, because it should be as wide as the box, minus the
				// width of the picture(20), and with a 10 pixel buffer on both sides(20).
			// (GUI.skin.label.fontSize * 4) is added to prevent words overflowing too far, without clipping
				GUI.Label(new Rect(height + 40, 10, width - (height - 40 + GUI.skin.label.fontSize * 4), height - 20), currentText);
				GUI.EndGroup();

			// Player choice will be set to true in the playerchoice state, after the main branch.
			if (playerChoice && choices.Length != 0) {

				// The buttons will be about a quarter of the width of the text box,
				// so they will fit just above the dialogue box.
				GUI.BeginGroup (new Rect (locationX, locationY - buttonHeight - 10, width + 40, buttonHeight));

				// If there is a choice, there must be at least two buttons,
				// so there is no need to check the first two buttons.
						
				// -Button One-
				if (GUI.Button (button1Location, choices [1])) {
					currentDialogueState = (int)dialogueState.BranchOne;
					currentTextIndex = -1;
					playerChoice = false;
				}
				// -Button Two-
				if (GUI.Button (button2Location, choices [2])) {
					currentDialogueState = (int)dialogueState.BranchTwo;
					currentTextIndex = -1;
					playerChoice = false;
				}
				// -Button Three-
				if (choices.Length > 3) {
					if (GUI.Button (button3Location, choices [3])) {
						currentDialogueState = (int)dialogueState.BranchThree;
						currentTextIndex = -1;
						playerChoice = false;
					}
				}
				// -Button Four-
				if (choices.Length > 4) {
					
					if (GUI.Button (button4Location, choices [4])) {
						currentDialogueState = (int)dialogueState.BranchFour;
						playerChoice = false;
						currentTextIndex = -1;
					}
				}
				GUI.EndGroup ();
			} 

			// If there is no player choice going on, display a name tag for the NPC.
			else {
				// The text will be a part of the box, to allow customization seperate from the label text.
				GUI.skin.box.fontSize = Screen.width / 30;

				// Box will be lined up with the box around the picture, in width and location.
				GUI.BeginGroup (new Rect (locationX, locationY - buttonHeight * 1.5f, height, buttonHeight * 1.5f));
				GUI.Box(new Rect(8, 8, height - 16, (buttonHeight + 4) * 1.5f), npcName );
				GUI.EndGroup ();
			}

		}
	}

	// This bool is used to prevent the text from advancing multiple times at the start.
	bool spacePress = false;

	void Update(){

		if (Input.GetKeyDown (KeyCode.Space) && currentDialogueState != (int)dialogueState.PlayerChoice && talking && !spacePress) {
			spacePress = true;
		}

		// Advances dialogue.
		if (Input.GetKeyUp (KeyCode.Space) && currentDialogueState != (int)dialogueState.PlayerChoice && talking && spacePress) {
			advance = true;
			spacePress = false;
		}
	}
}
