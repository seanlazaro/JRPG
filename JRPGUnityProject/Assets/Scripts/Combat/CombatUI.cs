using UnityEngine;
using System.Collections;
using System;

public class CombatUI : Singleton<CombatUI> {

	bool talking;
	int fontSize;
	int fontSpacing;
	int dialogueWidth;
	int dialogueHeight;
	string message;
	GUIStyle displayStyle = new GUIStyle();

	void Start()
	{
		//To prevent singleton from being destroyed and causing errors.
		DontDestroyOnLoad(this);
		//For Displaying Text
		talking = false;
		fontSize = (int)Math.Floor((decimal)((Screen.height + Screen.width) / 60));
		fontSpacing = fontSize / 2 + 1;
		dialogueHeight = fontSize;
	}
	
    void OnGUI () 
    {
        if (talking)
        {
			dialogueHeight = fontSize;
			if (message.Length * fontSpacing < Screen.width / 3)
				dialogueWidth = message.Length * fontSpacing;
			else {
				dialogueWidth = (int)(Screen.width / 3);
				dialogueHeight = (int)((dialogueHeight+5) * Math.Floor (message.Length * fontSpacing / (Screen.width / 3)+1.0));
			}
			//Starter rect (screen.width / 2 - dialogue width/2 is the left side of rect
			GUI.BeginGroup(new Rect(Screen.width / 2 - dialogueWidth / 2
				, Screen.height / 7,
            dialogueWidth + 10, dialogueHeight + 10));
            //The background box
			GUI.Box(new Rect(0, 0, dialogueWidth, dialogueHeight), "");

			//Layout end
			GUI.Label(new Rect(0, 0, dialogueWidth, dialogueHeight) ,
				message, displayStyle);
			//Layout end
			GUI.EndGroup();
        }
    }

    public IEnumerator DisplayMessage(string messageInput, float displayTime)
    {
        GUI.color = Color.white;
        displayStyle.alignment = TextAnchor.UpperCenter;
        displayStyle.fontSize = fontSize;
		displayStyle.wordWrap = true;
		message = messageInput;
        talking = true;
        yield return new WaitForSeconds(displayTime);
        talking = false;
    }

    public IEnumerator DisplayMessage(string messageInput, float displayTime, Color color)
    {
        GUI.color = color;
        displayStyle.alignment = TextAnchor.UpperCenter;
        displayStyle.fontSize = fontSize;
		displayStyle.wordWrap = true;
        message = messageInput;
        talking = true;
        yield return new WaitForSeconds(displayTime);
        talking = false;
    }

    // For displaying health bar.
    GameObject healthBar;

    public IEnumerator UpdateHealthBar(double health, double maxHealth, bool playerDamaged)
    {
        if (playerDamaged)
        {
            healthBar = GameObject.Find("/Canvas/Health Bar");
        }
        else
        {
            healthBar = GameObject.Find("/Canvas/Enemy Health Bar");
        }
        healthBar.GetComponent<UpdateHealth>().UpdateText((int)health, (int)maxHealth);
        healthBar.GetComponent<UpdateHealth>().UpdateBar((float)health / (float)maxHealth);
        yield break;
    }
}
