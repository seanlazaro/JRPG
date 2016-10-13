using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreditScreenController : MonoBehaviour {

    GameObject backButton;

    void Start()
    {
        backButton = GameObject.Find("Main Menu Button");
        backButton.GetComponent<Button>().onClick.AddListener(() => OnBackClicked());
    }
    
    void OnBackClicked()
    {
        SceneManager.LoadScene("TitleMenu");
    }

	void Update()
	{
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(backButton);
        }
	}
}
