using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleScreenController : MonoBehaviour {

    GameObject startButton;
    GameObject creditsButton;
    EventSystem eventSystem;

    void Start()
    {
        startButton = GameObject.Find("Go To Game Button");
        creditsButton = GameObject.Find("Credits Button");
        GameObject temp = GameObject.Find("Event System");
        eventSystem = temp.GetComponent<EventSystem>();
    }
    public void ChangeScene()
    {
        if(eventSystem.currentSelectedGameObject == startButton)
        {
            SceneManager.LoadScene("main");
        }
        else if(eventSystem.currentSelectedGameObject == creditsButton)
        {
            SceneManager.LoadScene("credits");
        }

    }
}
