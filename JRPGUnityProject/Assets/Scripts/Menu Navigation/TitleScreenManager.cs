using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class TitleScreenManager : MonoBehaviour {

    GameObject goToMain;
    GameObject goToCredits;
    EventSystem eventSystem;

    void Start()
    {
        goToMain = GameObject.Find("Go To Game Button");
        goToCredits = GameObject.Find("Credits Button");
        GameObject temp = GameObject.Find("Event System");
        eventSystem = temp.GetComponent<EventSystem>();
    }
    public void ChangeScene()
    {
        if(eventSystem.currentSelectedGameObject == goToMain)
        {
            SceneManager.LoadScene("main");
        }
        else if(eventSystem.currentSelectedGameObject == goToCredits)
        {
            SceneManager.LoadScene("credits");
        }

    }
}
