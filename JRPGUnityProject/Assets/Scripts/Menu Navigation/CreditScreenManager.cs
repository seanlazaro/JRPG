using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class CreditScreenManager : MonoBehaviour {

    GameObject goToTitleScreen;
    EventSystem eventSystem;

	// Use this for initialization
	void Start () {
	    goToTitleScreen = GameObject.Find("Main Menu Button");
        GameObject temp = GameObject.Find("EventSystem");
        eventSystem = temp.GetComponent<EventSystem>();
	}
	
	public void ChangeScene () {
	    if(eventSystem.currentSelectedGameObject == goToTitleScreen)
        {
            SceneManager.LoadScene("TitleMenu");
        }
	}
}
