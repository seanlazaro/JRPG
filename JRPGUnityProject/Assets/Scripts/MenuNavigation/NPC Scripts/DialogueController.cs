using UnityEngine;
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
    public string[] dialogue;
    public int[] numberOfChoices;
    public int[] startIndexInChoices;
    public int[] dialogueEffect;

    // The elements of the final two arrays with the same index correspond to each other.
    // Each choice in the choices array will have a corresponding index to which to skip to in the 
    // dialogue array.
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

	// Value is changed to true when user advances dialogue.
	// It will be set back to false after dialogue state changes.
    // NOTE: This functions very well because the WaitUntil function is evaluated right after the update function.
	bool advance = false;

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
	}

	public IEnumerator StartDialogue(){
		player.GetComponent<PlayerMovementController> ().EnableMovement (false);
		pauseMenu.GetComponent<PauseMenu> ().ToggleTalking ();
		talking = true;
		currentTextIndex = 0;


        while (talking)
        {
            currentText = dialogue[currentTextIndex];

            if (dialogueEffect[currentTextIndex] != 0)
            {
                effectTriggered = true;
            }

            if (numberOfChoices[currentTextIndex] != 0)
            {               
                int numChoices = numberOfChoices[currentTextIndex];
                int startIndex = startIndexInChoices[currentTextIndex];

                string[] choiceButtonLabels = new string[numChoices];
                int[] nextIndexAfterChoice = new int[numChoices];

                for (int i = 0; i < numChoices; i++)
                {
                    choiceButtonLabels[i] = choices[startIndex + i];
                    nextIndexAfterChoice[i] = nextIndexInDialogue[startIndex + i];
                }

                // Test code:
                for (int i = 0; i < choiceButtonLabels.Length; i++)
                {
                    Debug.Log(String.Format("Choice {0}: {1} (next index: {2})", i,
                        choiceButtonLabels[i], nextIndexAfterChoice[i]));
                }

                // Insert a function call in here that takes choiceButtonLabels and 
                // nextIndexAfterChoice as arguments. It displays as many buttons as the length of
                // choiceButtonLabels. This function will use the CurrentTextIndex property to
                // change currentTextIndex according to the nextIndexAfterChoice value corresponding
                // to the choice that the player picked. 
            }

            // Gives player 0.2 seconds to let go of space key.
            yield return new WaitForSeconds(0.2f);
            advance = false;

            yield return new WaitUntil(() => advance);
            advance = false;

            if (currentTextIndex + 1 < dialogue.Length)
            {
                if(dialogue[currentTextIndex + 1] == "")
                {
                    talking = false;
                }
                else
                {
                    currentTextIndex++;
                }
            }
        }
        

		pauseMenu.GetComponent<PauseMenu> ().ToggleTalking ();
		player.GetComponent<PlayerMovementController> ().EnableMovement (true);

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

			
			//Display a name tag for the NPC.
            
			// The text will be a part of the box, to allow customization seperate from the label text.
			GUI.skin.box.fontSize = Screen.width / 30;

			// Box will be lined up with the box around the picture, in width and location.
			GUI.BeginGroup (new Rect (locationX, locationY - buttonHeight * 1.5f, height, buttonHeight * 1.5f));
			GUI.Box(new Rect(8, 8, height - 16, (buttonHeight + 4) * 1.5f), npcName );
			GUI.EndGroup ();
		}
	}

	void Update(){
        if (Input.GetKeyUp(KeyCode.Space) && talking)
        {
            advance = true;
        }
	}
}
