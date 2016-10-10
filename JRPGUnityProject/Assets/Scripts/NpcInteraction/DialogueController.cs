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

	public string npcName; //for display
    string npcNameReal; //inherent to the npc

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

    public class DialogueScript{
        public string[] dialogue;
        public int[] numberOfChoices;
        public int[] startIndexInChoices;
        public int[] dialogueEffect;
        public string[] choices;
        public int[] nextIndexInDialogue;
    }

    public bool effectTriggered = false;
    public int effectFunc = 0;

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

	// Prevents skipping first line.
	bool spacePress = false;

	public bool SpacePress {
		set{ spacePress = value; }
	}

	// Each NPC must have its own dialogue choice buttons,
	// So create a transparent panel and duplicate all buttons into it to sort.
	GameObject DialogueChoiceMenu;

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

    // For demo only, states whether or not an accusation has been made at the end of a dialogue.
    bool accused = false;

	// Use this for initialization
	void Awake(){
        
        // Used to disable movement.
		player = GameObject.FindWithTag("Player");

		// Used to disable pausing
		pauseMenu = GameObject.FindWithTag("Pause Menu Canvas");

        Transform dcmParentTransform = GameObject.Find("Dialogue Choice Menu").transform;
        DialogueChoiceMenu = dcmParentTransform.Find("Choices").gameObject;
        ButtonOne = dcmParentTransform.Find("Choices/DialogueButtonOne").gameObject;
        ButtonTwo = dcmParentTransform.Find("Choices/DialogueButtonTwo").gameObject;
        ButtonThree = dcmParentTransform.Find("Choices/DialogueButtonThree").gameObject;
        ButtonFour = dcmParentTransform.Find("Choices/DialogueButtonFour").gameObject;

        npcNameReal = npcName;

	}

	void Start () {

		ButtonOne.GetComponent<Button> ().onClick.RemoveAllListeners ();
		ButtonTwo.GetComponent<Button> ().onClick.RemoveAllListeners ();
		ButtonThree.GetComponent<Button> ().onClick.RemoveAllListeners ();
		ButtonFour.GetComponent<Button> ().onClick.RemoveAllListeners ();

		ButtonOne.SetActive (false);
		ButtonTwo.SetActive (false);
		ButtonThree.SetActive (false);
		ButtonFour.SetActive (false);

		// Sets dimensions of the main dialogue box.
		width = Screen.width / 20 * 19;
		height = Screen.height / 3;
		locationX = (Screen.width - width) / 2;
		locationY = Screen.height - height - 10;
		//Dimensions of player choice buttons.
		NpcNameHeight = height / 4 - 10;

		if(DialogueChoiceMenu != null)
		DialogueChoiceMenu.SetActive (false);
	}

	public IEnumerator StartDialogue(DialogueScript ds = null, int startAtIndex = 0){

        if (ds == null)
        {
            ds = new DialogueScript();
            
            ds.dialogue = dialogue;
            ds.numberOfChoices = numberOfChoices;
            ds.startIndexInChoices = startIndexInChoices;
            ds.dialogueEffect = dialogueEffect;
            ds.choices = choices;
            ds.nextIndexInDialogue = nextIndexInDialogue;
        }

		player.GetComponent<PlayerSpriteController> ().EnableMovement (false);
		pauseMenu.GetComponent<PauseMenuController> ().ToggleTalking ();
		talking = true;
		currentTextIndex = 0;


        while (talking)
        {
            currentText = ds.dialogue[currentTextIndex];

            if (ds.dialogueEffect[currentTextIndex] != 0)
            {
                effectTriggered = true;
                effectFunc = ds.dialogueEffect[currentTextIndex];
            }

			if (ds.numberOfChoices[currentTextIndex] != 0) {               
				int numChoices = ds.numberOfChoices[currentTextIndex];
				int startIndex = ds.startIndexInChoices[currentTextIndex];

				string[] choiceButtonLabels = new string[numChoices];
				int[] nextIndexAfterChoice = new int[numChoices];

				for (int i = 0; i < numChoices; i++) {
					choiceButtonLabels[i] = ds.choices[startIndex + i];
					nextIndexAfterChoice[i] = ds.nextIndexInDialogue[startIndex + i];
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

				ButtonOne.GetComponent<Button> ().onClick.AddListener(() => ButtonOnePress());
				ButtonTwo.GetComponent<Button> ().onClick.AddListener(() => ButtonTwoPress());
				ButtonThree.GetComponent<Button> ().onClick.AddListener(() => ButtonThreePress());
				ButtonFour.GetComponent<Button> ().onClick.AddListener(() => ButtonFourPress());

				choosing = true;
				DialogueChoiceMenu.SetActive (true);
				EventSystem.current.SetSelectedGameObject (null);
				yield return new WaitForEndOfFrame ();
				EventSystem.current.SetSelectedGameObject (ButtonOne);
				yield return new WaitUntil (() => !choosing);
				ButtonOne.SetActive (false);
				ButtonTwo.SetActive (false);
				ButtonThree.SetActive (false);
				ButtonFour.SetActive (false);

				DialogueChoiceMenu.SetActive (false);
			} 
			else {
				advance = false;
				yield return new WaitUntil (() => advance);
				advance = false;

                if (currentTextIndex + 1 < ds.dialogue.Length)
                {
                    if (ds.dialogue[currentTextIndex + 1] == "")
                    {
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


        //for demo only
        AccuseDoppelganger ad = this.gameObject.GetComponent<AccuseDoppelganger>();

        if (ad)
        {
            if (!accused)
            {
                npcName = "Game";
                StartCoroutine(ad.Accuse(npcNameReal));
                accused = true;
            }
            else
            {
                //reset
                accused = false; 
                npcName = npcNameReal;
            }
        }


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
			GUI.skin.label.fontSize = Screen.width / 30;
			int LabelFontSize = GUI.skin.label.fontSize;
			GUI.BeginGroup(new Rect(locationX, locationY, width, height));

			// The big box that contains the dialogue and the image.
			GUI.Box (new Rect(0, 0, width, height), "");

			// Displays text 24 pixels away from the top and bottom.
			GUI.Label(new Rect(LabelFontSize / 2, LabelFontSize / 2, width - LabelFontSize, height - 48), currentText);
			GUI.EndGroup();

			//Display a name tag for the NPC.
            
			// The text will be a part of the box, to allow customization seperate from the label text.
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			// Box will be lined up with the box around the picture, in width and location.
			GUI.BeginGroup (new Rect (locationX, locationY - NpcNameHeight * 1.2f, height, NpcNameHeight * 1.25f));
			GUI.Box(new Rect(0, 0, height - LabelFontSize, NpcNameHeight * 1.2f), "");
			GUI.Label (new Rect (0,0, height - LabelFontSize, NpcNameHeight * 1.2f), npcName);
			GUI.EndGroup ();
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
		}
	}
		

	void Update(){
		if (Input.GetKeyDown(KeyCode.Space) && talking && spacePress)
		{
			advance = true;
		}
	}

	void LateUpdate()
	{
		// Put in lateupdate to prevent conflict with pause menu and other types of
		if (EventSystem.current.currentSelectedGameObject == null && choosing && ButtonOne.activeInHierarchy) {
			EventSystem.current.SetSelectedGameObject (ButtonOne);
		}		
		if (Input.GetKeyUp(KeyCode.Space) && talking && !spacePress)
		{
			spacePress = true;
		}
	}

    public void EndDialogue()
    {
        if (talking)
        {
            talking = false;

            pauseMenu.GetComponent<PauseMenuController>().ToggleTalking();
            player.GetComponent<PlayerSpriteController>().EnableMovement(true);

            //reset
            accused = false;
            npcName = npcNameReal;
        }
    }

    public void Reinitialize()
    {
        Awake();
        Start();
    }
}
