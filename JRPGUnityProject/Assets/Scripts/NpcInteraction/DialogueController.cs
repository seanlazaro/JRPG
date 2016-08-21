using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class DialogueController : MonoBehaviour {

	// Only used to enable and disable player movement.
	GameObject player;

	// Used to disable pausing.
	GameObject pauseMenu;

	// Stores npc image, it should be a square.
	public Texture image;
	public string npcName;

	//Stores text style info for dialogue text, will be edited in the inspector.
	public GUISkin textStyle;

    // Linear text iterates through the dialogue array.
    // An empty string causes the dialogue to end.
    // The elements of the first four arrays with the same index correspond to each other.
    // The number of choices is 0 for linear text.
    // If the number of choices is greater than 0 then the start index in the choices array will be
    // a valid value that indicates which index to start reading from in the choices array.
    // Dialogue effect indicates which block of code to execute in the the npc effects script.
    // If dialogue effect is 0, then there is no effect.
    
	[Header("Next Four Must Be Equal Size")]
	public string[] dialogue;
    public int[] numberOfChoices;
    public int[] startIndexInChoices;
    public int[] dialogueEffect;

    // The elements of the final two arrays with the same index correspond to each other.
    // Each choice in the choices array will have a corresponding index to which to skip to in the 
    // dialogue array.
	[Header("Next Two Must Be Equal Size")]
	public string[] choices;
    public int[] nextIndexInDialogue;

    public bool effectTriggered = false;

	// Stores the current line to be displayed.
	string currentText;
	int currentTextIndex;
    public int CurrentTextIndex
    {
        get { return currentTextIndex; }
        set {
            if (value < dialogue.Length)
            {
                currentTextIndex = value;
            }
            else
            {
                Debug.Log("Attempted to change currentTextIndex to invalid value.");
            }
        }
    }

	bool talking = false;
	// Choice Stuff
	bool choosing = false;


	// Each NPC must have its own dialogue choice buttons,
	// So create a transparent panel and duplicate all buttons into it to sort.
	public GameObject DialogueChoiceMenu;

	GameObject ButtonOne;
	int buttonOneDestination;
	GameObject ButtonTwo;
	int buttonTwoDestination;
	GameObject ButtonThree;
	int buttonThreeDestination;
	GameObject ButtonFour;
	int buttonFourDestination;

	// Value is changed to true when user advances dialogue.
	// It will be set back to false after dialogue state changes.
    // NOTE: This functions very well because the WaitUntil function is evaluated right after the update function.
	bool advance = false;

	// Used to control the size of main dialogue box.
	float width;
	float height;
	float locationX;
	float locationY;

	float NpcNameHeight;

	// Use this for initialization
	void Start () {
		// Used to disable movement.
		player = GameObject.FindWithTag("Player");
		// Used to disable pausing
		pauseMenu = GameObject.FindWithTag("Pause Menu Canvas");

		ButtonOne = GameObject.Find ("DialogueButtonOne");
		ButtonTwo = GameObject.Find ("DialogueButtonTwo");
		ButtonThree = GameObject.Find ("DialogueButtonThree");
		ButtonFour = GameObject.Find ("DialogueButtonFour");

		ButtonOne.GetComponent<Button> ().onClick.RemoveAllListeners ();
		ButtonTwo.GetComponent<Button> ().onClick.RemoveAllListeners ();
		ButtonThree.GetComponent<Button> ().onClick.RemoveAllListeners ();
		ButtonFour.GetComponent<Button> ().onClick.RemoveAllListeners ();

		// The strange lamda usage is allows methods to accept arguments in the future.
		ButtonOne.GetComponent<Button> ().onClick.AddListener (()=>{ButtonOnePress();});
		ButtonTwo.GetComponent<Button> ().onClick.AddListener (()=>{ButtonTwoPress();});
		ButtonThree.GetComponent<Button> ().onClick.AddListener (()=>{ButtonThreePress();});
		ButtonFour.GetComponent<Button> ().onClick.AddListener (()=>{ButtonFourPress();});


		// Sets dimensions of the main dialogue box.
		width = Screen.width / 20 * 19;
		height = Screen.height / 5 * 2;
		locationX = (Screen.width - width) / 2;
		locationY = Screen.height - height - 10;
		//Dimensions of player choice buttons.
		NpcNameHeight = height / 4 - 10;

		if(DialogueChoiceMenu != null)
		DialogueChoiceMenu.SetActive (false);
	}

	public IEnumerator StartDialogue(){
		player.GetComponent<PlayerSpriteController> ().EnableMovement (false);
		pauseMenu.GetComponent<PauseMenuController> ().ToggleTalking ();
		talking = true;
		currentTextIndex = 0;


        while (talking)
        {
            currentText = dialogue[currentTextIndex];

            if (dialogueEffect[currentTextIndex] != 0)
            {
                effectTriggered = true;
            }

			if (numberOfChoices [currentTextIndex] != 0) {               
				int numChoices = numberOfChoices [currentTextIndex];
				int startIndex = startIndexInChoices [currentTextIndex];

				string[] choiceButtonLabels = new string[numChoices];
				int[] nextIndexAfterChoice = new int[numChoices];

				for (int i = 0; i < numChoices; i++) {
					choiceButtonLabels [i] = choices [startIndex + i];
					nextIndexAfterChoice [i] = nextIndexInDialogue [startIndex + i];
				}

				// Displays buttons.
				for (int i = 0; i < choiceButtonLabels.Length; i++) {
					switch (i) {
					case 0:
						ButtonOne.SetActive (true);
						ButtonOne.GetComponentInChildren<Text> ().text = choiceButtonLabels [i];
						buttonOneDestination = nextIndexAfterChoice [i];
						break;
					case 1:
						ButtonTwo.SetActive(true);
						ButtonTwo.GetComponentInChildren<Text> ().text = choiceButtonLabels [i];
						buttonTwoDestination = nextIndexAfterChoice [i];
						break;
					case 2:
						ButtonThree.SetActive(true);
						ButtonThree.GetComponentInChildren<Text> ().text = choiceButtonLabels [i];
						buttonThreeDestination = nextIndexAfterChoice [i];
						break;
					case 3:
						ButtonFour.SetActive(true);
						ButtonFour.GetComponentInChildren<Text> ().text = choiceButtonLabels [i];
						buttonFourDestination = nextIndexAfterChoice [i];
						break;
					default:
						Debug.Log ("Something went wrong in dialogue choice.");
						break;
					}
				}

				choosing = true;
				DialogueChoiceMenu.SetActive (true);
				EventSystem.current.SetSelectedGameObject (ButtonOne);
				yield return new WaitUntil (() => !choosing);
				DialogueChoiceMenu.SetActive (false);
			} 
			else {
				advance = false;
				yield return new WaitUntil (() => advance);
				advance = false;

				if (currentTextIndex + 1 < dialogue.Length) {
					if (dialogue [currentTextIndex + 1] == "") {
						talking = false;
					} else {
						currentTextIndex++;
					}
				}
			}
			// Gives player 0.2 seconds to let go of space key.
			yield return new WaitForSeconds(0.2f);
        }
        

		pauseMenu.GetComponent<PauseMenuController> ().ToggleTalking ();
		player.GetComponent<PlayerSpriteController> ().EnableMovement (true);

		yield break;
	}

	#region Button Clicks

	public void ButtonOnePress()
	{
		currentTextIndex = buttonOneDestination;
		choosing = false;
	}

	public void ButtonTwoPress()
	{
		currentTextIndex = buttonTwoDestination;
		choosing = false;
	}

	public void ButtonThreePress()
	{
		currentTextIndex = buttonThreeDestination;
		choosing = false;
	}

	public void ButtonFourPress()
	{
		currentTextIndex = buttonFourDestination;
		choosing = false;
	}
	#endregion

	// UI Stuff!
	void OnGUI(){
		if (talking) {

			// Edit the GUI Skin to change the text's style.
			GUI.skin = textStyle;

			// Changing the divisor will change the font size ratio,
			GUI.skin.label.fontSize = (int)Math.Floor((double)(Screen.width /20	));
			int LabelFontSize = GUI.skin.label.fontSize;
			GUI.BeginGroup(new Rect(locationX, locationY, width, height));

			// The big box that contains the dialogue and the image.
			GUI.Box (new Rect(0, 0, width, height), "");

			// Displays image ten pixels away from the top, bottom, and left sides, scaling the image to fit.
			GUI.DrawTexture(new Rect(LabelFontSize, LabelFontSize, height-LabelFontSize * 2, height-LabelFontSize * 2),  image, ScaleMode.StretchToFill, true, 1);

			// Displays text 24 pixels away from the top, bottom, and 50 pixels to the right of the picture.
			// The width is set to width - height + 30, because it should be as wide as the box, minus the
			// width of the picture(20), and with a 10 pixel buffer on both sides(20).
			// (GUI.skin.label.fontSize * 4) is added to prevent words overflowing too far, without clipping
			GUI.Label(new Rect(height + 40, 24, width - (height - 40 + GUI.skin.label.fontSize * 4), height - 48), currentText);
			GUI.EndGroup();
			
			//Display a name tag for the NPC.
            
			// The text will be a part of the box, to allow customization seperate from the label text.
			GUI.skin.label.fontSize = Screen.width / 25;

			// Box will be lined up with the box around the picture, in width and location.
			GUI.BeginGroup (new Rect (locationX, locationY - NpcNameHeight * 1.5f, height, NpcNameHeight * 1.5f));
			GUI.Box(new Rect(8, 0, height - LabelFontSize, (LabelFontSize) * 3f), "");
			GUI.Label (new Rect (24, LabelFontSize / 2, height - LabelFontSize - 20, (LabelFontSize) * 3f), npcName);
			GUI.EndGroup ();
		}
	}

	// Prevents skipping first line.
	bool spacePress = false;

	void Update(){
		if (Input.GetKeyDown(KeyCode.Space) && talking && spacePress)
		{
			advance = true;
		}
	}

	void LateUpdate()
	{
		// Put in lateupdate to prevent conflict with pause menu and other types of
		if (EventSystem.current.currentSelectedGameObject == null && choosing) {
			EventSystem.current.SetSelectedGameObject (ButtonOne);
		}		
		if (Input.GetKeyUp(KeyCode.Space) && talking && !spacePress)
		{
			spacePress = true;
		}
	}
}
