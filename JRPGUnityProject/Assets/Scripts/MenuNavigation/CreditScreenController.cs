using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CreditScreenController : MonoBehaviour {

    GameObject backButton;
    EventSystem eventSystem;

	// Use this for initialization
	void Start () {
	    backButton = GameObject.Find("Back Button");
        GameObject temp = GameObject.Find("EventSystem");
        eventSystem = temp.GetComponent<EventSystem>();
	}
	
	public void LoadScene () {
	    if(eventSystem.currentSelectedGameObject == backButton)
        {
            SceneManager.LoadScene("TitleMenu");
        }
	}
}
