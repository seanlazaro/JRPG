using UnityEngine;
using System.Collections;

public class CombatUI : Singleton<CombatUI> {

    //For Displaying Text
    bool talking = false;
    int fontSpacing = 16;
    int fontSize = 32;
    int dialogueWidth;
    int dialogueHeight = 32;
    string message;
    GUIStyle displayStyle = new GUIStyle();

    protected CombatUI() { }
	
    void OnGUI () 
    {
        if (talking)
        {
            dialogueWidth = message.Length * fontSpacing;
            GUI.BeginGroup(new Rect(Screen.width / 2 - dialogueWidth / 2, 50,
            dialogueWidth + 10, dialogueHeight));
            //The background box
            GUI.Box(new Rect(0, 0, dialogueWidth, dialogueHeight), "");
            //The conversation text
            GUI.Label(new Rect(0, 0, dialogueWidth, fontSize),
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
            Debug.Log("I Updated The Player");
        }
        else
        {
            healthBar = GameObject.Find("/Canvas/Enemy Health Bar");
            Debug.Log("I Updated The Enemy");
        }
        healthBar.GetComponent<UpdateHealth>().UpdateText((int)health, (int)maxHealth);
        healthBar.GetComponent<UpdateHealth>().UpdateBar((float)health / (float)maxHealth);
        yield break;
    }
}
